using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSens = 500f, minRotate = -90, maxRotate = 90;
    [SerializeField] private GameObject playerObject;

    private float xRotate = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        LookHandle();
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
}
