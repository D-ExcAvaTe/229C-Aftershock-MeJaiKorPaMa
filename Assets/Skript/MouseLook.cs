using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSens = 500f, minRotate = -90, maxRotate = 90;
    [SerializeField] private GameObject playerObject;
    private Camera playerCamera;

    [Header("Zoom")]
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float defaultFOV = 60f;
    [SerializeField] private float zoomSpeed = 10f;

    private float xRotate = 0f;

    void Start()
    {
        playerCamera = Camera.main;
        
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera.fieldOfView = defaultFOV;
    }

    void Update()
    {
        LookHandle();
        ZoomHandle();
    }

    private void LookHandle()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotate -= mouseY;
        xRotate = Mathf.Clamp(xRotate, minRotate, maxRotate);

        transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);
        playerObject.transform.Rotate(Vector3.up * mouseX);
    }

    private void ZoomHandle()
    {
        if (Input.GetMouseButton(1))
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
        else
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV, zoomSpeed * Time.deltaTime);
    }
}