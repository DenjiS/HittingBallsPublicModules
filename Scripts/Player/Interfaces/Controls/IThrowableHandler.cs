using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public interface IThrowableHandler : ITickable
    {
        public bool HasThrowable { get; }

        public void OnThrowInput(CallbackContext context);

        public void SetGrenade(IThrowable grenade);
    }
}