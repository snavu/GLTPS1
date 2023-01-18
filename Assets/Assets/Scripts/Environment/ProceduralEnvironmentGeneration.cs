using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnvironmentGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject[] edges;
    [SerializeField]
    private GameObject[] nodes;
    [SerializeField]
    private Transform[] ports;

    void Start()
    {
        //initialize array of ports of the current node
        var portList = new List<Transform>();
        foreach (Transform port in transform)
        {
            if (port.CompareTag("Port"))
            {
                portList.Add(port);
            }
        }
        ports = portList.ToArray();

        //generate bridge at first port
        GenerateSceneGeometry(ports[0]);
    }

    //run generate script per NavMeshSceneGeometry gameobject to generate layers of local nodes
    private void GenerateSceneGeometry(Transform port)
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
        return edges[Random.Range(0, edges.Length)];
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
        return nodes[Random.Range(0, nodes.Length)];
    }

}
