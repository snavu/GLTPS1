using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnvironmentGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject[] edge;
    [SerializeField]
    private GameObject[] node;

    private GameObject[] port;

    void Start()
    {
        //initialize array of ports of the current node
        var portList = new List<GameObject>();
        foreach (GameObject port in transform)
        {
            if (port.CompareTag("port"))
            {
                portList.Add(port);

            }
        }
        port = portList.ToArray();
    }
    void Update()
    {

    }

    //run generate script per NavMeshSceneGeometry gameobject to generate layers of local nodes
    private void GenerateSceneGeometry(GameObject port)
    {
        //generate edge
        GameObject edge = GenerateRandomEdge();
        Vector3 edgeEntranceOffsetPos = CalculateEdgeEntranceOffsetPosition(edge);
        GameObject edgeCopy = Instantiate(edge, port.transform.position + edgeEntranceOffsetPos, edge.transform.rotation);

        //generate node
        GameObject node = GenerateRandomNode();
        Vector3 nodeEntranceOffsetPos = CalculateNodeEntranceOffsetPosition(node);
        GameObject nodeCopy = Instantiate(node, node.transform.position + nodeEntranceOffsetPos, node.transform.rotation);

    }

    private GameObject GenerateRandomEdge()
    {
        return edge[Random.Range(0, edge.Length)];
    }

    private Vector3 CalculateEdgeEntranceOffsetPosition(GameObject edge)
    {
        Transform edgeEntrance = edge.transform.GetChild(0).transform;
        return (edge.transform.position - edgeEntrance.position);
    }
    private Vector3 CalculateNodeEntranceOffsetPosition(GameObject node)
    {
        Transform nodeEntrance = node.transform.GetChild(0).transform;
        return (node.transform.position - nodeEntrance.position);
    }

    private GameObject GenerateRandomNode()
    {
        return node[Random.Range(0, node.Length)];
    }

}
