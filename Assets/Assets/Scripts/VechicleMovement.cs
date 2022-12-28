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
        velocity.z = Input.GetAxisRaw("Vertical");
    }
    void FixedUpdate()
    {
        //move
        if (Mathf.Abs(velocity.z) > 0)
        {
            rb.AddForce(velocity.z * transform.forward * force * Time.fixedDeltaTime);

            //rotate
            if (Mathf.Abs(velocity.x) > 0)
            {
                Quaternion deltaRotation = Quaternion.Euler(velocity.x * eulerAngularVelocity * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
        }
        //clamp velocity to max velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

        //set movement aniamtion
        movementAnim();

    }
    private void movementAnim()
    {
        //scale magnitude of rigidbody velocity in forward direction
        float velocityXZ = rb.velocity.z / maxVelocity;
        //set movement animation
        anim.SetFloat("velocityXZ", velocityXZ);
    }

}
