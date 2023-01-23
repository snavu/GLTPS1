using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnvironmentGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject[] edgePrefabs;
    [SerializeField]
    private List<GameObject> edgeList;
    [SerializeField]
    private GameObject[] nodePrefabs;
    [SerializeField]
    private List<GameObject> nodeList;
    [SerializeField]
    private List<Transform> portList;

    [SerializeField]
    private bool generate;

    [SerializeField]
    private bool isColliding = false;

    private bool isActiveFlag = false;


    void Start()
    {
        if (generate && nodePrefabs[0] != null)
        {
            for (int index = 0; index < portList.Count; index++)
            {
                //generate bridge and next node for every port of current node
                GenerateSceneGeometry(portList[index], index);
            }
        }

    }

    void Update()
    {
        //if next node is colliding, destroy it and regenerate next node
        for (int index = 0; index < nodeList.Count; index++)
        {
            if (nodeList[index] != null && nodeList[index].GetComponent<ProceduralEnvironmentGeneration>().isColliding)
            {
                Destroy(nodeList[index]);
                nodeList[index] = GenerateRandomNode(edgeList[index]);
            }
        }


        if (!isColliding && !isActiveFlag)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isActiveFlag = true;
        }
    }

    //note: run generate script per NavMeshSceneGeometry gameobject to generate layers of local nodes
    private void GenerateSceneGeometry(Transform port, int index)
    {
        //initialize edge and node
        edgeList.Add(GenerateRandomEdge(port));
        nodeList.Add(GenerateRandomNode(edgeList[index]));
    }

    private GameObject GenerateRandomEdge(Transform port)
    {
        //generate edge
        GameObject edge = edgePrefabs[Random.Range(0, edgePrefabs.Length)];
        //set rotation of edge to extend from direction of port
        edge.transform.rotation = port.rotation;
        //calculate offset position
        Vector3 edgeEntranceOffsetPos = CalculateEdgeOffsetPosition(edge);

        GameObject edgeClone = Instantiate(edge, port.transform.position + edgeEntranceOffsetPos, edge.transform.rotation);
        return edgeClone;
    }

    private Vector3 CalculateEdgeOffsetPosition(GameObject edge)
    {
        Transform edgeEntrance = edge.transform.GetChild(0).transform;
        return edge.transform.position - edgeEntrance.position;
    }


    private Vector3 CalculateNodeEntranceOffsetPosition(GameObject node)
    {
        Transform nodeEntrance = node.GetComponent<ProceduralEnvironmentGeneration>().portList[0];
        Debug.Log(nodeEntrance);
        return (node.transform.position - nodeEntrance.position);
    }

    private GameObject GenerateRandomNode(GameObject edgeClone)
    {
        //generate node
        GameObject node = nodePrefabs[Random.Range(0, nodePrefabs.Length)];
        //calculate node entrance offset position (for first port of next node, hardcoded for now)
        Transform edgeExit = edgeClone.transform.GetChild(1).transform;
        //set rotation of node to align with direction of edge exit
        node.transform.rotation = edgeExit.rotation;
        //calculate offset position
        Vector3 nodeEntranceOffsetPos = CalculateNodeEntranceOffsetPosition(node);
        GameObject nodeClone = Instantiate(node, edgeExit.position + nodeEntranceOffsetPos, node.transform.rotation);

        return nodeClone;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Node"))
        {
            isColliding = true;
        }
    }

}
