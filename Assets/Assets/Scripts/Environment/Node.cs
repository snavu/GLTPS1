using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Transform> portPrefab;
    public List<Transform> portList;
    [SerializeField]
    private int index = 0;

    public bool generate = false;
    [HideInInspector]
    public GenerateNode edge;

    public bool isColliding = true;
    private bool isActive = false;

    public bool allowCollisionCheck = true;

    void Start()
    {
        //generate edge at each port
        if (generate)
        {
            StartCoroutine(GenerateSceneGeometry());
        }
        StartCoroutine(CheckCollision());
    }

    //cache yield instructions
    WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    IEnumerator GenerateSceneGeometry()
    {
        yield return waitForSeconds;
        if (index < portPrefab.Count)
        {
            portPrefab[index].GetComponent<GenerateEdge>().enabled = true;
            index++;

            StartCoroutine(GenerateSceneGeometry());
        }
    }

    IEnumerator CheckCollision()
    {
        //wait for OnTrigger callbacks
        yield return waitForFixedUpdate;
        
        //check if not colliding, set mesh active and disable collision checking
        if (!isColliding && !isActive)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isActive = true;
            allowCollisionCheck = false;
        }
        else
        {
            //if colliding, loop
            StartCoroutine(CheckCollision());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (allowCollisionCheck && other.gameObject.CompareTag("Node"))
        {
            isColliding = true;
            ConnectFromDifferentPort();
        }
    }

    private void ConnectFromDifferentPort()
    {
        if (portList.Count > 1)
        {
            //remove previous port from list
            portList.RemoveAt(0);

            //recalculate rotation and offset position of node
            Vector3 nodeEntranceOffsetPos = GenerateNode.CalculateNodeEntranceOffset(gameObject, edge.edgeExit);
            transform.position = edge.edgeExit.position + nodeEntranceOffsetPos;
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

