using UnityEngine;

namespace Infrastructure
{
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError(string.Format("Multiple instances of {0} are not allowed", typeof(T)));

                Destroy(this);

                return;
            }

            Instance = (T)this;

            transform.SetParent(null);
            DontDestroyOnLoad(this);
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
