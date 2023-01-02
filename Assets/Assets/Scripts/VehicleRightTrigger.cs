using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRightTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnim;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            playerAnim.SetBool("right collision", true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            playerAnim.SetBool("right collision", false);
        }
    }
}