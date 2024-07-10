using Cinemachine;
using Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Installers
{
    public class WinnerLevelInstaller : MonoBehaviour
    {
        private const int LosersMaxAmount = 3;

        [Header("Templates")]
        [SerializeField] private NonPlayableInstaller _nonPlayableWinnerTemplate;
        [SerializeField] private NonPlayableInstaller _nonPlayableLoserTemplate;

        [Header("Positions")]
        [SerializeField] private Transform _winnerPosition;
        [SerializeField] private Transform[] _losersPositions = new Transform[LosersMaxAmount];

        [Header("Animation")]
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _sceneDuration;

        private PlayerConfiguration _winnerConfig;
        private PlayerConfiguration[] _losersConfigs;

        private void Awake()
        {
            IEnumerable<PlayerConfiguration> players = PlayerConfigurationsManager.Instance.GetPlayerConfigurations();

            _winnerConfig = players.OrderByDescending(config => config.Score).First();
            _losersConfigs = players.Where(config => config != _winnerConfig).ToArray();
        }

        private void Start()
        {
            NonPlayableInstaller player = InitializeNonPlayable(_winnerConfig, _winnerPosition, _nonPlayableWinnerTemplate);
            InitializeLosers();

            _camera.LookAt = player.transform;

            StartCoroutine(LaunchSceneLoading());
        }

        private void FixedUpdate()
        {
            
        }

        private void InitializeLosers()
        {
            for (int i = 0; i < _losersConfigs.Length; i++)
            {
                InitializeNonPlayable(_losersConfigs[i], _losersPositions[i], _nonPlayableLoserTemplate);
            }
        }

        private NonPlayableInstaller InitializeNonPlayable(PlayerConfiguration config, Transform point, NonPlayableInstaller template)
        {
            NonPlayableInstaller nonPlayable = Instantiate(template, point);
            nonPlayable.Initialize(config);
            return nonPlayable;
        }

        private IEnumerator LaunchSceneLoading()
        {
            yield return new WaitForSeconds(_sceneDuration);
            LevelsLoader.Instance.LoadLaunchScene();
        }
    }
}