using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class CharacterItemInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovementScript;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private bool isPickupable;

    [SerializeField]
    private ParentConstraint barrel;

    void Start()
    {
        playerMovementScript.actions.Player.Interact.performed += Interact;
        playerMovementScript.actions.Player.Drop.performed += Drop;

    }

    void Update()
    {
        //Debug.Log(anim.GetBool("carry"));
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && isPickupable)
        {
            Physics.IgnoreLayerCollision(7, 10, true);
            barrel.constraintActive = true;
            anim.SetBool("carry", true);
        }
    }
    private void Drop(InputAction.CallbackContext context)
    {

        if (context.performed && anim.GetCurrentAnimatorStateInfo(2).IsTag("carry"))
        {
            Physics.IgnoreLayerCollision(7, 10, false);
            barrel.constraintActive = false;
            anim.SetBool("carry", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            isPickupable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            isPickupable = false;
        }
    }
}
