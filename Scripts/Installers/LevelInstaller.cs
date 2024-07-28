using Cinemachine;
using Infrastructure;
using UnityEngine;

namespace Installers
{
    public class LevelInstaller : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup _cameraTargets;
        [SerializeField] private Transform[] _playerSpawns;
        [SerializeField] private PlayerInstaller _playerTemplate;
        [SerializeField] private float _centerCameraWeight;
        [SerializeField] private float _playerCameraWeight;

        private void Start()
        {
            InitializePlayers();
            Time.timeScale = 1f;
        }

        public void InitializePlayers()
        {
            PlayerConfiguration[] playerConfigs = PlayerConfigurationsManager.Instance.GetPlayerConfigurations();

            _cameraTargets.AddMember(transform, _centerCameraWeight, 0);

            foreach (PlayerConfiguration playerConfig in playerConfigs)
            {
                int position = playerConfig.Position;

                PlayerInstaller player = Instantiate(_playerTemplate, _playerSpawns[position].position, _playerSpawns[position].rotation);
                player.Initialize(playerConfig, GetComponent<DeathListener>(), GetComponent<PauseListener>());

                _cameraTargets.AddMember(player.transform, _playerCameraWeight, 0);
            }
        }
    }
}