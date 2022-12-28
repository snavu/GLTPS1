using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVehicleInteraction : MonoBehaviour
{
    private CharacterController controller;
    private CharacterMovement movementScript;
    [SerializeField]
    private Animator anim;

    private bool enterKet;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movementScript = GetComponent<CharacterMovement>();

    }

    void Update()
    {
        
    }
}
