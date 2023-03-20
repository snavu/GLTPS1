using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class CrosshairRecoil : MonoBehaviour
{
    [SerializeField]
    private RawImage crosshairTop;
    [SerializeField]
    private RawImage crosshairRight;
    [SerializeField]
    private RawImage crosshairLeft;
    [SerializeField]
    private RawImage crosshairBottom;
    [SerializeField]
    private float recoilDuration;
    [SerializeField]
    private float recoilResetDuration;
    [SerializeField]
    private Vector2 maxRecoil;

    [SerializeField]
    private float recoilX;
    [SerializeField]
    private float recoilY;

    [SerializeField]
    private PlayerInput playerInputScript;
    private bool fired;
    [SerializeField]
    private bool recoil;


    void Start()
    {
        playerInputScript.actions.Player.Fire.performed += Recoil;
    }

    private void Recoil(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            fired = true;
            Debug.Log("fire");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            recoil = true;
            fired = false;
        }

        if (recoil)
        {
            if ((maxRecoil.y - crosshairTop.rectTransform.anchoredPosition.y) < 1f)
            {
                recoil = false;
            }
            crosshairTop.rectTransform.anchoredPosition = LerpRectTranformPosition(crosshairTop.rectTransform, 0, maxRecoil.y);
            crosshairRight.rectTransform.anchoredPosition = LerpRectTranformPosition(crosshairRight.rectTransform, maxRecoil.x, 0);
            crosshairBottom.rectTransform.anchoredPosition = LerpRectTranformPosition(crosshairBottom.rectTransform, 0, -maxRecoil.y);
            crosshairLeft.rectTransform.anchoredPosition = LerpRectTranformPosition(crosshairLeft.rectTransform, -maxRecoil.x, 0);

        }
        if (!recoil)
        {
            crosshairTop.rectTransform.anchoredPosition = LerpRectTranformPosition(crosshairTop.rectTransform, 0, 0);
            crosshairRight.rectTransform.anchoredPosition = LerpRectTranformPosition(crosshairRight.rectTransform, 0, 0);
            crosshairBottom.rectTransform.anchoredPosition = LerpRectTranformPosition(crosshairBottom.rectTransform, 0, 0);
            crosshairLeft.rectTransform.anchoredPosition = LerpRectTranformPosition(crosshairLeft.rectTransform, 0, 0);
        }


    }

    private Vector2 LerpRectTranformPosition(RectTransform crosshairComponent, float recoilX, float recoilY)
    {

        Vector2 startPos = new Vector2(crosshairComponent.anchoredPosition.x, crosshairComponent.anchoredPosition.y);
        Vector2 targetPos = new Vector2(recoilX, recoilY);
        Vector2 currentPosition = Vector2.Lerp(startPos, targetPos, Time.deltaTime / recoilDuration);

        return currentPosition;
    }
}
