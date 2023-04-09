using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleLeftTrigger : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            playerAnim.SetBool("left collision", true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            playerAnim.SetBool("left collision", false);
        }
    }
}