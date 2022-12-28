using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField]
    private Vector3 velocity;
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
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //get vertical movement
        isGrounded = Physics.Raycast(transform.position, new Vector3(0, -1, 0), controller.skinWidth + 0.001f);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        //get horizontal movement
        //Input.GetAxis blends idle/walk and idle/run animation states 
        velocity.x = Input.GetAxis("Horizontal");
        velocity.z = Input.GetAxis("Vertical");

        //lerp speed values for blending walk/run animation states
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Mathf.Lerp(speed, runSpeed, 10.0f * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, walkSpeed, 10.0f * Time.deltaTime);
        }

        //move player
        Vector3 moveXZ = Vector3.ClampMagnitude(new Vector3(velocity.x, 0, velocity.z), 1.0f) * speed;
        Vector3 moveY = new Vector3(0, velocity.y, 0);
        controller.Move((moveXZ + moveY) * Time.deltaTime);

        //rotate player
        if (Vector3.Magnitude(moveXZ) != 0)
        {
            Quaternion rotationDir = Quaternion.LookRotation(moveXZ, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDir, rotationSpeed * Time.deltaTime);
        }

        //set movement animation
        movementAnim(moveXZ);

        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    /*
    movement animation is a blendtree with blendParameter as a function of moveXZ scaled between 0 and 1,
    with automatic thresholds 0, 0.5, and 1 for idle, walk and run states
    */
    private void movementAnim(Vector3 moveXZ)
    {
        //scale magnitude of vector in XZ plane
        float velocityXZ = Vector3.Magnitude(moveXZ) / runSpeed;

        //set movement animation
        anim.SetFloat("velocityXZ", velocityXZ);
    }
}
