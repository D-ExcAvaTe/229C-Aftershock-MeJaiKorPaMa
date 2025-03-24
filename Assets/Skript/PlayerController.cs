using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeedMultiplier = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;

    [Space] [Header("Dart")]
    [SerializeField] private Dart dartPrefab;
    [SerializeField] private Transform handParent;
    [SerializeField] private Transform playerCamera;

    public float throwForce = 500;
    [SerializeField] private float minThrowForce = 10f, maxThrowForce = 1000f, scrollSensitivity = 100f;
    [HideInInspector] public Dart currentDart;
    
    
    private float runSpeed;
    private Vector3 velocity;

    void Update()
    {
        DartHandle();
        MovementHandle();
        GravityAndJumpHandle();
    }

    void DartHandle()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        throwForce += scroll * scrollSensitivity;
        throwForce = Mathf.Clamp(throwForce, minThrowForce, maxThrowForce);

        if (Input.GetButtonDown("Fire1"))
        {
            if (currentDart == null)
            {
                ReloadDart();
                return;
            }

            Vector3 targetDirection = GetCameraAimDirection();

            currentDart.ThrowDart(targetDirection, throwForce);
            currentDart = null;
        }
    }
    Vector3 GetCameraAimDirection()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        Vector3 aimPoint;

        if (Physics.Raycast(ray, out hit, 100f))
            aimPoint = hit.point; 
        else
            aimPoint = ray.origin + ray.direction * 100f;
        Vector3 throwDirection = (aimPoint - handParent.position).normalized;

        return throwDirection;
    }
    
    void ReloadDart()
    {
        currentDart = Instantiate(dartPrefab, handParent);
    }
    void MovementHandle()
    {
        runSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeedMultiplier : 1f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * (moveSpeed * runSpeed) * Time.deltaTime);
    }

    void GravityAndJumpHandle()
    {
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;
            
            if (Input.GetButtonDown("Jump"))
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}