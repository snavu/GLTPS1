using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Transform> portPrefab;
    public List<Transform> portList;
    [SerializeField]
    private int index = 0;

    public GenerateNode edge;
    public bool isColliding = true;
    private bool isActive = false;
    public bool allowCollisionCheck = true;

    private NodeCache nodeCacheScript;

    private bool resetPorts = true;

    //cache yield instructions
    WaitForSeconds waitForSeconds = new WaitForSeconds(0.25f);
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    void Start()
    {
        nodeCacheScript = GameObject.FindWithTag("NodeCache").GetComponent<NodeCache>();
    }
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
            //remove port from list
            portList.RemoveAt(edge.portIndex);

            //store new port selection
            edge.portIndex = Random.Range(0, GetComponent<Node>().portList.Count);

            //recalculate rotation and offset position of node
            Vector3 nodeEntranceOffsetPos = GenerateNode.CalculateNodeEntranceOffset(gameObject, edge.portIndex, edge.edgeExit);
            transform.position = edge.edgeExit.position + nodeEntranceOffsetPos;
        }
    }

    void LateUpdate()
    {
        if (Vector3.Distance(transform.position, nodeCacheScript.player.transform.position) < nodeCacheScript.spawnThreshold && resetPorts)
        {
            while(portList.Count != 0)
            {
                portList.RemoveAt(0);
            }

            //initialize list of ports
            foreach (Transform port in portPrefab)
            {
                portList.Add(port);
            }

            StartCoroutine(GenerateSceneGeometry());
            StartCoroutine(CheckCollision());
            resetPorts = false;
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

