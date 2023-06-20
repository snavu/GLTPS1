using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnvironmentTrace : MonoBehaviour
{
    public Node node;
    private NodeData nodeDataScript;
    public bool triggerTraceFromPillarNode;
    public bool triggerTraceFromPlayerNode;

    void Start()
    {
        nodeDataScript = GameObject.FindWithTag("NodeData").GetComponent<NodeData>();

    }
    void LateUpdate()
    {
        if (triggerTraceFromPillarNode)
        {
            StartCoroutine(TriggerTraceFromPillarNode());
            triggerTraceFromPillarNode = false;
        }

        if (triggerTraceFromPlayerNode)
        {
            StartCoroutine(node.TracePathFromPlayerNode());
            triggerTraceFromPlayerNode = false;
        }
    }

    IEnumerator TriggerTraceFromPillarNode()
    {
        yield return new WaitForSeconds(2);
        nodeDataScript.branch.Add(new EdgeCollection());
        node.TracePathFromPillarNode(nodeDataScript.pillarIndex);
        nodeDataScript.pillarIndex++;
    }
}
