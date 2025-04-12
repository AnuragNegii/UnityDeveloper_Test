using UnityEngine;

public class PlayerScript : MonoBehaviour{

    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform cam; 

    [Header("Ground Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundDrag = 7f;
    [SerializeField] private bool isGrounded;
    [SerializeField]private float groundCheckHeight = 0.3f;

    private Rigidbody rb;
    private Vector2 inputVector;
    private Vector3 direction;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private bool isMoving;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    private void Update(){
        inputVector = gameInput.GetMovementVector();
        direction = new Vector3(inputVector.x, 0f, inputVector.y);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckHeight, whatIsGround);

        if (isGrounded){
            rb.drag = groundDrag;
        }else{
            rb.drag = 0f;
        }

        SpeedControl();
        isMoving = inputVector != Vector2.zero && isGrounded;
    }
    private void FixedUpdate(){
        MovePlayer();
    }

    private void MovePlayer(){
        if (inputVector.magnitude > 0.1f && isGrounded){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force );
        }
    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public bool IsMoving(){
        return isMoving;
    }

    public bool IsGrounded(){
        return isGrounded;
    }
}
