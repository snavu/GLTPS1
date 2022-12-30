using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    public Transform destination;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = destination.position;
    }
    void Update()
    {
        
    }
}
