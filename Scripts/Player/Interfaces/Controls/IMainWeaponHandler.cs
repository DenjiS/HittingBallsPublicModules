using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public interface IMainWeaponHandler : System.IDisposable
    {
        event UnityAction DroppedByWeapon;

        public void SetWeapon(IWeapon weapon);

        public void OnShootInput(CallbackContext context);

        public void OnReloadInput(CallbackContext context);

        public void OnDropInput(CallbackContext context);

        public void Drop();
    }
}