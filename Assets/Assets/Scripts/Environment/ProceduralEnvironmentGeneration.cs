using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubList
{
    [SerializeField]
    public List<GameObject> nodePrefabsSubList = new List<GameObject>();
}
public class ProceduralEnvironmentGeneration : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> edgePrefabs;
    [SerializeField]
    private List<GameObject> edgeList;
    [SerializeField]
    private List<GameObject> nodePrefabs;
    [SerializeField]
    private List<SubList> nodePrefabsCopy;
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
        //if next node is colliding, destroy it, remove from lists, and generate next node
        for (int index = 0; index < nodeList.Count; index++)
        {
            if (nodePrefabsCopy[index].nodePrefabsSubList[0] != null && nodeList[index].GetComponent<ProceduralEnvironmentGeneration>().isColliding)
            {
                Destroy(nodeList[index]);
                Destroy(edgeList[index]);

                nodePrefabsCopy[index].nodePrefabsSubList.Remove(nodeList[index]);

                nodeList.RemoveAt(index);
                edgeList.RemoveAt(index);

                if (nodePrefabsCopy[index].nodePrefabsSubList[0] != null)
                {
                    edgeList.Insert(index, GenerateRandomEdge(portList[index]));
                    nodeList.Insert(index, GenerateRandomNode(nodePrefabsCopy[index].nodePrefabsSubList, edgeList[index], index));
                }
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
        //initialize edge, copy of node prefab list, and node at the port index 
        edgeList.Add(GenerateRandomEdge(port));
        nodePrefabsCopy.Add(new SubList());
        foreach (GameObject node in nodePrefabs)
        {
            nodePrefabsCopy[index].nodePrefabsSubList.Add(node);
        }
        nodeList.Add(GenerateRandomNode(nodePrefabsCopy[index].nodePrefabsSubList, edgeList[index], index));

    }

    private GameObject GenerateRandomEdge(Transform port)
    {
        //generate edge
        GameObject edge = edgePrefabs[Random.Range(0, edgePrefabs.Count)];
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
        return (node.transform.position - nodeEntrance.position);
    }

    private GameObject GenerateRandomNode(List<GameObject> nodePrefabs, GameObject edgeClone, int index)
    {
        //generate node
        GameObject node = nodePrefabs[Random.Range(0, nodePrefabs.Count)];
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
