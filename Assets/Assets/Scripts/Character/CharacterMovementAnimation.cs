using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovementAnimation : MonoBehaviour
{
    //movement animation is a blendtree with blendParameter as a function of velocityXZ scaled between 0 and 1,
    //with automatic thresholds 0, 0.5, and 1 for idle, walk and run states
    public static void Movement(Animator anim, Vector3 velocityXZ, float maxSpeed)
    {
        //scale magnitude of vector in XZ plane
        float scaledVelocityXZ = Vector3.Magnitude(velocityXZ) / maxSpeed;
        //set movement animation
        anim.SetFloat("velocityXZ", scaledVelocityXZ);
    }
}
