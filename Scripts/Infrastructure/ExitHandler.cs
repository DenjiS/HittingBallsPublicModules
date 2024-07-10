using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class ExitHandler : MonoBehaviour
    {
        private const string LaunchSceneName = "LaunchScene";

        public void ExitApplication()
        {
            Application.Quit();
        }

        public void ExitSession()
        {
            SceneManager.LoadScene(LaunchSceneName);
        }
    }
}