using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAreaBaker : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface surface;
    [SerializeField]
    private Transform navMeshAgent;
    [SerializeField]
    private float distanceThreshold;

    void Start()
    {
        UpdateNavMesh();
    }
    void Update()
    {
        //bake navmesh around navmesh agents
        if (Vector3.Distance(navMeshAgent.position, transform.position) >= distanceThreshold)
        {
            UpdateNavMesh();
        }
    }

    private void UpdateNavMesh(){
        surface.RemoveData();
        transform.position = navMeshAgent.position;
        surface.BuildNavMesh(); 
    }
}
