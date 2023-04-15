using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class CharacterItemInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerInputInitialize playerInputScript;
    private bool isFood;
    private bool isAmmo;
    private bool isWater;

    [SerializeField] private Collider other;
    [SerializeField] private RawImage foodbarPrefab;

    [SerializeField] private GameObject panel;

    [SerializeField] private float eatValue = 20f;
    [SerializeField] private float drinkValue = 20f;

    public float hungerLevel = 100f;
    public float thirstLevel = 100f;
    [SerializeField] private CharacterItemData characterItemDataScript;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerGunController playerGunControllerScript;
    [SerializeField] private int maxAmmoValue = 5;

    [SerializeField] private float waterIncreaseRate = 1f;




    void OnEnable()
    {
        playerInputScript.actions.Player.Interact.performed += Interact;
        playerInputScript.actions.Player.Eat.performed += Eat;
        playerInputScript.actions.Player.Drink.performed += Drink;
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Interact.performed -= Interact;
        playerInputScript.actions.Player.Eat.performed -= Eat;
        playerInputScript.actions.Player.Drink.performed -= Drink;
    }



    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale == 1)
        {
            if (isFood)
            {
                //destroy scene food gameobject
                Destroy(other.gameObject);

                //add food ui object and set rotation and position on canvas
                characterItemDataScript.newFoodbar.Add(Instantiate(foodbarPrefab.gameObject, panel.transform));
                characterItemDataScript.newFoodbar[characterItemDataScript.index].GetComponent<RectTransform>().localRotation = Quaternion.identity;
                characterItemDataScript.newFoodbar[characterItemDataScript.index].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(foodbarPrefab.rectTransform.anchoredPosition3D.x + characterItemDataScript.offsetXPos
                                                                                                , foodbarPrefab.rectTransform.anchoredPosition3D.y
                                                                                                , foodbarPrefab.rectTransform.anchoredPosition3D.z);
                //offset position for adding ui objects
                characterItemDataScript.offsetXPos += -30;
                characterItemDataScript.index++;

                isFood = false;
            }
            if (isAmmo)
            {
                //destroy scene ammo gameobject
                Destroy(other.gameObject);

                //increase ammo count by random ammount
                playerGunControllerScript.ammoCount += Random.Range(1, maxAmmoValue + 1);
            }
        }
    }

    private void Eat(InputAction.CallbackContext context)
    {
        if (context.performed && characterItemDataScript.index > 0 && !anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS") && Time.timeScale == 1)
        {
            hungerLevel += eatValue;

            Destroy(characterItemDataScript.newFoodbar[characterItemDataScript.index - 1]);
            characterItemDataScript.newFoodbar.RemoveAt(characterItemDataScript.index - 1);
            characterItemDataScript.offsetXPos += 40;
            characterItemDataScript.index--;

            //trigger eating animation
            anim.SetTrigger("eat");
        }
    }

    private void Drink(InputAction.CallbackContext context)
    {
        if (context.performed && characterItemDataScript.waterLevel > 0 && !anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS") && Time.timeScale == 1)
        {
            thirstLevel += drinkValue;
            characterItemDataScript.waterLevel -= drinkValue;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            isFood = true;
            this.other = other;
        }
        if (other.gameObject.CompareTag("Ammo"))
        {
            isAmmo = true;
            this.other = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            isFood = false;
        }
        if (other.gameObject.CompareTag("Ammo"))
        {
            isAmmo = false;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            characterItemDataScript.waterLevel += waterIncreaseRate;
        }
    }
}
