using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class CharacterFuelInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInputScript;
    [SerializeField]
    private CharacterBarrelInteraction characterBarrelInteractionScript;
    [SerializeField]
    private Animator fuelAnim;
    [SerializeField]
    private bool inFuelingStationInteractArea;
    private bool isFueling;
    void Start()
    {
        playerInputScript.actions.Player.HoldInteract.performed += HoldInteract;
    }

    private void HoldInteract(InputAction.CallbackContext context)
    {
        if (context.performed && inFuelingStationInteractArea && !characterBarrelInteractionScript.isCarrying)
        {
            if (isFueling)
            {
                fuelAnim.SetBool("Fueling", false);
                isFueling = false;
            }
            else
            {
                fuelAnim.SetBool("Fueling", true);
                isFueling = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FuelingStation"))
        {
            fuelAnim = other.gameObject.GetComponentInChildren<Animator>();
            inFuelingStationInteractArea = true;
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