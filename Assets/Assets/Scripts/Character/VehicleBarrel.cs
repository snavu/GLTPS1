using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBarrel : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer ketBarrelMesh;
    [SerializeField]
    public CharacterBarrelInteraction characterBarrelInteractionScript;
    [SerializeField] private VehicleFuelManager vehicleFuelManagerScript;

    void Start()
    {
        ketBarrelMesh = GameObject.FindWithTag("BarrelDropArea").GetComponent<SkinnedMeshRenderer>();
        vehicleFuelManagerScript = GameObject.FindWithTag("Vehicle").GetComponent<VehicleFuelManager>();

        //hide the barrel mesh of the kettengrad
        ketBarrelMesh.enabled = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BarrelDropArea") && !characterBarrelInteractionScript.isCarrying)
        {
            ketBarrelMesh.enabled = true;
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Fuel"))
        {
            vehicleFuelManagerScript.currentFuel += vehicleFuelManagerScript.refuelingRate * Time.fixedDeltaTime;
        }
    }
}
