using Infrastructure;
using Player;
using RayFire;
using UnityEngine;

namespace Installers
{
    public class NonPlayableInstaller : MonoBehaviour
    {
        [SerializeField] private PlayerCustomizationInstaller _customization = new();
        [SerializeField] private bool _isDemolishable;

        [SerializeField] private RayfireRigid _rfRigid;

        private void OnCollisionEnter(Collision collision)
        {
            if (_isDemolishable && collision.gameObject.TryGetComponent<Rigidbody>(out _))
            {
                _rfRigid.Demolish();
            }
        }

        public void Initialize(PlayerConfiguration configuration)
        {
            _customization.Initialize(configuration);
        }
    }
}
