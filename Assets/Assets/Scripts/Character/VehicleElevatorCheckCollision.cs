using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleElevatorCheckCollision : MonoBehaviour
{
    public bool isInElevator;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            isInElevator = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            isInElevator = false;
        }
    }
}
