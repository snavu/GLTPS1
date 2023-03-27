using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class PlayerGunController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputInitialize playerInputScript;
    [SerializeField]
    private Transform muzzle;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private GameObject bulletHoleDecal;

    [SerializeField]
    private int ammoCount = 10;

    [SerializeField]
    private CinemachineFreeLook freeLookCamera;
    [SerializeField]
    private float recoilValue;
    private bool isPickupable;
    private Collider other;
    [SerializeField]
    private ParticleSystem barrelSmoke;


    void Start()
    {
        playerInputScript.actions.Player.Fire.performed += Fire;
        playerInputScript.actions.Player.Interact.performed += Interact;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (ammoCount != 0)
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

                    //decrease ammo count per shot
                    ammoCount--;

                    //camera recoil effect
                    freeLookCamera.m_YAxis.Value += recoilValue;

                    StartCoroutine(SmokeEffect(1f));

                }
            }
        }
    }

    IEnumerator SmokeEffect(float duration)
    {
        barrelSmoke.Play();
        yield return new WaitForSeconds(duration);
        barrelSmoke.Stop();
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && isPickupable)
        {
            //destroy scene ammo gameobject
            Destroy(other.gameObject);

            //increase ammo count
            ammoCount++;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            isPickupable = true;
            this.other = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            isPickupable = false;
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

