using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure
{
    public class PlayerConfigurationCleaner : MonoBehaviour
    {
        [SerializeField] private PlayerConfigurationsManager _configurationsManagerTemplate;

        public void Clear()
        {
            DestroyImmediate(PlayerConfigurationsManager.Instance.gameObject);
            Instantiate(_configurationsManagerTemplate);
        }
    }
}
