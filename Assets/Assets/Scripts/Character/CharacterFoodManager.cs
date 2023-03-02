using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class CharacterFoodManager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovementScript;
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
    private float hunger = 100f;
    [SerializeField]
    private float hungerRate = 1f;
    [SerializeField]
    private float hungerRefill = 20f;
    [SerializeField]
    private float maxHungerValue = 0;
    [SerializeField]
    private float minHungerValue = 100f;


    void Start()
    {
        playerMovementScript.actions.Player.Interact.performed += Interact;
        playerMovementScript.actions.Player.Eat.performed += Eat;
    }
    void Update()
    {
        //clamp hunger so player does not exceed max hunger value when refilling hunger
        hunger = Mathf.Clamp(hunger, minHungerValue, maxHungerValue);
        hunger -= hungerRate * Time.deltaTime;
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
        }

    }

    private void Eat(InputAction.CallbackContext context)
    {
        if (context.performed && index > 0)
        {
            hunger += hungerRefill;

            Destroy(newFoodbar[index - 1]);
            newFoodbar.RemoveAt(index-1);
            offsetXPos += 40;
            index--;
        }
    }

    void OnTriggerEnter(Collider other)
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
