using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Transform> portPrefab;
    public List<Transform> portList;
    public int numBlockedPorts;
    public bool isDeadEnd;
    [SerializeField] private int index = 0;

    public GenerateNode edge;
    public bool isColliding = true;
    private bool isActive = false;
    public bool allowCollisionCheck = true;
    [SerializeField] private bool resetPorts = true;
    private NodeData nodeDataScript;

    public bool tracePathFromPillarNode;
    public bool tracePathFromPlayerNode;
    public bool isIntersectionNodeSet;

    //cache yield instructions
    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    void Start()
    {
        nodeDataScript = GameObject.FindWithTag("NodeData").GetComponent<NodeData>();
    }
    IEnumerator GenerateSceneGeometry()
    {
        // enable GenerateEdge script for each port
        yield return waitForSeconds;
        if (index < portPrefab.Count)
        {
            portPrefab[index].GetComponent<GenerateEdge>().enabled = true;
            index++;

            StartCoroutine(GenerateSceneGeometry());
        }

        if (index == portPrefab.Count - 1)
        {
            nodeDataScript.count++;
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
        // start generation
        if (Vector3.Distance(transform.position, nodeDataScript.player.transform.position) < nodeDataScript.spawnThreshold && resetPorts)
        {
            while (portList.Count != 0)
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

        //check if this node is an intersection between player and pillar nodes
        if (tracePathFromPlayerNode && tracePathFromPillarNode && !isIntersectionNodeSet)
        {
            nodeDataScript.SetIntersectionNode(this);
            isIntersectionNodeSet = true;
        }

        //check if this node is a dead end
        if (numBlockedPorts == portPrefab.Count - 1)
        {
            isDeadEnd = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (allowCollisionCheck && other.gameObject.CompareTag("Node"))
        {
            isColliding = false;
        }
    }

    // pathfinding algorithm for map markers 
    public IEnumerator TracePathFromPlayerNode()
    {
        yield return new WaitForEndOfFrame();

        tracePathFromPlayerNode = true;

        if (edge != null)
        {
            edge.tracePathFromPlayerNode = true;

            if (nodeDataScript.edgesFromPlayer == null)
            {
                nodeDataScript.edgesFromPlayer = new List<GenerateNode>();
            }
            nodeDataScript.edgesFromPlayer.Add(edge);

            // call same method for previous node
            StartCoroutine(edge.port.GetComponentInParent<Node>().TracePathFromPlayerNode());
        }
    }
    public void TracePathFromPillarNode(int pillarIndex)
    {
        tracePathFromPillarNode = true;

        if (edge != null)
        {
            edge.tracePathFromPillarNode = true;

            if (nodeDataScript.branch[pillarIndex].edges == null)
            {
                nodeDataScript.branch[pillarIndex].edges = new List<GenerateNode>();
            }

            nodeDataScript.branch[pillarIndex].edges.Add(edge);

            // call same method for previous node
            edge.port.GetComponentInParent<Node>().TracePathFromPillarNode(pillarIndex);
        }
    }
}

