using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class CharacterFuelInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerInputInitialize playerInputScript;
    [SerializeField]
    private CharacterBarrelInteraction characterBarrelInteractionScript;
    [SerializeField]
    private Animator fuelAnim;
    [SerializeField]
    private bool inFuelingStationInteractArea;
    private bool isFueling;

    private Collider other;
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
        if (context.performed && inFuelingStationInteractArea && !characterBarrelInteractionScript.isCarrying && Time.timeScale == 1)
        {
            if (isFueling)
            {
                other.gameObject.GetComponentInChildren<ParticleSystem>().Play();
                isFueling = false;
            }
            else
            {
                other.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                isFueling = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FuelingStation"))
        {
            inFuelingStationInteractArea = true;
            this.other = other;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FuelingStation"))
        {
            inFuelingStationInteractArea = false;
        }
    }
}
