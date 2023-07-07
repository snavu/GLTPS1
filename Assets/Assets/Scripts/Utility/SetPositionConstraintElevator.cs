using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SetPositionConstraintElevator : MonoBehaviour
{
    private ConstraintSource source;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            // set position constraint for chito and yuuri to move up with the elevator
            source.sourceTransform = other.gameObject.transform;
            source.weight = 1;

            GetComponent<PositionConstraint>().SetSource(0, source);
            GetComponent<PositionConstraint>().constraintActive = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            GetComponent<PositionConstraint>().constraintActive = false;
        }
    }
}
