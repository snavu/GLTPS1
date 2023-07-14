using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform child;
    private Vector2 horizontalInput;

    private Vector2 horizontalMovement;
    private Vector3 wishDir;
    private Vector3 velocityY;
    private Vector3 vel;
    private float verticalMovement;
    public bool isGrounded;
    public float currentSpeed;
    public float walkSpeed = 4.0f;
    public float runSpeed = 7.0f;
    [SerializeField] float slowMovementRate = 0.5f;
    public float ADSSpeed = 2.0f;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float rotationSpeed = 600.0f;

    [SerializeField] private float jumpHeight = 1.0f;

    [SerializeField] private Transform camera;
    private Vector2 smoothMovement;
    [SerializeField] private float smoothInputSpeed = 0.2f;
    private LayerMask layerMask;
    private Vector3 sphereCastPosition;

    [SerializeField] private CharacterItemInteraction characterItemInteractionScript;
    [SerializeField] private CharacterStatusScreenEffect characterStatusScreenEffectScript;

    public bool isCarrying;

    public bool ADS;
    void OnEnable()
    {
        playerInputScript.actions.Player.Enable();
        playerInputScript.actions.Player.Jump.performed += Jump;

        //set layermask interact with vehicle and default
        layerMask = LayerMask.GetMask("Default", "Vehicle", "Elevator");
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Disable();
        playerInputScript.actions.Player.Jump.performed -= Jump;
    }

    void Update()
    {
        if (controller.enabled)
        {
            //get horizontal input
            horizontalInput = playerInputScript.actions.Player.Move.ReadValue<Vector2>();
            horizontalMovement = Vector2.SmoothDamp(horizontalMovement, horizontalInput, ref smoothMovement, smoothInputSpeed);

            //align spherecast at bottomRadius of collide, scale position inversely proportional to controller skin width, and minus small constant to extrude vertically down
            sphereCastPosition = new Vector3(0, controller.radius - controller.skinWidth - 0.01f, 0);

            //check ground collision
            isGrounded = Physics.CheckSphere(transform.position + sphereCastPosition, controller.radius, layerMask, QueryTriggerInteraction.Ignore);

            //apply gravity
            if (isGrounded && verticalMovement < 0)
            {
                verticalMovement = 0f;
            }
            else
            {
                verticalMovement += gravity * Time.deltaTime;
            }

            if (!ADS)
                //lerp speed values for blending walk/run animation states
                if (playerInputScript.actions.Player.Sprint.ReadValue<float>() > 0)
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, 10.0f * Time.deltaTime);
                }
                else
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, 10.0f * Time.deltaTime);
                }
            //lerp speed value for ADS speed
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, ADSSpeed, 10.0f * Time.deltaTime);
            }

            //slow movement when hungry/thirsty
            if (characterItemInteractionScript.hungerLevel < characterStatusScreenEffectScript.hungerThreshold || 
                characterItemInteractionScript.thirstLevel < characterStatusScreenEffectScript.thirstThreshold ||
                characterItemInteractionScript.temperatureLevel < characterStatusScreenEffectScript.temperatureThreshold)
            {
                wishDir = Vector3.ClampMagnitude(new Vector3(horizontalMovement.x, 0, horizontalMovement.y), 1.0f) * currentSpeed * slowMovementRate;
            }
            else
            {
                wishDir = Vector3.ClampMagnitude(new Vector3(horizontalMovement.x, 0, horizontalMovement.y), 1.0f) * currentSpeed;
            }
            wishDir = transform.TransformDirection(wishDir);
            velocityY = new Vector3(0, verticalMovement, 0);

            float addSpeed = (runSpeed - currentSpeed) * Time.deltaTime;
            vel += vel +  addSpeed * wishDir;

            //move character based on input
            controller.Move((wishDir + velocityY) * Time.deltaTime);

            //pass wishDir to drive movement animation
            CharacterMovementAnimation.Movement(anim, wishDir, runSpeed);
                
            if (!ADS)
            {
                //rotate child in direction of movement      
                if (Vector3.Magnitude(wishDir) > 0.5f)
                {
                    Quaternion rotationDir = Quaternion.LookRotation(wishDir);
                    child.rotation = Quaternion.RotateTowards(child.rotation, rotationDir, rotationSpeed * Time.deltaTime);
                }

                //rotate parent to camera
                if (Vector3.Magnitude(wishDir) > 0.5f)
                {
                    Vector3 cam = new Vector3(camera.TransformDirection(Vector3.forward).x, 0, camera.TransformDirection(Vector3.forward).z);
                    Quaternion rotationDir = Quaternion.LookRotation(cam);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDir, rotationSpeed * Time.deltaTime);
                }
            }
        }

    }
    private void Jump(InputAction.CallbackContext context)
    {
        //jump
        if (context.performed && isGrounded && !ADS && Time.timeScale == 1)
        {
            verticalMovement = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, controller.radius - controller.skinWidth - 0.01f, 0), controller.radius);
    }
}
