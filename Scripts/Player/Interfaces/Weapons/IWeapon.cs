using UnityEngine.Events;

namespace Player
{
    public interface IWeapon : IPhysics
    {
        public event UnityAction SelfDropped;

        public void OnActionPerformed();

        public void OnActionCancelled();

        public void OnReloadPressed();

        public void BindPlayer(IWeaponView view, PlayerEyebrowsEmotions emotions);

        public void UnbindPlayer();
    }
}