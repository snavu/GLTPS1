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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        //get input
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.z = Input.GetAxisRaw("Vertical");
    }
    void FixedUpdate()
    {
        //move
        if(Mathf.Abs(velocity.z) > 0){
            rb.AddForce(velocity.z * transform.forward * force * Time.fixedDeltaTime);
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

        //rotate
        if(Mathf.Abs(velocity.x) > 0){
            Quaternion deltaRotation = Quaternion.Euler(velocity.x * eulerAngularVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

    }
}
