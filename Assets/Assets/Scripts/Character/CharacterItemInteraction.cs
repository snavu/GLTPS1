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
    private bool isCampfire;
    public bool isInCampfireArea;


    [SerializeField] private Collider other;
    [SerializeField] private RawImage foodbarPrefab;
    [SerializeField] private GameObject panel;

    public float hungerLevel = 100f;
    [SerializeField] private float eatValue = 20f;
    [SerializeField] private int maxRandomFoodCount = 5;

    public float thirstLevel = 100f;
    [SerializeField] private float drinkValue = 20f;
    [SerializeField] private float waterRefillRate = 1f;

    public float temperatureLevel = 100f;
    [SerializeField] private float temperatureRefillValue = 1f;
    [SerializeField] private float fuelValue = 20f;
    public bool isUnderCeiling;
    [SerializeField] float isUnderCeilingHeight = 20f;
    [SerializeField] private GameObject campfirePrefab;
    [SerializeField] private Vector3 campfireOffsetPosition;
    [SerializeField] private VehicleFuelManager vehicleFuelManagerScript;


    [SerializeField] private CharacterStatus characterStatusScript;
    public Animator anim;
    [SerializeField] private PlayerGunController playerGunControllerScript;
    [SerializeField] private int maxRandomAmmoCount = 5;
    [SerializeField] private TextMeshProUGUI ammoCountText;


    public AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClip;

    void OnEnable()
    {
        playerInputScript.actions.Player.Interact.performed += Interact;
        playerInputScript.actions.Player.Eat.performed += Eat;
        playerInputScript.actions.Player.Drink.performed += Drink;
        playerInputScript.actions.Player.PlaceCampfire.performed += PlaceCampfire;
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Interact.performed -= Interact;
        playerInputScript.actions.Player.Eat.performed -= Eat;
        playerInputScript.actions.Player.Drink.performed -= Drink;
        playerInputScript.actions.Player.PlaceCampfire.performed -= PlaceCampfire;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale == 1)
        {
            if (isFood && characterStatusScript.index < 18 && other.gameObject.CompareTag("Food"))
            {
                //destroy scene food gameobject
                Destroy(other.gameObject);

                //increase food count by random amount
                for (int i = 0; i < Random.Range(1, maxRandomFoodCount + 1); i++)
                {
                    //add food ui object and set rotation and position on canvas
                    characterStatusScript.newFoodbar.Add(Instantiate(foodbarPrefab.gameObject, panel.transform));
                    characterStatusScript.newFoodbar[characterStatusScript.index].GetComponent<RectTransform>().localRotation = Quaternion.identity;
                    characterStatusScript.newFoodbar[characterStatusScript.index].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(foodbarPrefab.rectTransform.anchoredPosition3D.x + characterStatusScript.offsetXPos
                                                                                                    , foodbarPrefab.rectTransform.anchoredPosition3D.y
                                                                                                    , foodbarPrefab.rectTransform.anchoredPosition3D.z);
                    //offset position for adding ui objects
                    characterStatusScript.offsetXPos += -30;
                    characterStatusScript.index++;
                }
                _audioSource.PlayOneShot(_audioClip[2]);
                isFood = false;

            }
            else if (isAmmo && other.gameObject.CompareTag("Ammo"))
            {
                //destroy scene ammo gameobject
                Destroy(other.gameObject);

                //increase ammo count by random amount
                playerGunControllerScript.ammoCount += Random.Range(1, maxRandomAmmoCount + 1);

                //change ammo ui
                ammoCountText.text = playerGunControllerScript.ammoCount.ToString();

                _audioSource.PlayOneShot(_audioClip[2]);
                isAmmo = false;
            }
            else if (isCampfire && other.gameObject.CompareTag("CampfireInteractArea"))
            {
                if (other.gameObject.GetComponentInParent<CampfireFuelLevel>().currentFuelLevel > 0)
                {
                    other.gameObject.GetComponentInParent<CampfireFuelLevel>().currentFuelLevel = 0;
                    _audioSource.PlayOneShot(_audioClip[1]);
                    isCampfire = false;
                }
            }
        }
    }

    void Update()
    {
        //check if character is under ceiling
        if (Physics.Raycast(transform.position + new Vector3(0, 2f, 0), Vector2.up, isUnderCeilingHeight))
        {
            isUnderCeiling = true;
        }
        else
        {
            isUnderCeiling = false;
        }
    }

    private void Eat(InputAction.CallbackContext context)
    {
        if (context.performed && characterStatusScript.index > 0 && !anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS") && Time.timeScale == 1)
        {
            hungerLevel += eatValue;

            Destroy(characterStatusScript.newFoodbar[characterStatusScript.index - 1]);
            characterStatusScript.newFoodbar.RemoveAt(characterStatusScript.index - 1);
            characterStatusScript.offsetXPos += 30;
            characterStatusScript.index--;

            //trigger eating animation
            anim.SetTrigger("eat");

            _audioSource.PlayOneShot(_audioClip[Random.Range(4, _audioClip.Length)]);
        }
    }

    private void Drink(InputAction.CallbackContext context)
    {
        if (context.performed && characterStatusScript.waterLevel > 0 && !anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS") && Time.timeScale == 1)
        {
            thirstLevel += drinkValue;
            characterStatusScript.waterLevel -= drinkValue;

            anim.SetTrigger("drink");

            _audioSource.PlayOneShot(_audioClip[3]);
        }
    }

    private void PlaceCampfire(InputAction.CallbackContext context)
    {
        if (context.performed && vehicleFuelManagerScript.currentFuel > 0)
        {
            // check if previous campfire is extinguised before allowing for placing new one 
            if (characterStatusScript.newCampfire != null)
            {
                if (characterStatusScript.newCampfire.GetComponent<CampfireFuelLevel>().currentFuelLevel > 0)
                {
                    return;
                }
            }

            //spawn at raycast hit point
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0.4f, 0), transform.forward, out hit, campfireOffsetPosition.magnitude))
            {
                characterStatusScript.newCampfire = Instantiate(campfirePrefab, hit.point, transform.rotation);
            }
            //spawn at default offset position
            else
            {
                characterStatusScript.newCampfire = Instantiate(campfirePrefab, transform.position + transform.forward, transform.rotation);
            }
            vehicleFuelManagerScript.currentFuel -= fuelValue;
            _audioSource.PlayOneShot(_audioClip[0]);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            isFood = true;
            this.other = other;
        }
        else if (other.gameObject.CompareTag("Ammo"))
        {
            isAmmo = true;
            this.other = other;
        }
        else if (other.gameObject.CompareTag("CampfireInteractArea"))
        {
            isCampfire = true;
            this.other = other;
        }
        else if (other.gameObject.CompareTag("CampfireWarmthArea"))
        {
            isInCampfireArea = true;
            if (other.gameObject.GetComponent<CampfireFuelLevel>().currentFuelLevel > 0)
            {
                temperatureLevel += temperatureRefillValue;
            }
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
        if (other.gameObject.CompareTag("CampfireWarmthArea"))
        {
            isInCampfireArea = false;
        }
        if (other.gameObject.CompareTag("CampfireInteractArea"))
        {
            isCampfire = false;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            characterStatusScript.waterLevel += waterRefillRate;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + new Vector3(0, .1f, 0), transform.forward);
        Gizmos.DrawRay(transform.position + new Vector3(0, 2, 0), Vector2.up * isUnderCeilingHeight);
    }
}
