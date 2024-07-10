using Infrastructure;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    [System.Serializable]
    public class PlayerCustomizationInstaller
    {
        [SerializeField] private DecalProjector _eyesDecal;
        [SerializeField] private DecalProjector _mouthDecal;
        [SerializeField][Range(0, 1)] private float _shading;

        [SerializeField] private MeshRenderer _mesh;

        public void Initialize(PlayerConfiguration configuration)
        {
            SetFaceColor(configuration);
            SetCustomizationElements(configuration);
        }

        private void SetFaceColor(PlayerConfiguration configuration)
        {
            Color color = Color.Lerp(configuration.Color, Color.black, _shading);
            _eyesDecal.material = CreateColoredMaterial(_eyesDecal.material, color);
            _mouthDecal.material = CreateColoredMaterial(_mouthDecal.material, color);
        }

        private void SetCustomizationElements(PlayerConfiguration configuration)
        {
            _eyesDecal.material = configuration.Eyes.Element;
            _mouthDecal.material = configuration.Mouth.Element;
            _mesh.sharedMaterial = new Material(configuration.Skin.Element);
        }

        /// <summary>
        /// Creates a material for face decals with player color
        /// </summary>
        /// <param name="material"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private Material CreateColoredMaterial(Material material, Color color)
        {
            Material newMaterial = new(material);
            newMaterial.color = color;
            return newMaterial;
        }
    }
}