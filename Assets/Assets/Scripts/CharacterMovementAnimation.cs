using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Vector3 velocityXZ;
    [SerializeField]
    private Vector3 prevPosition;
    [SerializeField]
    private float runSpeed;

    // Update is called once per frame
    void Update()
    {
        velocityXZ = (transform.position - prevPosition) / Time.deltaTime;
        prevPosition = transform.position;


        //movement animation is a blendtree with blendParameter as a function of velocityXZ scaled between 0 and 1,
        //with automatic thresholds 0, 0.5, and 1 for idle, walk and run states

        //scale magnitude of vector in XZ plane
        float scaledVelocityXZ = Vector3.Magnitude(velocityXZ) / runSpeed;
        
        //set movement animation
        anim.SetFloat("velocityXZ", scaledVelocityXZ);
    }
}
