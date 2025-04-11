using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
   [SerializeField] private PlayerScript playerScript;
   private Animator animator;
   private const string Is_Moving = "IsMoving";

   private void Awake(){
       animator = GetComponent<Animator>();
   }

   private void Update(){
        animator.SetBool(Is_Moving, playerScript.IsMoving());
   }
}
