using UnityEngine;
using System;

public class GameInput : MonoBehaviour{

    public PlayerInputAction PlayerInputAction{get; private set;}
    public event EventHandler OnJumpPerformed;
    public event EventHandler OnUpPerformed;
    public event EventHandler OnDownPerformed;
    public event EventHandler OnLeftPerformed;
    public event EventHandler OnRightPerformed;
    public event EventHandler OnEnterPerformed;

    private void Awake(){
        PlayerInputAction = new PlayerInputAction();
        PlayerInputAction.Player.Enable();

        PlayerInputAction.Player.Jump.performed += PlayerInputAction_JumpPerformed;
        PlayerInputAction.Player.Up.performed += PlayerInputAction_OnUpPerformed; 
        PlayerInputAction.Player.Down.performed += PlayerInputAction_OnDownPerformed;
        PlayerInputAction.Player.Left.performed += PlayerInputAction_OnLeftPerformed;
        PlayerInputAction.Player.Right.performed += PlayerInputAction_OnRightPerformed;
        PlayerInputAction.Player.Enter.performed += PlayerInputAction_OnEnterPerformed;
    }

    public Vector2 GetMovementVector(){
        Vector2 movementVector = PlayerInputAction.Player.Movement.ReadValue<Vector2>();
        return movementVector.normalized;
    }

    private void PlayerInputAction_JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context){
        OnJumpPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerInputAction_OnUpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context){
        OnUpPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerInputAction_OnDownPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context){
        OnDownPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerInputAction_OnLeftPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context){
        OnLeftPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerInputAction_OnRightPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context){
        OnRightPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerInputAction_OnEnterPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context){
        OnEnterPerformed?.Invoke(this, EventArgs.Empty);
    }
}
