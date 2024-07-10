using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public class LogoSceneController : MonoBehaviour
    {
        [SerializeField] private string _mainSceneName;
        [SerializeField] private float _duration;

        private void Start()
        {
            MusicManager.Instance.GetComponent<AudioSource>().enabled = false;
            StartCoroutine(Passing());
        }

        private IEnumerator Passing()
        {
            yield return new WaitForSeconds(_duration);
            MusicManager.Instance.GetComponent<AudioSource>().enabled = true;
            LevelsLoader.Instance.LoadLaunchScene();
        }
    }
}