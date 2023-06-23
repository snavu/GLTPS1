using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerMapController : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    public Node node;
    [SerializeField] private NodeData nodeDataScript;
    [SerializeField] private Camera mapCamera;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private RawImage[] mapImage;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mapSFX;
    [SerializeField] private float duration = 0.2f;
    public bool isMapEnabled;
    private float elasped;

    public StartEnvironmentTrace startEnvironmentTraceScript;

    void OnEnable()
    {
        playerInputScript.actions.Player.Map.performed += Map;
        playerInputScript.actions.Vehicle.Map.performed += Map;
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Map.performed -= Map;
        playerInputScript.actions.Vehicle.Map.performed += Map;
    }

    private void Map(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isMapEnabled)
            {
                playerInputScript.actions.Player.Possess.Disable();

                //Time.timeScale = 0;
                StartCoroutine(RenderMap(0f));

                nodeDataScript.drawPath = true;

                // reset flags
                nodeDataScript.isFirstIntersectionNode = false;

                if (nodeDataScript.edgesFromPlayer.Count > 0)
                {
                    foreach (GenerateNode edge in nodeDataScript.edgesFromPlayer)
                    {
                        edge.newNode.GetComponent<Node>().tracePathFromPlayerNode = false;
                        edge.newNode.GetComponent<Node>().isIntersectionNodeSet = false;

                    }
                }
                nodeDataScript.edgesFromPlayer = new List<GenerateNode>();
                nodeDataScript.points = new List<Transform>();

                // if player is not on pillar path node, trigger trace path from player node
                if (!node.tracePathFromPillarNode)
                {
                    startEnvironmentTraceScript.triggerTraceFromPlayerNode = true;
                }
                // if player is on pillar path node, set current node player is on as intersect node for recalculating sub branches
                else
                {
                    node.tracePathFromPlayerNode = true;
                    node.isIntersectionNodeSet = false;
                    nodeDataScript.istracePathFromPlayerNodeComplete = true;
                }

                audioSource.Stop();
                audioSource.PlayOneShot(mapSFX);

                StartCoroutine(RenderMap(0.3f));

                isMapEnabled = true;
                elasped = 0;
            }
            else
            {
                playerInputScript.actions.Player.Possess.Enable();

                Time.timeScale = 1;
                isMapEnabled = false;
                elasped = 0;
            }
        }
    }

    IEnumerator RenderMap(float duration)
    {
        yield return new WaitForSeconds(duration);
        // capture bird's eye view
        mapCamera.Render();

        RenderTexture.active = renderTexture;
        Texture2D screenshotTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshotTexture.Apply();
        RenderTexture.active = null;

        mapImage[0].texture = screenshotTexture;
    }

    void Update()
    {
        float mapImageAlpha = mapImage[0].color.a;

        // lerp map UI alpha
        if (isMapEnabled)
        {
            elasped += Time.unscaledDeltaTime;

            foreach (RawImage UIComponent in mapImage)
            {
                Color color = UIComponent.color;
                color.a = Mathf.Lerp(mapImageAlpha, 1, elasped / duration);
                UIComponent.color = color;
            }
        }
        else
        {
            elasped += Time.deltaTime;

            foreach (RawImage UIComponent in mapImage)
            {
                Color color = UIComponent.color;
                color.a = Mathf.Lerp(mapImageAlpha, 0, elasped / duration);
                UIComponent.color = color;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Node>() != null)
        {
            if (other.gameObject.GetComponent<Node>().isActive)
            {
                node = other.gameObject.GetComponent<Node>();
                if (other.gameObject.GetComponent<StartEnvironmentTrace>() != null)
                {
                    startEnvironmentTraceScript = other.gameObject.GetComponent<StartEnvironmentTrace>();
                }
            }
        }
    }

}
