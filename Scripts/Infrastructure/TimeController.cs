using UnityEngine;

namespace Infrastructure
{
    public class TimeController : MonoBehaviour
    {
        public void TimeRun()
        {
            Time.timeScale = 1f;
        }

        public void TimeStop()
        {
            Time.timeScale = 0f;
        }
    }
}