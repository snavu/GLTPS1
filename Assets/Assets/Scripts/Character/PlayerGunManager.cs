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
    private Transform gun;
    [SerializeField]
    private Transform handSocket;
    [SerializeField]
    private Transform backSocket;

    [SerializeField]
    private float timeUntilEnequip = 0.5f;
    private bool unequip = true;

    [SerializeField]
    private Transform camera;
    [SerializeField]
    private float rotationSpeed = 600.0f;
    [SerializeField]
    private Transform spineBone;

    [SerializeField]
    private Vector3 CMFollowTargetOffsetPos;
    [SerializeField]
    private Vector3 CMLookAtTargetOffsetPos;

    [SerializeField]
    private Transform CMFollowTarget;
    [SerializeField]
    private Transform CMLookAtTarget;
    private Vector3 CMFollowTargetInitialLocalPos;

    private Vector3 CMLookAtTargetInitialLocalPos;
    [SerializeField]
    private Transform child;

    [SerializeField]
    private GameObject crosshair;
    void Start()
    {

        //playerInputScript.actions.Player.ADS.performed += AimDownSight;
    }

    void Update()
    {
        //check if yuuri is the player  
        if (characterManagerScript.playerInput == playerInputScript &&
            playerInputScript.actions.Player.ADS.ReadValue<float>() > 0f &&
            playerMovementScript.enabled &&
            playerMovementScript.isGrounded)
        {
            playerMovementScript.enabled = false;
        }

        if (characterManagerScript.playerInput == playerInputScript &&
            playerInputScript.actions.Player.ADS.ReadValue<float>() > 0f &&
            !playerMovementScript.enabled
            )
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

            //rotate child in direction of camera      
            child.rotation = Quaternion.Euler(0f, camera.rotation.eulerAngles.y, 0f);

            //rotate player to camera
            Vector3 camXZ = new Vector3(camera.TransformDirection(Vector3.forward).x, 0, camera.TransformDirection(Vector3.forward).z);
            Quaternion rotationDirXZ = Quaternion.LookRotation(camXZ);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirXZ, rotationSpeed * Time.deltaTime);

            CMFollowTargetInitialLocalPos = new Vector3(0, CMFollowTarget.transform.position.y, 0);
            CMLookAtTargetInitialLocalPos = new Vector3(0, CMLookAtTarget.transform.position.y, 0);
            //set player ADS animation
            if (unequip)
            {
                //set camera position
                CMFollowTarget.localPosition = CMFollowTargetOffsetPos;
                CMLookAtTarget.localPosition = CMLookAtTargetOffsetPos;

                //set gun to hand
                StopAllCoroutines();
                AttachToGunHandSocket();
                anim.SetBool("ADS", true);
                crosshair.SetActive(true);
                unequip = false;

            }

        }
        else
        {
            if (!unequip)
            {
                //reset camera position
                CMFollowTarget.localPosition = CMFollowTargetInitialLocalPos;
                CMLookAtTarget.localPosition = CMLookAtTargetInitialLocalPos;

                //set gun to back
                StartCoroutine(AttachToGunBackSocket());
                anim.SetBool("ADS", false);
                playerMovementScript.enabled = true;
                crosshair.SetActive(false);
                unequip = true;

            }
        }
    }

    void LateUpdate()
    {
        if (characterManagerScript.playerInput == playerInputScript &&
            playerInputScript.actions.Player.ADS.ReadValue<float>() > 0f &&
            !playerMovementScript.enabled
            )
        {
            spineBone.rotation = camera.rotation;
        }
    }

    public void AttachToGunHandSocket()
    {
        //set parent of gun to hand
        gun.parent = handSocket;
        gun.localPosition = Vector3.zero;
        gun.localRotation = Quaternion.identity;
    }
    public IEnumerator AttachToGunBackSocket()
    {
        yield return new WaitForSeconds(timeUntilEnequip);

        //set parent of gun to back
        gun.parent = backSocket;
        gun.localPosition = Vector3.zero;
        gun.localRotation = Quaternion.identity;


    }

    // private void AimDownSight(InputAction.CallbackContext context)
    // {
    //     if (characterManagerScript.playerInput == playerInputScript)
    //     {
    //         isAiming = context.ReadValue<float>() > 0f;

    //     }
    // }

}
