using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public class LogoSceneController : MonoBehaviour
    {
        [SerializeField] private float _duration;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_duration);
            MusicManager.Instance.Launch();
            LevelsLoader.Instance.LoadLaunchScene();
        }
    }
}