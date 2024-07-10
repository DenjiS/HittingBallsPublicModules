using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class LevelsLoader : Singleton<LevelsLoader>
    {
        private const string LevelLoadedAnimatorParameter = "IsLevelLoaded";

        [SerializeField, HideInInspector] private string _launchSceneName;
        [SerializeField, HideInInspector] private string _winnerSceneName;
        [SerializeField, HideInInspector] private RandomArray<string> _sceneNames;

#if UNITY_EDITOR
        [Header("Configuration")]
        [SerializeField] private UnityEditor.SceneAsset _launchScene;
        [SerializeField] private UnityEditor.SceneAsset _winnerScene;
        [SerializeField] private UnityEditor.SceneAsset[] _scenes;
#endif

        [Header("Transition animations")]
        [SerializeField] private float _duration;

        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponent<Animator>();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected override void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            base.OnDestroy();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _launchSceneName = _launchScene.name;
            _winnerSceneName = _winnerScene.name;
            _sceneNames = new RandomArray<string>(_scenes.Select(scene => scene.name));
        }
#endif

        public void LoadLaunchScene() =>
            StartCoroutine(LevelLoading(_launchSceneName));

        public void LoadWinnerScene() =>
            StartCoroutine(LevelLoading(_winnerSceneName));

        public void LoadNext() =>
            StartCoroutine(LevelLoading(_sceneNames.GetNewRandom()));

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _animator.SetBool(LevelLoadedAnimatorParameter, true);
        }

        private IEnumerator LevelLoading(string scene)
        {
            _animator.SetBool(LevelLoadedAnimatorParameter, false);

            yield return new WaitForSeconds(_duration);

            SceneManager.LoadScene(scene);
        }
    }
}