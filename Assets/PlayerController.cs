using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float backSpeed = 2f;
    public float turnSpeed = 120f;
    public float gravity = -20f;

    CharacterController controller;
    Animator animator;
    float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Is the punch playing right now?
        bool isPunching = animator.GetCurrentAnimatorStateInfo(0).IsName("Punch");

        // Read the keyboard: W/S and A/D
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // Can't move or turn while punching
        if (isPunching)
        {
            v = 0;
            h = 0;
        }

        // Turn left/right
        transform.Rotate(0, h * turnSpeed * Time.deltaTime, 0);

        // Move forward (fast) or backward (slow)
        float speed = (v > 0) ? runSpeed : backSpeed;
        Vector3 move = transform.forward * v * speed;

        // Gravity
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        // Send info to the Animator
        animator.SetFloat("Move", v);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Punch");
        }
    }
}