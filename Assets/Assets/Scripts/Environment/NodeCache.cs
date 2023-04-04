using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCache : MonoBehaviour
{
    public List<GameObject> nodeList;
    public GameObject player;
    public float despawnThreshold = 500f;
    public float spawnThreshold = 400f;

    void Awake()
    {
        nodeList.Clear();
    }

    void Update()
    {
        //DespawnNode();
    }

    //despawn node past threshold distance from player
    //note: threshold should be less than the length of the box collider of the NodeCache 
    private void DespawnNode()
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            float distanceFromPlayer = Vector3.Distance(player.transform.position, nodeList[i].transform.position);
            if (distanceFromPlayer > despawnThreshold)
            {
                foreach (Transform child in nodeList[i].transform)
                {
                    Destroy(child.gameObject);
                }
                Destroy(nodeList[i]);
                nodeList.RemoveAt(i);
            }
        }
    }
}