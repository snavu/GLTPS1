using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private List<Transform> portPrefab;
    public List<Transform> portList;
    [SerializeField]
    private int index = 0;

    public float delay = 0.5f;

    public bool generate = false;

    void Start()
    {
        //initialize list of nodes
        foreach (Transform port in portPrefab)
        {
            portList.Add(port);
        }

        //generate edge at each port
        if (generate)
        {
            StartCoroutine(GenerateSceneGeometry());
        }
    }

    IEnumerator GenerateSceneGeometry()
    {
        yield return new WaitForSeconds(delay);
        if (index < portPrefab.Count)
        {
            portPrefab[index].GetComponent<GenerateEdge>().enabled = true;
            index++;

            StartCoroutine(GenerateSceneGeometry());
        }
    }
}

