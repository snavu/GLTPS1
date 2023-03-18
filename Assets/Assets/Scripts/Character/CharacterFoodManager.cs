using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class CharacterFoodManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInputScript;
    [SerializeField]
    private bool isPickupable;
    [SerializeField]
    private Collider other;

    [SerializeField]
    private int index;
    private int offsetXPos;
    [SerializeField]
    private RawImage foodbarPrefab;
    [SerializeField]
    private List<GameObject> newFoodbar;
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private float hungerRefill = 20f;

    [SerializeField]
    private CharacterHunger characterHungerScript;

    void Start()
    {
        playerInputScript.actions.Player.Interact.performed += Interact;
        playerInputScript.actions.Player.Eat.performed += Eat;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && isPickupable)
        {
            //destroy scene food gameobject
            Destroy(other.gameObject);

            //add food ui object and set rotation and position on canvas
            newFoodbar.Add(Instantiate(foodbarPrefab.gameObject, panel.transform));
            newFoodbar[index].GetComponent<RectTransform>().localRotation = Quaternion.identity;
            newFoodbar[index].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(foodbarPrefab.rectTransform.anchoredPosition3D.x + offsetXPos
                                                                                            , foodbarPrefab.rectTransform.anchoredPosition3D.y
                                                                                            , foodbarPrefab.rectTransform.anchoredPosition3D.z);
            //offset position for adding ui objects
            offsetXPos += -40;
            index++;

            isPickupable = false;
        }

    }

    private void Eat(InputAction.CallbackContext context)
    {
        if (context.performed && index > 0)
        {
           characterHungerScript.hunger += hungerRefill;

            Destroy(newFoodbar[index - 1]);
            newFoodbar.RemoveAt(index-1);
            offsetXPos += 40;
            index--;
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
