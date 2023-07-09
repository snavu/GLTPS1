using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SetPositionConstraintElevator : MonoBehaviour
{
    public Transform target;
    public float offsetY = 0.15f;
    private void Update()
    {
        if (target != null)
        {
            Vector3 constrainedPosition = transform.position;
            constrainedPosition.y = target.position.y + offsetY;
            transform.position = constrainedPosition;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            target = other.gameObject.transform;
            transform.SetParent(other.gameObject.transform);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            target = null;
            transform.parent = null;
        }
    }
}
