using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VechicleMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private float force;
    [SerializeField]
    private float maxVelocity;
    private Vector3 velocity;
    [SerializeField]
    private Vector3 eulerAngularVelocity;
    [SerializeField]
    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //get input
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.z = Input.GetAxis("Vertical");

        //set movement aniamtion
        movementAnim();
    }
    void FixedUpdate()
    {
        //move
        if (Mathf.Abs(velocity.z) > 0)
        {
            rb.AddForce(velocity.z * transform.forward * force * Time.fixedDeltaTime);

            //rotate
            if (Mathf.Abs(velocity.x) > 0 && Mathf.Abs(velocity.z) >= 0.5f)
            {
                Quaternion deltaRotation = Quaternion.Euler(velocity.x * eulerAngularVelocity * Time.fixedDeltaTime);
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
    private void movementAnim()
    {
        //set movement animation
        anim.SetFloat("velocityZ", velocity.z);
    }

}
