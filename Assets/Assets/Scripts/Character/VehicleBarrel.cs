using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBarrel : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer ketBarrelMesh;
    [SerializeField]
    private CharacterBarrelInteraction characterBarrelInteractionScript;
    [SerializeField]

    void Start()
    {
        ketBarrelMesh = GameObject.FindWithTag("BarrelDropArea").GetComponent<SkinnedMeshRenderer>();
        characterBarrelInteractionScript = GameObject.FindWithTag("Player").GetComponent<CharacterBarrelInteraction>();

        //hide the barrel mesh of the kettengrad
        ketBarrelMesh.enabled = false;
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
}
