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
    private bool isPickupable;
    private bool isCarrying;
    [SerializeField]
    private bool inBarrelDropArea;

    [SerializeField]
    private SkinnedMeshRenderer ketBarrelMesh;
    [SerializeField]
    private GameObject barrelPrefab;
    [SerializeField]
    private GameObject newBarrel;
    [SerializeField]
    private GameObject parentConstraintSource;
    private ConstraintSource source;
    [SerializeField]
    private CharacterController controller;



    void Start()
    {
        playerMovementScript.actions.Player.Interact.performed += Interact;
        playerMovementScript.actions.Player.Drop.performed += Drop;

    }

    void Update()
    {

    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && isPickupable && inBarrelDropArea && !isCarrying)
        {
            //disable player jump and spring
            playerMovementScript.actions.Player.Jump.Disable();
            playerMovementScript.actions.Player.Sprint.Disable();

            SetParameters(true, 0.53f, true, true);

            //hide the barrel mesh of the kettengrad
            ketBarrelMesh.enabled = false;

            //set parent constraint of barrel
            source.sourceTransform = parentConstraintSource.transform;
            source.weight = 1;
            //spawn barrel
            newBarrel = Instantiate(barrelPrefab, transform.position, transform.rotation);
            newBarrel.GetComponent<ParentConstraint>().SetSource(0, source);
            newBarrel.GetComponent<ParentConstraint>().constraintActive = true;
        }

        if (context.performed && isPickupable && !inBarrelDropArea)
        {
            SetParameters(true, 0.54f, true, true);

            newBarrel.GetComponent<ParentConstraint>().constraintActive = true;
        }
    }

    private void Drop(InputAction.CallbackContext context)
    {
        playerMovementScript.actions.Player.Jump.Enable();
        playerMovementScript.actions.Player.Sprint.Enable();

        if (context.performed && anim.GetCurrentAnimatorStateInfo(2).IsTag("carry"))
        {
            SetParameters(false, 0.25f, false, false);

            newBarrel.GetComponent<ParentConstraint>().constraintActive = false;

            if (newBarrel.GetComponent<VehicleBarrel>().isInBarrelDropArea)
            {
                Destroy(newBarrel);
                ketBarrelMesh.enabled = true;
            }

            isPickupable = false;
        }
    }

    private void SetParameters(bool carry, float radius, bool carryAnimParameter, bool physicsIgnore)
    {
        isCarrying = carry;
        //set character controller radius size
        controller.radius = radius;
        //set carry animation
        anim.SetBool("carry", carryAnimParameter);
        //disable physics collisions bettwen player and barrel
        Physics.IgnoreLayerCollision(7, 10, physicsIgnore);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            isPickupable = true;
        }
        if (other.gameObject.CompareTag("BarrelDropArea"))
        {
            inBarrelDropArea = true;
            isPickupable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BarrelDropArea"))
        {
            inBarrelDropArea = false;
        }
    }
}
