using UnityEngine;

public class GameInput : MonoBehaviour{
    private PlayerInputAction playerInputAction;
    private void Awake(){
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Movement.Enable();
    }

    public Vector2 GetMovementVector(){
        Vector2 movementVector = playerInputAction.Player.Movement.ReadValue<Vector2>();
        return movementVector.normalized;
    }
}
