using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayeGunController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputInitialize playerInputScript;
    [SerializeField]
    private Transform muzzle;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private GameObject bulletHoleDecal;
    void Start()
    {
        playerInputScript.actions.Player.Fire.performed += Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Cast a ray from the muzzle position to the center of the screen
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // Check if the ray hits something
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Determine the direction of the bullet
                Vector3 bulletDirection = (hit.point - muzzle.position).normalized;

                //instantiate decal 
                //note: minus hit.normal to spawn position to offset the decal object into the mesh along direction of normal and prevent z-fighting
                Quaternion newBulletHoleDecalRotation = Quaternion.LookRotation(hit.normal, Vector3.up);
                GameObject newBulletHoleDecal = Instantiate(bulletHoleDecal, hit.point - (hit.normal * 0.1f), newBulletHoleDecalRotation);
            }
        }

        void OnDrawGizmos()
        {
            // Cast a ray from the muzzle position to the center of the screen
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // Check if the ray hits something
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            // Determine the direction of the bullet
            Vector3 bulletDirection = (hit.point - muzzle.position).normalized;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(muzzle.position, bulletDirection * 100);
        }
    }
}

