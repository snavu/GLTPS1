using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class CharacterFoodInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerInputInitialize playerInputScript;
    [SerializeField]
    private bool isPickupable;
    [SerializeField]
    private Collider other;
    [SerializeField]
    private RawImage foodbarPrefab;

    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private float hungerRefill = 20f;

    [SerializeField]
    private CharacterHunger characterHungerScript;
    [SerializeField]
    private Animator anim;

    void Start()
    {
        playerInputScript.actions.Player.Interact.performed += Interact;
        playerInputScript.actions.Player.Eat.performed += Eat;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && isPickupable && this.enabled)
        {
            Debug.Log(true);
            //destroy scene food gameobject
            Destroy(other.gameObject);

            //add food ui object and set rotation and position on canvas
            characterHungerScript.newFoodbar.Add(Instantiate(foodbarPrefab.gameObject, panel.transform));
            Debug.Log(characterHungerScript.index);
            characterHungerScript.newFoodbar[characterHungerScript.index].GetComponent<RectTransform>().localRotation = Quaternion.identity;
            characterHungerScript.newFoodbar[characterHungerScript.index].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(foodbarPrefab.rectTransform.anchoredPosition3D.x + characterHungerScript.offsetXPos
                                                                                            , foodbarPrefab.rectTransform.anchoredPosition3D.y
                                                                                            , foodbarPrefab.rectTransform.anchoredPosition3D.z);
            //offset position for adding ui objects
            characterHungerScript.offsetXPos += -40;
            characterHungerScript.index++;

            isPickupable = false;
        }
    }

    private void Eat(InputAction.CallbackContext context)
    {
        if (context.performed && characterHungerScript.index > 0 && !anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS"))
        {
            characterHungerScript.hunger += hungerRefill;

            Destroy(characterHungerScript.newFoodbar[characterHungerScript.index - 1]);
            characterHungerScript.newFoodbar.RemoveAt(characterHungerScript.index - 1);
            characterHungerScript.offsetXPos += 40;
            characterHungerScript.index--;

            //trigger eating animation
            anim.SetTrigger("eat");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            isPickupable = true;
            this.other = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            isPickupable = false;
        }
    }
}
