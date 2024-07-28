using static System.TimeSpan;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    [RequireComponent(typeof(Animator))]
    public class LevelsLoader : Singleton<LevelsLoader>
    {
        private const string LevelLoadedAnimatorParameter = "IsLevelLoaded";

        [Header("Configuration")]
        [SerializeField] private AssetReference _logoScene;
        [SerializeField] private AssetReference _launchScene;
        [SerializeField] private AssetReference _winnerScene;
        [SerializeField] private LevelsConfiguration _levels;

        [Header("Transition animations")]
        [SerializeField] private float _duration;

        private Animator _animator;
        private AudioListener _interLevelListener;

        private SceneInstance _currentScene;
        private bool _hasActiveScene = false;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        /// <summary>
        /// Loads first scene. Takes <see cref="AudioListener"/> from Bootstrap for inter-scenes usage.
        /// </summary>
        /// <param name="listener"></param>
        public async void LoadFirstScene(AudioListener listener)
        {
            _interLevelListener = listener;
            await LoadLevelAsync(_logoScene);
        }

        /// <summary>
        /// Loads main menu scene.
        /// </summary>
        public void LoadLaunchScene()
        {
            PerformAnimatedLoading(_launchScene);
        }

        /// <summary>
        /// Loads winner scene after finished session.
        /// </summary>
        public void LoadWinnerScene() =>
            PerformAnimatedLoading(_winnerScene);

        /// <summary>
        /// Loads next playable level.
        /// </summary>
        public void LoadNext() =>
            PerformAnimatedLoading(_levels
                .SceneReferences
                .GetNewRandom());

        /// <summary>
        /// Subscribes on <see cref="SceneManager.sceneLoaded"/>.
        /// Level enter animation and loaded scene activation.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _interLevelListener.enabled = false;

            SceneManager.SetActiveScene(scene);
        }

        /// <summary>
        /// Subscribes on <see cref="SceneManager.sceneUnloaded"/>.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void OnSceneUnloaded(Scene scene)
        {
            _interLevelListener.enabled = true;
        }

        /// <summary>
        /// Plays animation and after loads next level.
        /// </summary>
        /// <param name="sceneReference">Next scene reference.</param>
        /// <returns></returns>
        private async void PerformAnimatedLoading(AssetReference sceneReference) // 2022.3.38 Coroutine sometimes doesn't work, behaves as it wasn't called.
        {
            _animator.SetBool(LevelLoadedAnimatorParameter, false);
            await Task.Delay(FromSeconds(_duration));

            await LoadLevelAsync(sceneReference);

            _animator.SetBool(LevelLoadedAnimatorParameter, true);
        }

        /// <summary>
        /// Asynchronously <b>loads next</b> and <b>unloads previoust</b> scenes.
        /// </summary>
        /// <param name="sceneReference">Next scene reference.</param>
        private async Task LoadLevelAsync(AssetReference sceneReference)
        {
            if (_hasActiveScene)
                await ManageSceneAsyncLoading(Addressables.UnloadSceneAsync(_currentScene), false);

            await ManageSceneAsyncLoading(Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive), true);
        }

        private async Task ManageSceneAsyncLoading(AsyncOperationHandle<SceneInstance> loadingHandle, bool isLoaded)
        {
            await loadingHandle.Task;

            _hasActiveScene = isLoaded;

            if (isLoaded)
                _currentScene = loadingHandle.Result;
            else
                _currentScene = new SceneInstance();
        }
    }
}