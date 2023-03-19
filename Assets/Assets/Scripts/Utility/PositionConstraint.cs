using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionConstraint : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    private Vector3 offset;
    void Update()
    {
        transform.position = player.position + offset;
    }
}
