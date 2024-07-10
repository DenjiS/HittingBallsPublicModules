using Steamworks;
using UnityEngine;

namespace Infrastructure
{

    public class SteamManager : Singleton<SteamManager>
    {
        [SerializeField] private uint _appId;

        protected override void Awake()
        {
            base.Awake();

            try
            {
                SteamClient.Init(_appId);
            }
            catch (System.Exception exception)
            {
                Debug.LogException(exception);

                Application.Quit();
            }
        }

        protected override void OnDestroy()
        {
            ClientShutdown();
            base.OnDestroy();
        }

        private void OnApplicationQuit()
        {
            ClientShutdown();
        }

        private void ClientShutdown()
        {
            if (Instance == this)
                SteamClient.Shutdown();
        }
    }
}
