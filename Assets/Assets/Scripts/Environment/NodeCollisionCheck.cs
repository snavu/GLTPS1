using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCollisionCheck : MonoBehaviour
{
    public bool isColliding = false;
    private bool isActiveFlag = false;

    public bool allowCollisionCheck = true;

    void Update()
    {
        //if not colliding, set meshes to visible
        if (!isColliding && !isActiveFlag)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isActiveFlag = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (allowCollisionCheck && other.gameObject.CompareTag("Node"))
        {
            isColliding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (allowCollisionCheck && other.gameObject.CompareTag("Node"))
        {
            isColliding = false;
        }
    }
}
