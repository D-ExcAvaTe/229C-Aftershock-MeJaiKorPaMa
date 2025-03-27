using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasLook : MonoBehaviour
{
    private PlayerController playerController;
    private Transform cameraToLookAt;
    private Canvas worldCanvas;

    void Start()
    {
        worldCanvas = GetComponent<Canvas>();

        if (worldCanvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogError("WorldSpaceLookAtPlayer script is attached to a non-WorldSpace Canvas!", this);
            enabled = false;
            return;
        }

        playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            cameraToLookAt = Camera.main.transform;

            if (cameraToLookAt == null)
            {
                 Debug.LogError("PlayerController found, but its PlayerCamera is not assigned or accessible!", playerController);
                 enabled = false;
            }
        }
        else
        {
            Debug.LogError("WorldSpaceLookAtPlayer: Could not find an active PlayerController in the scene!", this);
            enabled = false;
        }
    }

    void LateUpdate()
    {
        if (cameraToLookAt != null && gameObject.activeInHierarchy)
            transform.LookAt(cameraToLookAt);
    }
}