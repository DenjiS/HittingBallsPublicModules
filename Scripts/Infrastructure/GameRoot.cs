using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Infrastructure
{
    public class GameRoot : MonoBehaviour
    {
        [Tooltip("Singletons only")]
        [SerializeField] private AssetReference[] _globalManagers;

        private List<AsyncOperationHandle> _loadHandles = new();

        private void Awake()
        {
            foreach (AssetReference managerReference in _globalManagers)
                _loadHandles.Add(Addressables.InstantiateAsync(managerReference));
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _loadHandles.All(loadHandle => loadHandle.IsDone));
            LevelsLoader.Instance.LoadFirstScene(GetComponent<AudioListener>());
        }
    }
}