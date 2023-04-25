using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class CharacterItemInteraction : MonoBehaviour
{
    public PlayerInputInitialize playerInputScript;
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
    public Animator anim;
    [SerializeField] private PlayerGunController playerGunControllerScript;
    [SerializeField] private int maxRandomAmmoCount = 5;
    [SerializeField] private TextMeshProUGUI ammoCountText;

    [SerializeField] private float waterRefillRate = 1f;
    [SerializeField] private int maxRandomFoodCount = 5;

    public AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClip;

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
            if (isFood && characterItemDataScript.index < 18 && other != null)
            {
                //destroy scene food gameobject
                Destroy(other.gameObject);

                //increase food count by random amount
                for (int i = 0; i < Random.Range(1, maxRandomFoodCount + 1); i++)
                {
                    //add food ui object and set rotation and position on canvas
                    characterItemDataScript.newFoodbar.Add(Instantiate(foodbarPrefab.gameObject, panel.transform));
                    characterItemDataScript.newFoodbar[characterItemDataScript.index].GetComponent<RectTransform>().localRotation = Quaternion.identity;
                    characterItemDataScript.newFoodbar[characterItemDataScript.index].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(foodbarPrefab.rectTransform.anchoredPosition3D.x + characterItemDataScript.offsetXPos
                                                                                                    , foodbarPrefab.rectTransform.anchoredPosition3D.y
                                                                                                    , foodbarPrefab.rectTransform.anchoredPosition3D.z);
                    //offset position for adding ui objects
                    characterItemDataScript.offsetXPos += -30;
                    characterItemDataScript.index++;
                }

                isFood = false;

                _audioSource.PlayOneShot(_audioClip[0]);
            }
            if (isAmmo && other != null)
            {
                //destroy scene ammo gameobject
                Destroy(other.gameObject);

                //increase ammo count by random amount
                playerGunControllerScript.ammoCount += Random.Range(1, maxRandomAmmoCount + 1);

                //change ammo ui
                ammoCountText.text = playerGunControllerScript.ammoCount.ToString();

                _audioSource.PlayOneShot(_audioClip[0]);
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
            characterItemDataScript.offsetXPos += 30;
            characterItemDataScript.index--;

            //trigger eating animation
            anim.SetTrigger("eat");

            _audioSource.PlayOneShot(_audioClip[Random.Range(2, _audioClip.Length)]);
        }
    }

    private void Drink(InputAction.CallbackContext context)
    {
        if (context.performed && characterItemDataScript.waterLevel > 0 && !anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS") && Time.timeScale == 1)
        {
            thirstLevel += drinkValue;
            characterItemDataScript.waterLevel -= drinkValue;

            anim.SetTrigger("drink");

            _audioSource.PlayOneShot(_audioClip[1]);
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
        if (other.gameObject.CompareTag("Water"))
        {
            characterItemDataScript.waterLevel += waterRefillRate;
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
            characterItemDataScript.waterLevel += waterRefillRate;
        }
    }
}
