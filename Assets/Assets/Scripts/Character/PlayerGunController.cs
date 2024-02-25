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
    public int maxAmmoCount = 10;
    [SerializeField] private int maxClipCapacity = 5;
    public int ammoCount = 0;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float recoilValue;
    [SerializeField] private GameObject barrelSmoke;
    private ConstraintSource source;
    public Animator anim;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    private LayerMask layerMask;

    public AudioSource audioSourceGunFire;
    public AudioSource audioSourceGunBolt;
    public AudioSource audioSourceGunEmpty;
    [SerializeField] private AudioClip[] _audioClip;
    public List<GameObject> ammoCountUI;
    [SerializeField] private RawImage ammoPrefab;
    [SerializeField] private GameObject panel;
    [SerializeField] private int offsetXPos;
    public bool isReloading = false;
    [SerializeField] private CrosshairRecoil crosshairRecoilScript;

    void OnEnable()
    {
        playerInputScript.actions.Player.Fire.performed += Fire;
        playerInputScript.actions.Player.Reload.performed += Reload;
        layerMask = LayerMask.GetMask("Default", "Vehicle");

        //initailize ammo count ui
        Reload(maxClipCapacity);
        ammoCountText.text = maxAmmoCount.ToString();
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Fire.performed -= Fire;
        playerInputScript.actions.Player.Reload.performed -= Reload;
    }

    private void Reload(InputAction.CallbackContext context)
    {
        if (context.performed && !isReloading && maxAmmoCount > 0)
        {
            StartCoroutine(ReloadWait(3.5f));
        }
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed &&
            !anim.GetCurrentAnimatorStateInfo(5).IsTag("Reload") &&
            anim.GetCurrentAnimatorStateInfo(3).IsTag("ADS") &&
            !isReloading &&
            Time.timeScale == 1)
        {

            // If ammo has been exhausted
            if (ammoCount == 0)
            {
                audioSourceGunEmpty.PlayOneShot(_audioClip[2]);
                return;
            }

            // Decrement ammo count
            if (ammoCountUI[ammoCount - 1] != null)
            {
                Destroy(ammoCountUI[ammoCount - 1]);
                ammoCountUI.RemoveAt(ammoCount - 1);
            }

            offsetXPos += 18;
            ammoCount--;

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
            crosshairRecoilScript.fire = true;

            StartCoroutine(SmokeEffect(1f));

            audioSourceGunFire.PlayOneShot(_audioClip[0]);
            StartCoroutine(BoltAction(0.25f));
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
        isReloading = true;
        anim.SetTrigger("reload");
        yield return new WaitForSeconds(duration);
        audioSourceGunBolt.PlayOneShot(_audioClip[1]);
        isReloading = false;
    }

    IEnumerator ReloadWait(float duration)
    {
        isReloading = true;
        audioSourceGunEmpty.PlayOneShot(_audioClip[3]);
        yield return new WaitForSeconds(duration);

        int reloadCount = maxClipCapacity - ammoCount;
        if (maxAmmoCount < reloadCount)
        {
            reloadCount = maxAmmoCount;
        }
        Reload(reloadCount);

    }

    private void Reload(int reloadCount)
    {
        for (int i = 0; i < reloadCount; i++)
        {
            //add ammo count ui object and set rotation and position on canvas
            ammoCountUI.Add(Instantiate(ammoPrefab.gameObject, panel.transform));
            ammoCountUI[ammoCount].GetComponent<RectTransform>().localRotation = Quaternion.identity;
            ammoCountUI[ammoCount].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(ammoPrefab.rectTransform.anchoredPosition3D.x + offsetXPos
                                                                                            , ammoPrefab.rectTransform.anchoredPosition3D.y
                                                                                            , ammoPrefab.rectTransform.anchoredPosition3D.z);
            //offset position for adding ui objects
            offsetXPos += -18;

            ammoCount++;
            maxAmmoCount--;
        }

        ammoCountText.text = maxAmmoCount.ToString();

        isReloading = false;
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

