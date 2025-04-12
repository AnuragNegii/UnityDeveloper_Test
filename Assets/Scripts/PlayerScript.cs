using UnityEngine;

public class PlayerScript : MonoBehaviour{

    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform cam; 
    private CharacterController characterController;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private bool isMoving;

    private void Awake(){
        characterController = GetComponent<CharacterController>();
    }
    
    private void Update(){
        Vector2 inputVector = gameInput.GetMovementVector();
        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);
        if (inputVector.magnitude > 0.1f){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }
        isMoving = inputVector != Vector2.zero;
    }


    public bool IsMoving(){
        return isMoving;
    }
}
