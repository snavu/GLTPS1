using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BuildNavMesh : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface surface;

    void Start()
    {
        surface.BuildNavMesh();
    }
}
