using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleLeftTrigger : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Untagged") ||
            other.gameObject.CompareTag("Metal") ||
            other.gameObject.CompareTag("Pipe") ||
            other.gameObject.CompareTag("Elevator"))
        {
            playerAnim.SetBool("left collision", true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Untagged") ||
            other.gameObject.CompareTag("Metal") ||
            other.gameObject.CompareTag("Pipe") ||
            other.gameObject.CompareTag("Elevator"))
        {
            playerAnim.SetBool("left collision", false);
        }
    }
}