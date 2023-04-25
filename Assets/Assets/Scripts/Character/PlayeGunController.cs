using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations;
using TMPro;
public class PlayerGunController : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject bulletHoleDecal;
    public int ammoCount = 10;

    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float recoilValue;
    [SerializeField] private GameObject barrelSmoke;
    private ConstraintSource source;
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    private LayerMask layerMask;

    public AudioSource audioSourceGunFire;
    public AudioSource audioSourceGunBolt;

    [SerializeField] private AudioClip[] _audioClip;
    private bool reload = true;

    void OnEnable()
    {
        playerInputScript.actions.Player.Fire.performed += Fire;
        layerMask = LayerMask.GetMask("Default", "Vehicle");

        ammoCountText.text = ammoCount.ToString();
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Fire.performed -= Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed && ammoCount != 0 &&
            !anim.GetCurrentAnimatorStateInfo(5).IsTag("Reload") &&
            anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS") &&
            reload &&
            Time.timeScale == 1)
        {
            // Set ray from the viewport to world space
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // Get raycast hit from hitmarker ui object
            RaycastHit cameraRayCastHit;
            //Physics.Raycast(ray, out cameraRayCastHit, 100f, layerMask);
            Physics.Raycast(ray, out cameraRayCastHit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore);


            // Determine the direction of the bullet
            Vector3 bulletDirection = (cameraRayCastHit.point - muzzle.position).normalized;
            //cast ray from muzzle position to camera raycast hit position
            RaycastHit bulletRayCastHit;
            //ray cast from muzzle and instantiate decal 
            //note: minus hit.normal to spawn position to offset the decal object into the mesh along direction of normal and prevent z-fighting
            if (Physics.Raycast(muzzle.position, bulletDirection, out bulletRayCastHit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {
                Quaternion newBulletHoleDecalRotation = Quaternion.LookRotation(bulletRayCastHit.normal, Vector3.up);
                GameObject newBulletHoleDecal = Instantiate(bulletHoleDecal, bulletRayCastHit.point - (bulletRayCastHit.normal * 0.1f), newBulletHoleDecalRotation);

                newBulletHoleDecal.transform.parent = bulletRayCastHit.collider.gameObject.transform;
            }

            //decrease ammo count per shot
            ammoCount--;
            ammoCountText.text = ammoCount.ToString();

            //camera recoil effect
            freeLookCamera.m_YAxis.Value += recoilValue;

            StartCoroutine(SmokeEffect(1f));

            audioSourceGunFire.PlayOneShot(_audioClip[0]);
            StartCoroutine(BoltAction(0.25f));
            reload = false;
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

    IEnumerator BoltAction(float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSourceGunBolt.PlayOneShot(_audioClip[1]);
        anim.SetTrigger("reload");
        reload = true;
    }


    void OnDrawGizmos()
    {
        // Cast a ray from the viewport to world space
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Get raycast hit from hitmarker ui object
        RaycastHit cameraRayCastHit;
        Physics.Raycast(ray, out cameraRayCastHit);

        // Determine the direction of the bullet
        Vector3 bulletDirection = (cameraRayCastHit.point - muzzle.position).normalized;


        Gizmos.color = Color.red;
        Gizmos.DrawRay(muzzle.position, bulletDirection * 100);

    }
}

