using UnityEngine;
using System.Collections;

public class FPSWalkerCS : MonoBehaviour
{
    private float speed = 6.0f;
    public float walk_speed;
    public float running_speed = 12.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    
    private Vector3 moveDirection = Vector3.zero;
    private bool grounded = false;

    void Start()
    {
        speed = walk_speed;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            // We are grounded, so recalculate movedirection directly from axes
            if (Input.GetButtonDown("Reload"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }

            if (Input.GetButton("Run"))
            {
                speed = running_speed;
            }
            else speed = walk_speed;

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        CharacterController controller = GetComponent<CharacterController>();
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.CollidedBelow) != 0;
    }

    
}