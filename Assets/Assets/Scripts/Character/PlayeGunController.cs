using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations;
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
    public int ammoCount = 10;

    [SerializeField]
    private CinemachineFreeLook freeLookCamera;
    [SerializeField]
    private float recoilValue;
    private bool isPickupable;
    private Collider other;
    [SerializeField]
    private GameObject barrelSmoke;
    private ConstraintSource source;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private PlayerGunMovement playerGunMovementScript;
    [SerializeField]
    private CharacterManager characterManagerScript;

    void Start()
    {
        playerInputScript.actions.Player.Fire.performed += Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed && ammoCount != 0 && 
            !anim.GetCurrentAnimatorStateInfo(5).IsTag("Reload") &&
            anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS") &&
            characterManagerScript.PlayerInputInitialize == playerInputScript)
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

                anim.SetTrigger("reload");
            }
        }
    }

    IEnumerator SmokeEffect(float duration)
    {
        GameObject newBarrelSmoke = Instantiate(barrelSmoke, muzzle.position, transform.rotation);
        source.sourceTransform = camera.transform;
        source.weight = 1;
        newBarrelSmoke.GetComponent<LookAtConstraint>().SetSource(0, source);
        newBarrelSmoke.GetComponent<Rigidbody>().AddForce(muzzle.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(duration);
        Destroy(newBarrelSmoke);
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

