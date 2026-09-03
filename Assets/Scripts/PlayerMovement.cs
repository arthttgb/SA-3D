using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Referências")]
    public CharacterController controller;
    public Transform cameraHolder;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Slider barraStamina;

    [Header("Movimento")]
    public float walkSpeed = 6f;
    public float runSpeed = 9f;

    [HideInInspector]
    public float speed;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float drainRate = 15f;
    public float regenRate = 7f;

    [Header("Pulo")]
    public float gravity = -20f;
    public float jumpHeight = 2.5f;

    [Header("Ground Check")]
    public float groundDistance = 0.15f;

    [Header("Head Bob")]
    public float walkBobSpeed = 10f;
    public float runBobSpeed = 16f;
    public float bobAmount = 0.05f;

    private Vector3 velocity;
    private bool isGrounded;
    private float bobTimer;

    void Start()
    {
        speed = walkSpeed;

        if (barraStamina != null)
        {
            barraStamina.maxValue = maxStamina;
            barraStamina.value = currentStamina;
        }
    }

    void Update()
    {
        GroundCheck();
        Movement();
        Jump();
        ApplyGravity();
        HandleHeadBob();
        HandleFOV();
        UpdateStaminaUI();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        bool moving = move.magnitude > 0.1f;

        bool running =
            Input.GetKey(KeyCode.LeftControl) &&
            moving &&
            currentStamina > 0;

        if (running)
        {
            speed = runSpeed;
            currentStamina -= drainRate * Time.deltaTime;
        }
        else
        {
            speed = walkSpeed;
            currentStamina += regenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(
            currentStamina,
            0,
            maxStamina
        );

        controller.Move(
            move.normalized *
            speed *
            Time.deltaTime
        );
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y =
                Mathf.Sqrt(
                    jumpHeight *
                    -2f *
                    gravity
                );
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        controller.Move(
            velocity * Time.deltaTime
        );
    }

    void HandleHeadBob()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool moving =
            Mathf.Abs(x) > 0.1f ||
            Mathf.Abs(z) > 0.1f;

        if (moving && isGrounded)
        {
            float bobSpeed =
                speed > walkSpeed
                ? runBobSpeed
                : walkBobSpeed;

            bobTimer += Time.deltaTime * bobSpeed;

            float bob =
                Mathf.Sin(bobTimer) *
                bobAmount;

            cameraHolder.localPosition =
                new Vector3(
                    0,
                    1.6f + bob,
                    0
                );
        }
        else
        {
            bobTimer = 0;

            cameraHolder.localPosition =
                Vector3.Lerp(
                    cameraHolder.localPosition,
                    new Vector3(0, 1.6f, 0),
                    5f * Time.deltaTime
                );
        }
    }

    void HandleFOV()
    {
        Camera cam = Camera.main;

        if (cam == null)
            return;

        float targetFOV = 65f;

        if (speed > walkSpeed)
            targetFOV = 75f;

        cam.fieldOfView =
            Mathf.Lerp(
                cam.fieldOfView,
                targetFOV,
                5f * Time.deltaTime
            );
    }

    void UpdateStaminaUI()
    {
        if (barraStamina != null)
        {
            barraStamina.value = currentStamina;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundDistance
        );
    }
}