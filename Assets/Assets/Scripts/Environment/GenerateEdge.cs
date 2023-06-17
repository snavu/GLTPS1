using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateEdge : MonoBehaviour
{
    [SerializeField] private List<GameObject> edgePrefabs;
    [SerializeField] private GameObject newEdge;
    [SerializeField] private List<GameObject> edgeList;
    private int index = 0;
    [SerializeField] private List<GameObject> barrierPrefabs;
    private bool spawnedBarrier = false;
    private bool resetEdge = false;

    void Start()
    {
        //initialize list of edges
        foreach (GameObject edge in edgePrefabs)
        {
            edgeList.Add(edge);
        }

        //generate first random edge
        if (edgeList.Count != 0)
        {
            newEdge = GenerateRandomEdge();
        }

    }

    void Update()
    {
        // //respawn edge after despawn 
        // if (Vector3.Distance(transform.position, nodeDataScript.player.transform.position) < nodeDataScript.spawnThreshold && resetEdge)
        // {
        //     //clear pre-existing edge list
        //     while (edgeList.Count != 0)
        //     {
        //         edgeList.RemoveAt(0);
        //     }

        //     //initialize list of edges
        //     foreach (GameObject edge in edgePrefabs)
        //     {
        //         edgeList.Add(edge);
        //     }

        //     //generate first random edge
        //     if (edgeList.Count != 0)
        //     {
        //         //regenerate different edge from edge lsit
        //         newEdge = GenerateRandomEdge();
        //     }

        //     resetEdge = false;
        // }

        if (newEdge != null)
        {
            if (newEdge.GetComponent<GenerateNode>().isColliding || newEdge.GetComponent<GenerateNode>().nodeList.Count == 0)
            {
                //no nodes left for which a node will be non-colliding, destroy and remove the edge from edge list
                Destroy(newEdge);
                edgeList.RemoveAt(index);

                if (edgeList.Count != 0)
                {
                    //regenerate different edge from edge list
                    newEdge = GenerateRandomEdge();
                }
            }
        }
        else
        {
            //no edges left in edge list and the port at which the node spawned at is not the same port as this one, generate barrier 
            if (barrierPrefabs.Count != 0 && edgeList.Count == 0 && !spawnedBarrier &&
                GetComponentInParent<Node>().portList[GetComponentInParent<Node>().edge.portIndex].name != gameObject.name)
            {
                GenerateRandomBarrier();
                spawnedBarrier = true;
            }
        }
    }

    private GameObject GenerateRandomEdge()
    {
        //store index 
        index = Random.Range(0, edgeList.Count);
        //generate edge
        GameObject edge = edgeList[index];
        //set rotation for calculating offset vector
        edge.transform.rotation = transform.rotation;

        //calculate offset position
        Transform edgeEntrance = edge.transform.GetChild(1).transform;
        Vector3 edgeEntranceOffsetPos = edge.transform.position - edgeEntrance.position;

        GameObject newEdge = Instantiate(edge, transform.position + edgeEntranceOffsetPos, transform.rotation);

        newEdge.GetComponent<GenerateNode>().port = this;

        return newEdge;
    }

    private void GenerateRandomBarrier()
    {
        GameObject barrier = barrierPrefabs[Random.Range(0, barrierPrefabs.Count)];
        barrier.transform.rotation = transform.rotation;
        GameObject barrierClone = Instantiate(barrier, transform.position, transform.rotation);
        barrierClone.transform.SetParent(gameObject.transform);
    }
}
