using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorGateCollisionCheck : MonoBehaviour
{
    [SerializeField] private Animator elevatorAnim;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private bool triggerOpenGate;
    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 startPosition;
    private float elasped = 0f;
    [SerializeField] private float duration = 2f;
    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        // check if elevator gate can close all the way
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.transform.forward, out hit, 0.2f))
        {
            if (!hit.collider.gameObject.CompareTag("Elevator"))
            {
                currentPosition = transform.position;
                elevatorAnim.SetBool("CloseGate", false);
                elevatorAnim.enabled = false;
                triggerOpenGate = true;
            }
        }
        // gate unable to close all the way, lerp to open
        if (triggerOpenGate)
        {
            elasped += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPosition, startPosition, elasped / duration);
            if (Vector3.Distance(transform.position, startPosition) < 0.001f)
            {
                elevatorAnim.enabled = true;
                elevatorAnim.SetBool("isGateOpen", true);
                triggerOpenGate = false;
                elasped = 0f;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(raycastOrigin.position, raycastOrigin.transform.forward * 0.2f);
    }
}
