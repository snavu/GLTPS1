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

    [SerializeField]
    private bool generate = false;

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
        //test 
        if (generate)
        {
            //generate bridge at first port
            GenerateSceneGeometry(ports[0]);
        }

    }

    //run generate script per NavMeshSceneGeometry gameobject to generate layers of local nodes
    private void GenerateSceneGeometry(Transform port)
    {
        //generate edge
        GameObject edge = GenerateRandomEdge();
        //set rotation of edge to extend from direction of port
        edge.transform.rotation = port.rotation;
        //calculate offset position
        Vector3 edgeEntranceOffsetPos = CalculateEdgeOffsetPosition(edge);

        GameObject edgeCopy = Instantiate(edge, port.transform.position + edgeEntranceOffsetPos, edge.transform.rotation);

        //generate node
        GameObject node = GenerateRandomNode();
        //calculate node entrance offset position (for first port of next node, hardcoded for now)
        Transform edgeExit = edgeCopy.transform.GetChild(1).transform;
        //set rotation of node to align with direction of edge exit
        node.transform.rotation = edgeExit.rotation;

        //calculate offset position
        Vector3 nodeEntranceOffsetPos = CalculateNodeEntranceOffsetPosition(node, 0);

        GameObject nodeCopy = Instantiate(node, edgeExit.position + nodeEntranceOffsetPos, node.transform.rotation);

    }

    private GameObject GenerateRandomEdge()
    {
        return edges[Random.Range(0, edges.Length)];
    }

    private Vector3 CalculateEdgeOffsetPosition(GameObject edge)
    {
        Transform edgeEntrance = edge.transform.GetChild(0).transform;
        return edge.transform.position - edgeEntrance.position;
    }


    private Vector3 CalculateNodeEntranceOffsetPosition(GameObject node, int portIndex)
    {
        Transform nodeEntrance = node.GetComponent<ProceduralEnvironmentGeneration>().ports[portIndex];
        return (node.transform.position - nodeEntrance.position);
    }

    private GameObject GenerateRandomNode()
    {
        return nodes[Random.Range(0, nodes.Length)];
    }

}
