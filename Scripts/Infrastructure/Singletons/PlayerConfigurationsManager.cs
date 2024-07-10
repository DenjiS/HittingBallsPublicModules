using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

namespace Infrastructure
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerConfigurationsManager : Singleton<PlayerConfigurationsManager>
    {
        [SerializeField] private Color[] _colors = new Color[PlayerAmount];

        private const string PlayerMapName = "Player";
        private const int PlayerAmount = 4;

        private List<PlayerConfiguration> _configs = new();
        private PlayerInputManager _inputManager;

        protected override void Awake()
        {
            base.Awake();
            _inputManager = GetComponent<PlayerInputManager>();
        }

        public PlayerConfiguration[] GetPlayerConfigurations()
        {
            return _configs.ToArray();
        }

        public void SetPlayerElement<T>(int index, T element) where T : CustomizationElement<Material>
        {
            switch (element)
            {
                case SkinElement skin:
                    _configs[index].Skin = skin;
                    break;

                case EyesElement eyes:
                    _configs[index].Eyes = eyes;
                    break;

                case MouthElement mouth:
                    _configs[index].Mouth = mouth;
                    break;
            }
        }

        public void SetPlayerReady(int index, out bool isReady)
        {
            _configs[index].IsReady = !_configs[index].IsReady;
            isReady = _configs[index].IsReady;

            if (_configs.Count > 1
                && _configs.All(player => player.IsReady))
            {
                foreach (PlayerInput input in _configs.Select(config => config.Input))
                    input.SwitchCurrentActionMap(PlayerMapName);

                _inputManager.DisableJoining();

                LevelsLoader.Instance.LoadNext();
            }
        }

        public void SetPlayerNextPosition(int index, out int position)
        {
            if (_configs.Count == PlayerAmount && _configs[index].Position != -1) // Loop danger
            {
                position = _configs[index].Position;
                return;
            }

            int rawPosition = _configs[index].Position;

            while (_configs.Any(config => config.Position == rawPosition))
            {
                rawPosition++;

                if (rawPosition > PlayerAmount - 1)
                    rawPosition = 0;
            }

            position = rawPosition;
            _configs[index].Position = position;
        }

        public virtual void HandlePlayerJoin(PlayerInput input)
        {
            if (_configs.Any(config => config.Input.playerIndex == input.playerIndex) == false)
            {
                input.transform.SetParent(transform);

                PlayerConfiguration config = new(input)
                {
                    Color = _colors[input.playerIndex],
                    Number = input.playerIndex + 1
                };

                _configs.Add(config);
            }
        }

        public void ClearInputs()
        {
            foreach (PlayerConfiguration config in _configs)
                Destroy(config.Input);
        }
    }
}
