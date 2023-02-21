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

    [SerializeField]

    void Start()
    {
        //conditions for generating nodes
        if (generate && nodePrefabs[0] != null)
        {
            for (int index = 0; index < portList.Count; index++)
            {
                //generate bridge and next node for every port of current node
                GenerateSceneGeometry(portList[index], index);
            }
        }
    }
    

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

        //set rotation for calculating offset vector
        edge.transform.rotation = port.rotation;

        //calculate offset position
        Vector3 edgeEntranceOffsetPos = CalculateEdgeOffsetPosition(edge);

        GameObject newEdge = Instantiate(edge, port.transform.position + edgeEntranceOffsetPos, port.rotation);
        return newEdge;
    }

    private Vector3 CalculateEdgeOffsetPosition(GameObject edge)
    {
        Transform edgeEntrance = edge.transform.GetChild(0).transform;
        return edge.transform.position - edgeEntrance.position;
    }

    private GameObject GenerateRandomNode(List<GameObject> nodePrefabs, GameObject newEdge, int index)
    {
        //generate node
        GameObject node = nodePrefabs[Random.Range(0, nodePrefabs.Count)];
        Transform edgeExit = newEdge.transform.GetChild(1).transform;

        //set rotation for calculating offset vector
        node.transform.rotation = edgeExit.rotation; 

        //calculate offset position
        Vector3 nodeEntranceOffsetPos = CalculateNodeEntranceOffsetPosition(node);
        GameObject newNode = Instantiate(node, edgeExit.position + nodeEntranceOffsetPos, edgeExit.rotation);

        //GameObject newNode = Instantiate(node, edgeExit.position, edgeExit.rotation);

        //trim "(Clone)" from instantiated gamgeobject name for string check to nodePrefabsSubList for preventing collided objects from regenerating
        newNode.name = newNode.name.Replace("(Clone)", "").Trim();

        return newNode;
    }

    private Vector3 CalculateNodeEntranceOffsetPosition(GameObject node)
    {
        Transform nodeEntrance = node.GetComponent<ProceduralEnvironmentGeneration>().portList[0];
        return node.transform.position - nodeEntrance.position;
    }

    void Update()
    {
        //if next node is colliding, remove from lists, destroy it, and generate next node
        for (int portIndex = 0; portIndex < nodeList.Count; portIndex++)
        {
            if (nodePrefabsCopy[portIndex].nodePrefabsSubList.Count != 0 && nodeList[portIndex].GetComponent<ProceduralEnvironmentGeneration>().isColliding)
            {
                //remove the nodePrefabsSubList element corresponding to the instantiated nodeList gameobject through string check
                for (int subListIndex = 0; subListIndex < nodePrefabsCopy[portIndex].nodePrefabsSubList.Count; subListIndex++)
                {
                    if (nodePrefabsCopy[portIndex].nodePrefabsSubList[subListIndex].name == nodeList[portIndex].name)
                    {
                        nodePrefabsCopy[portIndex].nodePrefabsSubList.RemoveAt(subListIndex);
                    }
                }

                //destroy instantiated gameobjects from scene
                Destroy(nodeList[portIndex]);
                Destroy(edgeList[portIndex]);

                //reassign null at element index of lists
                nodeList[portIndex] = null;
                edgeList[portIndex] = null;


                //replace elements from lists with regenerated gameobjects
                if (nodePrefabsCopy[portIndex].nodePrefabsSubList.Count != 0)
                {
                    edgeList[portIndex] = GenerateRandomEdge(portList[portIndex]);
                    nodeList[portIndex] = GenerateRandomNode(nodePrefabsCopy[portIndex].nodePrefabsSubList, edgeList[portIndex], portIndex);
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Node"))
        {
            isColliding = true;
        }
    }

}
