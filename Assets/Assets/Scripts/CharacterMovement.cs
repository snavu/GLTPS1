using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public InputActions actions;
    private CharacterController controller;
    private Vector2 horizontalMovement;
    private float verticalMovement;

    [SerializeField]
    private bool isGrounded;
    private float speed = 0f;
    [SerializeField]
    private float walkSpeed = 1.0f;
    [SerializeField]
    private float runSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float rotationSpeed = 1.0f;

    [SerializeField]

    private Animator anim;

    [SerializeField]
    private Transform camera;
    [SerializeField]
    private Transform child;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        actions = new InputActions();
        actions.Player.Move.Enable();
        actions.Player.Sprint.Enable();
        actions.Player.Jump.Enable();
        actions.Player.Jump.performed += Jump;
    }

    void Update()
    {
        //get vertical movement
        isGrounded = Physics.Raycast(transform.position, new Vector3(0, -1, 0), controller.skinWidth + 0.001f);

        if (isGrounded && verticalMovement < 0)
        {
            verticalMovement = 0f;
        }
        else
        {
            verticalMovement += gravity * Time.deltaTime;
        }

        //get horizontal movement
        //analog mode for smoothing idle/walk and idle/run animation states 
        horizontalMovement = actions.Player.Move.ReadValue<Vector2>();

        //sprint
        //lerp speed values for blending walk/run animation states
        if (actions.Player.Sprint.ReadValue<float>() > 0)
        {
            speed = Mathf.Lerp(speed, runSpeed, 10.0f * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, walkSpeed, 10.0f * Time.deltaTime);
        }


        //move parent
        Vector3 velocityXZ = Vector3.ClampMagnitude(new Vector3(horizontalMovement.x, 0, horizontalMovement.y), 1.0f) * speed;
        velocityXZ = transform.TransformDirection(velocityXZ);
        Vector3 velocityY = new Vector3(0, verticalMovement, 0);

        controller.Move((velocityXZ + velocityY) * Time.deltaTime);

        //rotate child in direction of movement      
        if (Vector3.Magnitude(velocityXZ) != 0)
        {
            Quaternion rotationDir = Quaternion.LookRotation(velocityXZ);
            child.rotation = Quaternion.RotateTowards(child.rotation, rotationDir, rotationSpeed * Time.deltaTime);
        }

        //rotate parent to camera
        if (Vector3.Magnitude(velocityXZ) != 0)
        {
            Vector3 cam = new Vector3(camera.TransformDirection(Vector3.forward).x, 0, camera.TransformDirection(Vector3.forward).z);
            Quaternion rotationDir = Quaternion.LookRotation(cam);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDir, rotationSpeed * Time.deltaTime);
        }

        //set movement animation
        movementAnim(velocityXZ);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        //jump
        if (context.performed && isGrounded)
        {
            verticalMovement = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {

    }

    /*
    movement animation is a blendtree with blendParameter as a function of velocityXZ scaled between 0 and 1,
    with automatic thresholds 0, 0.5, and 1 for idle, walk and run states
    */
    private void movementAnim(Vector3 velocityXZ)
    {
        //scale magnitude of vector in XZ plane
        float scaledVelocityXZ = Vector3.Magnitude(velocityXZ) / runSpeed;

        //set movement animation
        anim.SetFloat("velocityXZ", scaledVelocityXZ);
    }
}
