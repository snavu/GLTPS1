using UnityEngine;
using UnityEngine.AI;
using System.Collections;


[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
    IEnumerator Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                yield return StartCoroutine(Move(agent));
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    IEnumerator Move(NavMeshAgent agent)
    {
        //move navmesh agent across offmesh link
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
            yield return null;
        }
    }
}