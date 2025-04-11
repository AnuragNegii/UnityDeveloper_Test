using UnityEngine;

public class PlayerScript : MonoBehaviour{

    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float rotateSpeed = 10f;
    private bool isMoving;

    private void Update(){
        Vector2 inputVector = new Vector2(0 ,0);
        if (Input.GetKey(KeyCode.W)){
            inputVector.y = 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            transform.eulerAngles += new Vector3(0, 180, 0);
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1;
            transform.eulerAngles += new Vector3(0, -1, 0) * rotateSpeed * Time.deltaTime;
        }       
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = 1;
            transform.eulerAngles += new Vector3(0, 1, 0) * rotateSpeed * Time.deltaTime;
        }
        if (inputVector.y > 0){
             transform.position += transform.forward * moveSpeed * Time.deltaTime;
        } 
        isMoving = inputVector != Vector2.zero;
    }


    public bool IsMoving(){
        return isMoving;
    }
}
