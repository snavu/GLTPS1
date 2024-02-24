using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class PlayerGunController : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject bulletHoleDecal;
    public int ammoCount = 10;
    public int reloadCount = 0;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float recoilValue;
    [SerializeField] private GameObject barrelSmoke;
    private ConstraintSource source;
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private RawImage ammoClip;
    private LayerMask layerMask;

    public AudioSource audioSourceGunFire;
    public AudioSource audioSourceGunBolt;
    public AudioSource audioSourceGunEmpty;
    [SerializeField] private AudioClip[] _audioClip;
    
    [SerializeField] public Texture ammo1;
    [SerializeField] public Texture ammo2;
    [SerializeField] public Texture ammo3;
    [SerializeField] public Texture ammo4;
    [SerializeField] public Texture ammo;
    [SerializeField] public Texture blank;
    private bool reload = true;

    void OnEnable()
    {
        playerInputScript.actions.Player.Fire.performed += Fire;
        playerInputScript.actions.Player.Reload.performed += Reload;
        layerMask = LayerMask.GetMask("Default", "Vehicle");

        ammoCountText.text = ammoCount.ToString();

    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Fire.performed -= Fire;
        playerInputScript.actions.Player.Reload.performed -= Reload;
    }

    private void Reload(InputAction.CallbackContext context)
    {   
        if (context.performed)
        {
            // Basic reload
            reload = false;
            if (ammoCount == 0)
            {
                return;
            }
            if (ammoCount < 5)
            {
                reloadCount = 5 - ammoCount;
                ammoCount = 0;
            }
            else
            {
                ammoCount += -reloadCount;
                reloadCount = 0;
            }
            ammoCountText.text = ammoCount.ToString();
            StartCoroutine(ReloadWait(3.5f));          
        }
    
    }
    
    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed &&
            !anim.GetCurrentAnimatorStateInfo(5).IsTag("Reload") &&
            anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS") &&
            reload &&
            Time.timeScale == 1)
        {  

            // If ammo has been exhausted
            if (reloadCount == 5)
            {
                audioSourceGunEmpty.PlayOneShot(_audioClip[2]);
                return;
            }

            if (reloadCount == 4)
            {
                ammoClip.texture = blank;
            }
            
            // Increment Reload
            reloadCount += 1;

            // not pretty but at least it works
            if (reloadCount == 0)
            {
                ammoClip.texture = ammo;
            }
            if (reloadCount == 1)
            {
                ammoClip.texture = ammo4;
            }
            if (reloadCount == 2)
            {
                ammoClip.texture = ammo3;
            }
            if (reloadCount == 3)
            {
                ammoClip.texture = ammo2;
            }
            if (reloadCount == 4)
            {
                ammoClip.texture = ammo1;
            }

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

    // easiest way to just have the program wait
    // also not pretty
    IEnumerator ReloadWait(float duration)
    {
        audioSourceGunEmpty.PlayOneShot(_audioClip[3]);
        yield return new WaitForSeconds(duration);     
        if (reloadCount == 0)
        {
            ammoClip.texture = ammo;
        }
        if (reloadCount == 1)
        {
            ammoClip.texture = ammo4;
        }
        if (reloadCount == 2)
        {
            ammoClip.texture = ammo3;
        }
        if (reloadCount == 3)
        {
            ammoClip.texture = ammo2;
        }
        if (reloadCount == 4)
        {
            ammoClip.texture = ammo1;
        }
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

