using UnityEngine;
using UnityEngine.AddressableAssets;


#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace Infrastructure
{
    [CreateAssetMenu(fileName = "NewLevelsConfiguration", menuName = "Configurations/LevelsConfiguration")]
    public class LevelsConfiguration : ScriptableObject
    {
        [SerializeField] private RandomArray<AssetReference> _sceneReferences;

        internal RandomArray<AssetReference> SceneReferences => _sceneReferences;
    }
}