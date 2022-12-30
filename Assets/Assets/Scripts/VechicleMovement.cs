using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VechicleMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovementScript;
    private Rigidbody rb;
    [SerializeField]
    private float force;
    [SerializeField]
    private float maxVelocity;
    private Vector2 movement;
    [SerializeField]
    private Vector3 eulerAngularVelocity;
    [SerializeField]
    private Animator anim;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //get input
        movement = playerMovementScript.actions.Vehicle.Drive.ReadValue<Vector3>();

        //set movement aniamtion
        MovementAnim();
    }
    void FixedUpdate()
    {
        //move
        if (Mathf.Abs(movement.y) > 0)
        {
            rb.AddForce(movement.y * transform.forward * force * Time.fixedDeltaTime);

            //rotate
            if (Mathf.Abs(movement.x) > 0 && Mathf.Abs(movement.y) >= 0.5f)
            {
                Quaternion deltaRotation = Quaternion.Euler(movement.x * eulerAngularVelocity * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
        }
        //clamp velocity to max velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    /*
    movement animation is a blendtree with blendParameter binded directly to velocity.z of vertical input,
    with thresholds -1, 0, and 1 for backward, idle and forward states
    */
    private void MovementAnim()
    {
        //set movement animation
        anim.SetFloat("velocityZ", movement.y);
    }

}
