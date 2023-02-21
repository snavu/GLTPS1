using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNode : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> nodePrefabs;
    [SerializeField]
    private GameObject newNode;
    public List<GameObject> nodeList;
    private int index = 0;
    public Transform edgeExit;


    public bool isColliding = false;
    private bool isActive = false;
    public bool allowCollisionCheck = true;

    void Start()
    {
        //initialize list of nodes
        foreach (GameObject node in nodePrefabs)
        {
            nodeList.Add(node);
        }
        StartCoroutine(CheckCollision());

    }

    // Update is called once per frame
    void Update()
    {
        if (newNode != null)
        {
            if (newNode.GetComponent<Node>().isColliding && newNode.GetComponent<Node>().portList.Count == 1)
            {
                //no ports left for which the node will be non-colliding, destroy and remove node from node list
                Destroy(newNode);
                nodeList.RemoveAt(index);

                if (nodeList.Count != 0)
                {
                    //regenerate different node from node list
                    newNode = GenerateRandomNode();
                }
            }
        }
    }

    IEnumerator CheckCollision()
    {
        //wait for OnTrigger callbacks
        yield return new WaitForFixedUpdate(); ;

        //check if not colliding, set mesh active and disable collision checking
        if (!isColliding && !isActive)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isActive = true;
            allowCollisionCheck = false;

            //generate first random edge
            if (nodeList.Count != 0)
            {
                newNode = GenerateRandomNode();
            }
        }
        else
        {
            //if colliding, loop
            StartCoroutine(CheckCollision());
        }
    }

    private GameObject GenerateRandomNode()
    {
        //store index
        index = Random.Range(0, nodeList.Count);

        //generate node
        GameObject node = nodeList[index];
        Vector3 nodeEntranceOffsetPos = CalculateNodeEntranceOffset(node, edgeExit);
        GameObject newNode = Instantiate(node, edgeExit.position + nodeEntranceOffsetPos, node.transform.rotation);
        newNode.GetComponent<Node>().edge = GetComponent<GenerateNode>();
        return newNode;
    }

    public static Vector3 CalculateNodeEntranceOffset(GameObject node, Transform edgeExit)
    {
        //rotate node to point in direction of edge exit, and add opposite local angle of port as global angle to node angle 
        Quaternion mirrorRotation = edgeExit.rotation * Quaternion.Euler(0, 180, 0);
        node.transform.rotation = mirrorRotation * Quaternion.Inverse(node.GetComponent<Node>().portList[0].localRotation);

        //calculate offset position 
        return node.transform.position - node.GetComponent<Node>().portList[0].position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (allowCollisionCheck && other.gameObject.CompareTag("Node"))
        {
            isColliding = true;
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
