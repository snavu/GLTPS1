using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterVehicleInteraction : MonoBehaviour
{
    private CharacterController controller;
    private CharacterMovement movementScript;
    [SerializeField]
    private Animator vehicleAnim;
    [SerializeField]
    private Animator playerAnim;

    private float steer;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        movementScript = GetComponent<CharacterMovement>();

    }

    void Update()
    {
        
    }

    void OnCollisionStay(Collision collisionInfo){
        
    }
}
