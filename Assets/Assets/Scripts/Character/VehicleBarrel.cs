using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBarrel : MonoBehaviour
{
    public bool isInBarrelDropArea = false;
    [SerializeField]
    private SkinnedMeshRenderer ketBarrelMesh;
    [SerializeField]
    private CharacterItemInteraction characterItemInteractionScript;
    [SerializeField]

    void Start()
    {
        ketBarrelMesh = GameObject.FindWithTag("BarrelDropArea").GetComponent<SkinnedMeshRenderer>();
        characterItemInteractionScript = GameObject.FindWithTag("Player").GetComponent<CharacterItemInteraction>();

        //hide the barrel mesh of the kettengrad
        ketBarrelMesh.enabled = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BarrelDropArea") && !characterItemInteractionScript.isCarrying)
        {
            ketBarrelMesh.enabled = true;
            Destroy(gameObject);
        }


    }
}
