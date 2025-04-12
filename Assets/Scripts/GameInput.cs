using UnityEngine;

public class GameInput : MonoBehaviour{
    public PlayerInputAction PlayerInputAction{get; private set;}
    private void Awake(){
        PlayerInputAction = new PlayerInputAction();
        PlayerInputAction.Player.Enable();
    }

    public Vector2 GetMovementVector(){
        Vector2 movementVector = PlayerInputAction.Player.Movement.ReadValue<Vector2>();
        return movementVector.normalized;
    }
}
