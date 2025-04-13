using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour{

    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce= 7f;
    [SerializeField]private float airMultiplier= 0.4f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform cam; 

    [Header("Ground Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundDrag = 3f;
    [SerializeField] private bool isGrounded;
    [SerializeField]private float groundCheckHeight = 0.3f;

    //Shadow Indicator for where gravity is gonna go
    [SerializeField] private GameObject shadowPlayer;

    [SerializeField] private Transform playerVisualMidPoint;
    private float shadowActiveTime = 2.0f;
    private bool shadowActive;

    private Rigidbody rb;
    private Vector2 inputVector;
    private Vector3 direction;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private bool isMoving;

    private float freeFalling = 5.0f;

    public static event Action PlayerDied;
    public static event Action OnBoxDestroyed;

    //Creating custom Gravity with these
    private Vector3 customGravity = Vector3.down;
    private float gravityStrength = 40f;
    private PlayerGravity playerGravity;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
        playerGravity = PlayerGravity.Down;

        shadowPlayer.SetActive(false);

        gameInput.OnEnterPerformed += Enter_Performed;

        gameInput.OnJumpPerformed += Jump_Performed;
        gameInput.OnUpPerformed += Up_Performed;
        gameInput.OnDownPerformed += Down_Performed;
        gameInput.OnLeftPerformed += Left_Performed;
        gameInput.OnRightPerformed += Right_Performed;
    }

    private void Update(){
        inputVector = gameInput.GetMovementVector();
        direction = new Vector3(inputVector.x, 0f, inputVector.y);
        isGrounded = Physics.Raycast(playerVisualMidPoint.position, customGravity.normalized, groundCheckHeight, whatIsGround);
        Debug.DrawRay(playerVisualMidPoint.position, customGravity.normalized * groundCheckHeight, isGrounded ? Color.red: Color.green, 100f);

        if (isGrounded){
            rb.drag = groundDrag;
        }else{
            rb.drag = 0f;
        }

        FreeFalling();
        SpeedControl();

        //gravity indicator activate or deactivate
        if (shadowActive){
            shadowActiveTime -= Time.deltaTime;
            if (shadowActiveTime <= 0){
                shadowPlayer.SetActive(false);
                shadowActiveTime = 2.0f;
            }
        }
        isMoving = inputVector != Vector2.zero && isGrounded;
    }
    private void FixedUpdate(){
        MovePlayer();
        //Gravity Manipulation
        GravityManipulation();
    }

    private void MovePlayer(){
        if (inputVector.magnitude > 0.1f && isGrounded){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z);
            Vector3 moveDirection = Quaternion.Euler(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z) * Vector3.forward;
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force );
        }
        else if(inputVector.magnitude > 0.1f && !isGrounded){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Enter_Performed(object sender, EventArgs e){
        switch(playerGravity){
            case (PlayerGravity.Up):
                customGravity = transform.up ;
                playerVisualMidPoint.rotation= Quaternion.Euler(180, transform.eulerAngles.y, transform.eulerAngles.z);
               break;
            case PlayerGravity.Down:
                customGravity = -transform.up;
                playerVisualMidPoint.rotation= Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z);
                break;
            case PlayerGravity.Left:
                customGravity = -transform.right;
                playerVisualMidPoint.rotation= Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -90f);
                break;
            case PlayerGravity.Right:
                customGravity = transform.right ;
                playerVisualMidPoint.rotation= Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 90f);
                break;
            default:
                customGravity = -transform.up;
                break;
        }
    }
    private void Jump_Performed(object sender, EventArgs e){
        if (isGrounded){
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Up_Performed(object sender, EventArgs e){
        playerGravity = PlayerGravity.Up;
        shadowPlayer.SetActive(true);
        shadowPlayer.transform.rotation = Quaternion.Euler(180f, transform.eulerAngles.y, transform.eulerAngles.z);
        shadowActive = true;
    }

    private void Down_Performed(object sender, EventArgs e){
        playerGravity = PlayerGravity.Down;
        shadowPlayer.SetActive(true);
        shadowActive = true;
        shadowPlayer.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z);
    }
     
    private void Left_Performed(object sender, EventArgs e){
        playerGravity = PlayerGravity.Left;
        shadowPlayer.SetActive(true);
        shadowActive = true;
        shadowPlayer.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z- 90f);
    }

    private void Right_Performed(object sender, EventArgs e){
        playerGravity = PlayerGravity.Right;
        shadowPlayer.SetActive(true);
        shadowActive = true;
        shadowPlayer.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z + 90f);
    }
    public bool IsMoving(){
        return isMoving;
    }

    public bool IsGrounded(){
        return isGrounded;
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.transform.gameObject.tag == "Box"){
            Destroy(collision.gameObject);
            OnBoxDestroyed?.Invoke();
        }
    }

    private void FreeFalling(){
        if (!isGrounded){
            freeFalling -= Time.deltaTime;
            if (freeFalling <= 0){
                PlayerDied?.Invoke();
            }
        }
        if (isGrounded){
            freeFalling = 5.0f;
        }
    }

    private void GravityManipulation(){
        rb.AddForce(customGravity * gravityStrength, ForceMode.Acceleration);
    }

    private void OnDisable(){
        gameInput.OnEnterPerformed -= Enter_Performed;

        gameInput.OnJumpPerformed -= Jump_Performed;
        gameInput.OnUpPerformed -= Up_Performed;
        gameInput.OnDownPerformed -= Down_Performed;
        gameInput.OnLeftPerformed -= Left_Performed;
        gameInput.OnRightPerformed -= Right_Performed;
    
    }
}



public enum PlayerGravity{
    Up,
    Down,
    Left,
    Right
}
