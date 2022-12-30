using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterMovementAnimation
{
    //movement animation is a blendtree with blendParameter as a function of velocityXZ scaled between 0 and 1,
    //with automatic thresholds 0, 0.5, and 1 for idle, walk and run states
    public static void Movement(Animator anim, Vector3 velocityXZ, float runSpeed)
    {
        //scale magnitude of vector in XZ plane
        float scaledVelocityXZ = Vector3.Magnitude(velocityXZ) / runSpeed;

        //set movement animation
        anim.SetFloat("velocityXZ", scaledVelocityXZ);
    }
}
