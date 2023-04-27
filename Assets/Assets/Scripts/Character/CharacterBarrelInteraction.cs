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
    public bool inBarrelDropArea;

    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private GameObject newBarrel;
    [SerializeField] private GameObject parentConstraintSource;
    private ConstraintSource source;
    [SerializeField] private CharacterController controller;


    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _audioClip;

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
            if (context.performed && isPickupable && !anim.GetCurrentAnimatorStateInfo(2).IsTag("Carry") && inBarrelDropArea && !GameObject.FindWithTag("Barrel"))
            {
                //disable player jump and sprint
                playerInputScript.actions.Player.Jump.Disable();
                playerInputScript.actions.Player.Sprint.Disable();

                SetCarryParameters(true, 0.4f, 0f, true, true);

                newBarrel = Instantiate(barrelPrefab, transform.position, transform.rotation);
                newBarrel.GetComponent<VehicleBarrel>().characterBarrelInteractionScript = this;
                SetParentConstraint();

                _audioSource.PlayOneShot(_audioClip);

            }
            if (context.performed && isPickupable && !anim.GetCurrentAnimatorStateInfo(2).IsTag("Carry") && !inBarrelDropArea && !isCarrying)
            {
                //disable player jump and sprint
                playerInputScript.actions.Player.Jump.Disable();
                playerInputScript.actions.Player.Sprint.Disable();
                SetCarryParameters(true, 0.4f, 0f, true, true);
                newBarrel.GetComponent<VehicleBarrel>().characterBarrelInteractionScript = this;
                SetParentConstraint();

                _audioSource.PlayOneShot(_audioClip);
            }
            if (context.performed && anim.GetCurrentAnimatorStateInfo(2).IsTag("Carry"))
            {
                DropBarrel();
            }
        }
    }

    public void DropBarrel()
    {
        playerInputScript.actions.Player.Jump.Enable();
        playerInputScript.actions.Player.Sprint.Enable();

        SetCarryParameters(false, 0.25f, 0.23f, false, false);

        if (newBarrel != null)
        {
            newBarrel.GetComponent<ParentConstraint>().constraintActive = false;
        }

        isPickupable = false;
    }

    private void SetParentConstraint()
    {
        //set parent constraint of barrel
        source.sourceTransform = parentConstraintSource.transform;
        source.weight = 1;

        newBarrel.GetComponent<ParentConstraint>().SetSource(0, source);
        newBarrel.GetComponent<ParentConstraint>().constraintActive = true;
    }

    private void SetCarryParameters(bool carry, float radius, float stepOffset, bool carryAnimParameter, bool physicsIgnore)
    {
        //ignore collision between player and barrel
        Physics.IgnoreLayerCollision(7, 11, physicsIgnore);

        isCarrying = carry;
        //set character controller values
        controller.stepOffset = stepOffset;
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
