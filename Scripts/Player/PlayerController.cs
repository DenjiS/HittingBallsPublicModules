using Player.EffectStates;
using RayFire;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Utils;

namespace Player
{
    /// <summary>
    /// Main controller of player character
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour, IDamageableHealable, IPartsForEffects
    {
        private const string DamagedTrigger = "Damaged";

        [Header("Visual Effects")]
        [SerializeField] private VisualEffect _dashEffect;

        [Header("Sounds")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SoundData _takeDamageSound;
        [SerializeField] private SoundData _deathSound;
        [SerializeField] private SoundData _pickSound;
        [SerializeField] private SoundData _dropSound;
        [SerializeField] private SoundData _dashSound;

        [Header("Interactions")]
        [SerializeField] private float _pickIgnoreTime;
        [SerializeField] private float _dashIgnoreTime;
        [SerializeField] private float _damageMuteTime;

        [Header("Collision Damage")]
        [SerializeField] private float _minCollisionDamage;
        [SerializeField] private float _maxCollisionDamage;
        [SerializeField] private float _minDamagingSpeed;
        [SerializeField] private float _maxDamagingSpeed;

        private IMovementService _mover;
        private IAimingService _aimer;

        private IDamageableHealable _health;
        private IMainWeaponHandler _weaponHandler;
        private IThrowableHandler _grenadeHandler;
        private IDashService _dasher;

        private Transform _transform;
        private Animator _animator;

        private PlayerEyebrowsEmotions _emotions;
        private RayfireRigid _rayfireRigid;

        private ProjectInputActions.PlayerActions _actionMap;

        private CooldownTimer _pickWeaponCooldown;
        private CooldownTimer _dashCooldown;
        private CooldownTimer _damageMuteCooldown;

        private bool _canPickWeapon = true;
        private bool _canPickGrenade = true;

        public UnityEvent ClickedPause;

        public Transform BaseTransform => _transform;

        private void Awake()
        {
            _transform = transform;
            _animator = GetComponent<Animator>();
            _rayfireRigid = GetComponent<RayfireRigid>();

            _actionMap = new ProjectInputActions().Player;

            _pickWeaponCooldown = new CooldownTimer(this, _pickIgnoreTime);
            _pickWeaponCooldown.Finished += () => _canPickWeapon = true;

            _dashCooldown = new CooldownTimer(this, _dashIgnoreTime);
            _damageMuteCooldown = new CooldownTimer(this, _damageMuteTime);
        }

        private void OnDisable()
        {
            _emotions.Dispose();
            _pickWeaponCooldown.Dispose();
        }

        private void OnDestroy()
        {
            _weaponHandler.Dispose();
        }

        private void Update()
        {
            _grenadeHandler?.Tick();
        }

        private void FixedUpdate()
        {
            _mover?.FixedTick();
            _aimer?.FixedTick();
        }

        private void OnTriggerStay(Collider other) // TODO Move this logic to spawners (Should I??)
        {
            if (_canPickWeapon
                && other.TryGetComponent(out IWeaponSpawner weaponSpawner)
                && weaponSpawner.TryGet(out IWeapon spawnedWeapon))
            {
                _weaponHandler.SetWeapon(spawnedWeapon);

                _canPickWeapon = false;

                _pickSound.PlayBy(_audioSource);
            }
            else if (_grenadeHandler.HasThrowable == false
                && other.TryGetComponent(out IThrowableSpawner grenadeSpawner)
                && grenadeSpawner.TryGet(out IThrowable grenade))
            { 
                if (grenade is ITeleportThrowable teleportThrowable)
                    teleportThrowable.SetPlayerTransform(BaseTransform);

                _grenadeHandler.SetGrenade(grenade);

                _pickSound.PlayBy(_audioSource);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Rigidbody collisionBody = collision.rigidbody;

            if (ReferenceEquals(collisionBody, null))
            {
                return;
            }
            else if (_canPickWeapon && collisionBody.TryGetComponent(out IWeapon weapon))
            {
                _weaponHandler.SetWeapon(weapon);
                _canPickWeapon = false;

                _pickSound.PlayBy(_audioSource);
            }
            else
            {
                TakeDamageFromCollision(collision.relativeVelocity.magnitude);
            }
        }

        public void Initialize(
            IMovementService mover,
            IAimingService aimer,
            IDamageableHealable health,
            IMainWeaponHandler shooter,
            IThrowableHandler thrower,
            IDashService dasher,
            PlayerEyebrowsEmotions emotions
            )
        {
            _mover = mover;
            _aimer = aimer;

            _health = health;
            _weaponHandler = shooter;
            _grenadeHandler = thrower;
            _dasher = dasher;

            _emotions = emotions;

            _weaponHandler.DroppedByWeapon += () => _canPickWeapon = true;
        }

        /// <summary>
        /// This method subscribes for <see cref="PlayerInput.onActionTriggered"/> event. The subscription takes place in installer class.
        /// </summary>
        /// <param name="context">Callback context</param>
        public void OnActionTriggered(InputAction.CallbackContext context)
        {
            string actionName = context.action.name;

            if (actionName == _actionMap.Movement.name)
            {
                _mover.OnMovementInput(context);
            }
            else if (actionName == _actionMap.Aiming.name)
            {
                _aimer.OnAimingInput(context);
            }
            else if (actionName == _actionMap.Shoot.name)
            {
                _weaponHandler.OnShootInput(context);
            }
            else if (actionName == _actionMap.Reload.name)
            {
                _weaponHandler.OnReloadInput(context);
            }
            else if (actionName == _actionMap.Throw.name)
            {
                _grenadeHandler.OnThrowInput(context);
            }
            else if (actionName == _actionMap.Drop.name)
            {
                _weaponHandler.OnDropInput(context);

                _pickWeaponCooldown.Activate();
            }
            else if (actionName == _actionMap.Pause.name)
            {
                if (context.performed == true)
                    ClickedPause?.Invoke();
            }
            else if (actionName == _actionMap.Dash.name)
            {
                if (_dashCooldown.Activate() == false)
                    return;

                _dasher.OnDashInput(context);
                _dashEffect.Play();
                _dashSound.PlayBy(_audioSource);
            }
            else
            {
                Debug.Log($"{context}{context.action.name} not found");
            }
        }

        public void TakeDamage(float damage)
        {
            if (_damageMuteCooldown.Activate())
                _takeDamageSound.PlayBy(_audioSource);

            _animator.SetTrigger(DamagedTrigger);
            _emotions.ShowEmotion(PlayerEyebrowsEmotions.Emotions.Sad);

            _health.TakeDamage(damage);
        }

        public void TakeHeal(float health)
        {
            _health.TakeHeal(health);
        }

        public void OnPlayerDied(IDeathNotifier health)
        {
            _weaponHandler.Drop();

            StopAllCoroutines();

            _audioSource.transform.SetParent(null);
            _deathSound.PlayBy(_audioSource);

            _rayfireRigid.Demolish();
            health.Died -= OnPlayerDied;
        }

        public object[] GetPlayerParts(BaseScriptableEffect effect)
        {
            List<object> parts = new();

            switch (effect)
            {
                case BurnEffect:
                    parts.Add(_health);
                    break;

                case GlueEffect:
                    parts.Add(_mover);
                    break;

                case FreezeEffect:
                    parts.Add(_mover);
                    break;

                default:
                    throw new NotImplementedException("PlayerController cannot handle this Effect");
            }

            return parts.ToArray();
        }

        private void TakeDamageFromCollision(float relativeVelocity)
        {
            if (relativeVelocity > _minDamagingSpeed)
            {
                float damage = Mathf.Lerp(
                    _minCollisionDamage,
                    _maxCollisionDamage,
                    (relativeVelocity - _minDamagingSpeed) / (_maxDamagingSpeed - _minDamagingSpeed)
                    );

                _health.TakeDamage(damage);
            }
        }
    }
}
