using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class CharacterBarrelInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private Animator anim;
    public bool isPickupable;
    public bool isCarrying;
    [SerializeField] private bool inBarrelDropArea;

    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private GameObject newBarrel;
    [SerializeField] private GameObject parentConstraintSource;
    private ConstraintSource source;
    [SerializeField] private CharacterController controller;

    public Rigidbody vehicleRigidbody;

    void OnEnable()
    {
        playerInputScript.actions.Player.Interact.performed += Interact;
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Interact.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 1)
        {
            if (context.performed && isPickupable && inBarrelDropArea && !isCarrying && newBarrel == null)
            {
                //disable player jump and spring
                playerInputScript.actions.Player.Jump.Disable();
                playerInputScript.actions.Player.Sprint.Disable();

                //freeze kettengrad rigidbody to prevent movement from player collider clipping bug
                vehicleRigidbody.constraints = RigidbodyConstraints.FreezeAll;

                SetCarryParameters(true, 0.53f, true, true);

                newBarrel = Instantiate(barrelPrefab, transform.position, transform.rotation);

                SetParentConstraint();
            }

            if (context.performed && isPickupable && !inBarrelDropArea)
            {
                playerInputScript.actions.Player.Jump.Disable();
                playerInputScript.actions.Player.Sprint.Disable();

                SetCarryParameters(true, 0.5f, true, true);

                SetParentConstraint();
            }

            if (context.performed && anim.GetCurrentAnimatorStateInfo(2).IsTag("Carry"))
            {
                playerInputScript.actions.Player.Jump.Enable();
                playerInputScript.actions.Player.Sprint.Enable();

                vehicleRigidbody.constraints = RigidbodyConstraints.FreezeAll;

                SetCarryParameters(false, 0.25f, false, false);

                newBarrel.GetComponent<ParentConstraint>().constraintActive = false;

                isPickupable = false;
            }
        }
    }

    private void SetParentConstraint()
    {
        //set parent constraint of barrel
        source.sourceTransform = parentConstraintSource.transform;
        source.weight = 1;

        newBarrel.GetComponent<ParentConstraint>().SetSource(0, source);
        newBarrel.GetComponent<ParentConstraint>().constraintActive = true;
    }

    private void SetCarryParameters(bool carry, float radius, bool carryAnimParameter, bool physicsIgnore)
    {

        //disable physics collisions bettwen player and barrel, and vehicle and barrel
        Physics.IgnoreLayerCollision(7, 10, physicsIgnore);
        Physics.IgnoreLayerCollision(6, 10, physicsIgnore);
        isCarrying = carry;
        //set character controller radius size
        controller.radius = radius;
        //set carry animation
        anim.SetBool("carry", carryAnimParameter);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            isPickupable = true;
            newBarrel = other.gameObject;
        }
        if (other.gameObject.CompareTag("BarrelDropArea"))
        {
            inBarrelDropArea = true;
            isPickupable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            isPickupable = false;
        }
        if (other.gameObject.CompareTag("BarrelDropArea"))
        {
            inBarrelDropArea = false;
            isPickupable = false;
        }
    }
}
