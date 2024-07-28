using UnityEngine;

namespace Infrastructure
{
    public class ExitHandler : MonoBehaviour
    {
        public void ExitApplication()
        {
            Application.Quit();
        }

        public void ExitSession()
        {
            LevelsLoader.Instance.LoadLaunchScene();
        }
    }
}