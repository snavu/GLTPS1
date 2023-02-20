using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNode : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> nodePrefabs;
    [SerializeField]
    private GameObject nodeClone;
    public List<GameObject> nodeList;
    private int index = 0;

    [SerializeField]
    private Transform edgeExit;
    private Vector3 nodeEntranceOffsetPos;

    void Start()
    {
        //initialize list of nodes
        foreach (GameObject node in nodePrefabs)
        {
            nodeList.Add(node);
        }

        //initialize edge exit position
        edgeExit = transform.GetChild(1).transform;

        //generate first random edge
        if (nodeList.Count != 0)
        {
            nodeClone = GenerateRandomNode();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (nodeClone != null)
        {
            if (nodeClone.GetComponent<NodeCollisionCheck>().isColliding)
            {
                //remove previous port from list
                nodeClone.GetComponent<Node>().portList.RemoveAt(0);
                
                if (nodeClone.GetComponent<Node>().portList.Count != 0)
                {
                    //recalculate rotation and offset position of node
                    nodeEntranceOffsetPos = CalculateNodeEntranceOffset(nodeClone);
                    nodeClone.transform.position = edgeExit.position + nodeEntranceOffsetPos;
                }
                else
                {
                    //no ports left for which the node will be non-colliding, destroy and remove node from node list
                    Destroy(nodeClone);
                    nodeList.RemoveAt(index);

                    if (nodeList.Count != 0)
                    {
                        //regenerate different node from node list
                        nodeClone = GenerateRandomNode();
                    }
                }
            }
            else
            {
                //node is non-colliding, disable collision detection
                nodeClone.GetComponent<NodeCollisionCheck>().allowCollisionCheck = false;
            }
        }
    }

    private GameObject GenerateRandomNode()
    {
        //store index
        index = Random.Range(0, nodeList.Count);

        //generate node
        GameObject node = nodeList[index];

        nodeEntranceOffsetPos = CalculateNodeEntranceOffset(node);
        GameObject nodeClone = Instantiate(node, edgeExit.position + nodeEntranceOffsetPos, node.transform.rotation);
        return nodeClone;
    }

    private Vector3 CalculateNodeEntranceOffset(GameObject node)
    {
        //rotate node to point in direction of edge exit, and add opposite local angle of port as global angle to node angle 
        Quaternion mirrorRotation = edgeExit.rotation * Quaternion.Euler(0, 180, 0);
        node.transform.rotation = mirrorRotation * Quaternion.Inverse(node.GetComponent<Node>().portList[0].localRotation);

        //calculate offset position 
        return node.transform.position - node.GetComponent<Node>().portList[0].position;
    }
}
