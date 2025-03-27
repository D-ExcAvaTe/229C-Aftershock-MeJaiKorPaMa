using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public Slider sensitivitySlider; 
    public float mouseSensitivity = 1.0f;


    [SerializeField] private float mouseSens = 500f, minRotate = -90, maxRotate = 90;
    [SerializeField] private GameObject playerObject;
    private Camera playerCamera;

    [Header("Zoom")]
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float defaultFOV = 60f;
    [SerializeField] private float zoomSpeed = 10f;

    [Space]
    [Header("Items")]
    [SerializeField] private LayerMask itemLayerMask;

    [SerializeField] private float collectRange = 5f;
    private float xRotate = 0f;

    public ItemPrefab lastLookedItem = null;
    
    
    public static MouseLook instance;
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }
    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("currentSensitivity", 100);
        sensitivitySlider.value = mouseSensitivity / 10;
        

        playerCamera = Camera.main;
        
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera.fieldOfView = defaultFOV;
    }

    void Update()
    {
        LookHandle();
        ZoomHandle();
    }

    private void FixedUpdate()
    {
        LookItems();
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

    private void LookItems() // Renamed from LookItems for clarity
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, collectRange, itemLayerMask);

        ItemPrefab currentItem = null;

        if (didHit) currentItem = hit.collider.GetComponent<ItemPrefab>();

        if (currentItem != null && currentItem != lastLookedItem)
        {
            if (lastLookedItem != null) lastLookedItem.CanCollect(false);

            currentItem.CanCollect(true);

            lastLookedItem = currentItem;
        }
        else if (currentItem == null)
        {
            if (lastLookedItem != null)
            {
                lastLookedItem.CanCollect(false);
                lastLookedItem = null;
            }
        }

    }

    private void ZoomHandle()
    {
        if (Input.GetMouseButton(1))
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
        else
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV, zoomSpeed * Time.deltaTime);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity * 10;
    }
}