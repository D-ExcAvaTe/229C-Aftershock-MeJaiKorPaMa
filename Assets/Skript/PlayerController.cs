using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed,runSpeedMultiplier=2f;
    private float runSpeed;
    
    void Update()
    {
        MovementHandle();
    }

    void MovementHandle()
    {
        if (Input.GetKey(KeyCode.LeftShift)) runSpeed = runSpeedMultiplier;
        else runSpeed = 1f;
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * (moveSpeed * runSpeed) * Time.deltaTime);
    }

}
