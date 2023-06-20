using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanazawaInstanceManager : MonoBehaviour
{
    [SerializeField] private Node node;
    private NodeData nodeDataScript;
    [SerializeField] private GameObject kanazawaCompound;
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
        if (kanazawaCompound != null)
        {
            if (node.isDeadEnd && !kanazawaCompound.activeInHierarchy && !playerMapControllerScript.enabled)
            {
                kanazawaCompound.SetActive(true);
            }
        }

        if (playerMapControllerScript.enabled && isPlayerOnNode && !flag)
        {
            // hide all kanazawas except the one the player interacted with 
            GameObject[] kanazawasToHide = GameObject.FindGameObjectsWithTag("Kanazawa");

            foreach (GameObject kanazawa in kanazawasToHide)
            {
                if (kanazawa != firstKanazawa)
                {
                    Destroy(kanazawa.transform.parent.transform.parent.gameObject);
                }
            }

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
