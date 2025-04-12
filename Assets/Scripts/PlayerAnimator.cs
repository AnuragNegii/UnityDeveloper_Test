using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
   [SerializeField] private PlayerScript playerScript;
   private Animator animator;
   private const string Is_Moving = "IsMoving";
   private const string Is_Grounded= "IsGrounded";

   private void Awake(){
       animator = GetComponent<Animator>();
   }

   private void Update(){
        animator.SetBool(Is_Moving, playerScript.IsMoving());
        animator.SetBool(Is_Grounded, playerScript.IsGrounded());
   }
}
