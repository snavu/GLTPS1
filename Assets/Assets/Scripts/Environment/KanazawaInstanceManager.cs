using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanazawaInstanceManager : MonoBehaviour
{
    [SerializeField] private Node node;
    private NodeData nodeDataScript;
    [SerializeField] private StartEnvironmentTrace startEnvironmentTraceScript; 
    [SerializeField] private GameObject kanazawa;
    public GameObject firstKanazawa;
    [SerializeField] private PlayerMapController playerMapControllerScript;
    private bool isPlayerOnNode;


    private bool flag;

    void Start()
    {
        nodeDataScript = GameObject.FindWithTag("NodeData").GetComponent<NodeData>();
        playerMapControllerScript = GameObject.Find("ChitoParent").GetComponent<PlayerMapController>();
    }

    void Update()
    {
        if (node.isDeadEnd && !kanazawa.activeInHierarchy && nodeDataScript.intersectionNode == null)
        {
            kanazawa.SetActive(true);
        }

        if (playerMapControllerScript.enabled && isPlayerOnNode && !flag)
        {
            // hide all kanazawas except the one the player interacted with 
            GameObject[] kanazawasToHide = GameObject.FindGameObjectsWithTag("Kanazawa");

            foreach (GameObject kanazawa in kanazawasToHide)
            {
                if (kanazawa != firstKanazawa)
                {
                    kanazawa.SetActive(false);
                }
            }
            startEnvironmentTraceScript.triggerTraceFromPlayerNode = true;

            flag = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerOnNode = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerOnNode = false;
        }
    }
}
