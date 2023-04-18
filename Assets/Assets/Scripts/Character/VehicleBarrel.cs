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
    private float timeThreshold = 1f;

    void Start()
    {
        ketBarrelMesh = GameObject.FindWithTag("BarrelDropArea").GetComponent<SkinnedMeshRenderer>();
        vehicleFuelManagerScript = GameObject.FindWithTag("Vehicle").GetComponent<VehicleFuelManager>();
        fadeHUDScript = GameObject.FindWithTag("UI").GetComponent<FadeHUD>();

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
            isFueling = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BarrelDropArea") && !characterBarrelInteractionScript.isCarrying)
        {
            ketBarrelMesh.enabled = true;
            characterBarrelInteractionScript.isPickupable = false;
            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Fuel"))
        {
            Debug.Log(true);
            vehicleFuelManagerScript.currentFuel += vehicleFuelManagerScript.refuelingRate;
            isFueling = true;
            elapsed = 0f;
        }
    }
}
