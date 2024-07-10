using static UnityEngine.InputSystem.InputAction;

public interface IMovementService : IFixedTickable
{
    void OnMovementInput(CallbackContext context);
}
