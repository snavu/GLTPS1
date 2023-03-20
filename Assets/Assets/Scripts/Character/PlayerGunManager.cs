using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
public class PlayerGunManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInputScript;
    [SerializeField]
    private PlayerMovement playerMovementScript;
    [SerializeField]
    private CharacterManager characterManagerScript;

    public bool isAiming;
    [SerializeField]
    private Vector2 horizontalInput;
    [SerializeField]

    private Vector2 horizontalMovement;
    private Vector3 velocityXZ;
    private Vector2 smoothMovement;
    [SerializeField]
    private float smoothInputSpeed;
    [SerializeField]
    private float currentSpeed = 2f;
    [SerializeField]
    private CharacterController controller;
    [SerializeField]

    private Animator anim;
    [SerializeField]
    private float walkSpeed = 2f;
    [SerializeField]
    private float runSpeed = 4f;

    [SerializeField]
    private ParentConstraint gunParentConstraint;
    [SerializeField]
    private GameObject handBone;
    [SerializeField]
    private GameObject backBone;
    [SerializeField]
    private ConstraintSource handSocket;
    [SerializeField]
    private ConstraintSource backSocket;


    void Start()
    {
        //playerInputScript.actions.Player.ADS.performed += AimDownSight;
    }

    void Update()
    {
        //check if player movement script is enabled and that the yuuri is the player  
        if (playerMovementScript.enabled &&
            characterManagerScript.playerInput == playerInputScript &&
            playerInputScript.actions.Player.ADS.ReadValue<float>() > 0f &&
            playerMovementScript.isGrounded)
        {
            isAiming = true;
            playerMovementScript.enabled = false;
        }

        if (playerInputScript.actions.Player.ADS.ReadValue<float>() > 0f && !playerMovementScript.enabled)
        {
            //get horizontal input
            horizontalInput = playerInputScript.actions.Player.Move.ReadValue<Vector2>();
            horizontalMovement = Vector2.SmoothDamp(horizontalMovement, horizontalInput, ref smoothMovement, smoothInputSpeed);

            velocityXZ = Vector3.ClampMagnitude(new Vector3(horizontalMovement.x, 0, horizontalMovement.y), 1.0f) * currentSpeed;
            velocityXZ = transform.TransformDirection(velocityXZ);

            //move character based on input
            controller.Move((velocityXZ) * Time.deltaTime);

            //pass velocityXZ to drive movement animation
            CharacterMovementAnimation.Movement(anim, velocityXZ, runSpeed);

            //set player ADS animation
            anim.SetBool("ADS", true);
        }
        else
        {
            anim.SetBool("ADS", false);
            playerMovementScript.enabled = true;
        }
    }

    public void AttachToGunHandSocket()
    {
        if (!gunParentConstraint.GetSource(0).Equals(handSocket))
        {
            //set parent constraint of gun to hand
            handSocket.sourceTransform = handBone.transform;
            handSocket.weight = 1;

            gunParentConstraint.SetSource(0, handSocket);
            gunParentConstraint.constraintActive = true;
        }

    }
    public void AttachToGunBackSocket()
    {
        if (!gunParentConstraint.GetSource(0).Equals(backSocket))
        {
            //set parent constraint of gun to back
            backSocket.sourceTransform = backBone.transform;
            backSocket.weight = 1;

            gunParentConstraint.SetSource(0, backSocket);
            gunParentConstraint.constraintActive = true;
        }

    }

    // private void AimDownSight(InputAction.CallbackContext context)
    // {
    //     if (characterManagerScript.playerInput == playerInputScript)
    //     {
    //         isAiming = context.ReadValue<float>() > 0f;

    //     }
    // }

}
