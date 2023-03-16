using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    private Animator anim;
    [SerializeField]
    private Transform child;
    private Vector2 horizontalInput;

    private Vector2 horizontalMovement;
    private Vector3 velocityXZ;
    private Vector3 velocityY;
    private float verticalMovement;

    [SerializeField]
    private bool isGrounded;
    public float currentSpeed;
    public float walkSpeed = 2.0f;
    public float runSpeed = 4.0f;
    [SerializeField]
    private float sprint = 0f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float rotationSpeed = 600.0f;
    [SerializeField]
    Quaternion rotationDir;
    public InputActions actions;
    [SerializeField]

    private float jumpHeight = 1.0f;

    [SerializeField]
    private Transform camera;
    private Vector2 smoothMovement;
    [SerializeField]
    private float smoothInputSpeed = 0.2f;

    private LayerMask layerMask = 7;

    private Vector3 sphereCastPosition;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        actions = new InputActions();
        Debug.Log(actions);
        actions.Player.Enable();
        actions.Player.Jump.performed += Jump;

        //align spherecast at bottom of collide, scale position inversely proportional to controller skin width, and minus small constant to extrude vertically down
        sphereCastPosition = new Vector3(0, controller.radius - controller.skinWidth - 0.01f, 0);
    }

    void Update()
    {
        //get horizontal input
        horizontalInput = actions.Player.Move.ReadValue<Vector2>();
        horizontalMovement = Vector2.SmoothDamp(horizontalMovement, horizontalInput, ref smoothMovement, smoothInputSpeed);

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

        //lerp speed values for blending walk/run animation states
        if (actions.Player.Sprint.ReadValue<float>() > 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, 10.0f * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, 10.0f * Time.deltaTime);
        }

        velocityXZ = Vector3.ClampMagnitude(new Vector3(horizontalMovement.x, 0, horizontalMovement.y), 1.0f) * currentSpeed;
        velocityXZ = transform.TransformDirection(velocityXZ);
        velocityY = new Vector3(0, verticalMovement, 0);

        //move character based on input
        controller.Move((velocityXZ + velocityY) * Time.deltaTime);

        //pass velocityXZ to drive movement animation
        CharacterMovementAnimation.Movement(anim, velocityXZ, runSpeed);
        //rotate child in direction of movement      
        if (Vector3.Magnitude(velocityXZ) > 0.5f)
        {
            rotationDir = Quaternion.LookRotation(velocityXZ);
            child.rotation = Quaternion.RotateTowards(child.rotation, rotationDir, rotationSpeed * Time.deltaTime);
        }

        //rotate parent to camera
        if (Vector3.Magnitude(velocityXZ) > 0.5f)
        {
            Vector3 cam = new Vector3(camera.TransformDirection(Vector3.forward).x, 0, camera.TransformDirection(Vector3.forward).z);
            Quaternion rotationDir = Quaternion.LookRotation(cam);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDir, rotationSpeed * Time.deltaTime);
        }

    }
    private void Jump(InputAction.CallbackContext context)
    {
        //jump
        if (context.performed && isGrounded)
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
