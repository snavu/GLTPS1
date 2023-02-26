using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBarrel : MonoBehaviour
{
    public bool isInBarrelDropArea = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BarrelDropArea"))
        {
            isInBarrelDropArea = true;
            Debug.Log(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BarrelDropArea"))
        {
            isInBarrelDropArea = false;
        }
    }
}
