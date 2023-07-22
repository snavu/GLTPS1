using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBarrel : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer ketBarrelMesh;
    [SerializeField]
    public CharacterBarrelInteraction characterBarrelInteractionScript;
    [SerializeField] private VehicleFuelManager vehicleFuelManagerScript;
    [SerializeField] private FadeHUD fadeHUDScript;
    public bool isFueling;

    private float elapsed = 0f;
    [SerializeField] private float timeThreshold = 1f;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioSource audioScourceMountBarrel;
    private bool triggerFuelingAudio;
    void Start()
    {
        ketBarrelMesh = GameObject.FindWithTag("BarrelDropArea").GetComponent<SkinnedMeshRenderer>();
        vehicleFuelManagerScript = GameObject.FindWithTag("Vehicle").GetComponent<VehicleFuelManager>();
        fadeHUDScript = GameObject.FindWithTag("UI").GetComponent<FadeHUD>();
        audioScourceMountBarrel = GameObject.FindWithTag("Vehicle").GetComponentInChildren<AudioSource>();


        //hide the barrel mesh of the kettengrad
        ketBarrelMesh.enabled = false;

        //assign reference for FadeHUD script
        fadeHUDScript.vehicleBarrelScript = this;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > timeThreshold)
        {
            _audioSource.Stop();
            isFueling = false;
            triggerFuelingAudio = false;
        }

        if (_audioSource != null && isFueling && !triggerFuelingAudio)
        {
            _audioSource.Play();
            triggerFuelingAudio = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BarrelDropArea") && !characterBarrelInteractionScript.isCarrying)
        {
            ketBarrelMesh.enabled = true;
            characterBarrelInteractionScript.isPickupable = false;
            audioScourceMountBarrel.Play();
            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Fuel"))
        {
            vehicleFuelManagerScript.currentFuel += vehicleFuelManagerScript.refuelingRate * Time.fixedDeltaTime;
            isFueling = true;
            elapsed = 0f;
        }
    }
}
