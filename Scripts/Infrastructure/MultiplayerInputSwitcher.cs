using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure
{
    public class MultiplayerInputSwitcher : MonoBehaviour
    {
        private const string PlayerMapName = "Player";

        private IEnumerable<PlayerConfiguration> _configs;

        private GameObject _playersManagerObject;

        private void Awake()
        {
            PlayerConfigurationsManager configsManager = FindObjectOfType<PlayerConfigurationsManager>();
            _playersManagerObject = configsManager.gameObject;
            _configs = configsManager.GetPlayerConfigurations();
        }

        public void SetEnabled(bool isEnabled)
        {
            _playersManagerObject.SetActive(isEnabled);

            if (isEnabled)
            {
                foreach (PlayerConfiguration config in _configs)
                {
                    PlayerInput input = config.Input;
                    input.SwitchCurrentActionMap(PlayerMapName);
                    input.SwitchCurrentControlScheme(config.ControlScheme, config.Devices);
                }
            }
        }
    }
}