using Controls;
using Infrastructure;
using Player;
using Player.UIGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Installers
{
    /// <summary>
    /// Installs and all player components after level loaded
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInstaller : MonoBehaviour
    {
        private const string KeyboardMouseScheme = "KeyboardMouse";
        private const string GamepadScheme = "Gamepad";

        [SerializeField] private PlayerCustomizationInstaller _customization = new();
        [SerializeField] private PlayerEyebrowsEmotions _emotions = new();

        [Header("View")]
        [SerializeField] private PlayerUIView _viewTemplate;

        [Header("Movement")]
        [SerializeField] private Rigidbody _playerBody;
        [SerializeField] private float _speed = 3;
        [SerializeField] private MoveDasher _dasher = new();

        [Header("Aiming")]
        [SerializeField] private Transform _aimPoint;
        [SerializeField] private PidRegulator _aimPidRegulator = new();
        [SerializeField] private float _aimRotationForce;
        [SerializeField] private float _gamepadDeadZone;

        [Header("Health")]
        [SerializeField][Min(1)] private float _maxHealth;

        [Header("Shooting")]
        [SerializeField] private WeaponHandler _weaponHandler = new();

        [Header("Throwing")]
        [SerializeField] private ThrowableHandler _grenadeHandler = new();
        [SerializeField] private TrajectoryRenderer _trajectoryRenderer = new();

        private PlayerController _controller;

        private PlayerConfiguration _configuration;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _playerBody = GetComponent<Rigidbody>();
        }

        private void OnDestroy()
        {
            _configuration.Input.onActionTriggered -= _controller.OnActionTriggered;

            if (_aimPoint != null)
                Destroy(_aimPoint.gameObject);
        }

        /// <summary>
        /// Initiliazes player, his components and binds him to level systems
        /// </summary>
        /// <param name="configuration">global player configuration</param>
        /// <param name="deathListener">death tracking system</param>
        /// <param name="pauseListener">pause control system</param>
        public void Initialize(PlayerConfiguration configuration, DeathListener deathListener, PauseListener pauseListener)
        {
            _configuration = configuration;

            _customization.Initialize(_configuration);
            _emotions.Initialize(this);

            PlayerUIView view = Instantiate(_viewTemplate, null);
            view.Initialize(transform, configuration.Color, configuration.Number);

            Mover mover = new(_playerBody, _speed);
            _dasher.Initialize(mover);

            IAimingService aimer = CreateAimingService(_configuration.Input);

            Health health = new(view, _maxHealth);

            _weaponHandler.Initialize(view.AmmoView, _emotions, _playerBody);

            _trajectoryRenderer.Initialize();
            _grenadeHandler.InitializeTrajectoryRenderer(_trajectoryRenderer);

            deathListener?.SubscribeToDeath(health, _configuration);
            pauseListener?.SubscribeToPause(_controller.ClickedPause);

            health.Died += _controller.OnPlayerDied;

            _controller.Initialize(mover, aimer, health, _weaponHandler, _grenadeHandler, _dasher, _emotions);

            _configuration.Input.onActionTriggered += _controller.OnActionTriggered;

            _aimPoint.SetParent(null);
        }

        /// <summary>
        /// Creates an aiming service for player's current control scheme
        /// </summary>
        /// <param name="input">Current player's input data</param>
        /// <returns>Character aiming control service</returns>
        private IAimingService CreateAimingService(PlayerInput input) => input.currentControlScheme switch
        {
            KeyboardMouseScheme => new MouseAimer(_aimPoint, _aimPidRegulator, _aimRotationForce),
            GamepadScheme => new GamepadAimer(_aimPoint, _aimPidRegulator, _aimRotationForce, _gamepadDeadZone),
            _ => new GamepadAimer(_aimPoint, _aimPidRegulator, _aimRotationForce, _gamepadDeadZone),
        };
    }
}