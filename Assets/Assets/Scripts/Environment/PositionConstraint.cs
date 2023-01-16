using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionConstraint : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Vector3 offset;
    void Update()
    {
        transform.position = player.position + offset;
    }
}
