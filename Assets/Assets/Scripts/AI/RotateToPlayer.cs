using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 direction;
    public float rotationSpeed = 100f;

    void Update()
    {
        if (player != null)
        {
            // calculate direction along xz plane
            Vector3 playerPositionXZ = new Vector3(player.position.x, 0, player.position.z);
            direction = playerPositionXZ - transform.position;

            Quaternion rotation = Quaternion.LookRotation(direction);

            // lerp rotate
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        }
    }
}
