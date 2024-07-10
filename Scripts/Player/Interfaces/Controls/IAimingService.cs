using UnityEngine;
using UnityEngine.InputSystem;

public interface IAimingService: IFixedTickable
{
    public void OnAimingInput(InputAction.CallbackContext context);
}
