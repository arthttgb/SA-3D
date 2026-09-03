using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Movimento")]
    public float motorForce = 15f;
    public float maxSpeed = 20f;
    public float turnSpeed = 100f;
    public float brakeForce = 30f;

    private Rigidbody rb;

    private float moveInput;
    private float turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Inputs
        moveInput = Input.GetAxis("Vertical");   // W/S ou ↑/↓
        turnInput = Input.GetAxis("Horizontal"); // A/D ou ←/→
    }

    void FixedUpdate()
    {
        Move();
        Turn();
        Brake();
    }

    void Move()
    {
        // Impede velocidade infinita
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveInput * motorForce, ForceMode.Acceleration);
        }
    }

    void Turn()
    {
        // Só vira se estiver andando
        if (rb.linearVelocity.magnitude > 0.5f)
        {
            float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
            Quaternion rotation = Quaternion.Euler(0f, turn, 0f);

            rb.MoveRotation(rb.rotation * rotation);
        }
    }

    void Brake()
    {
        // Espaço para frear
        if (Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity = Vector3.Lerp(
                rb.linearVelocity,
                Vector3.zero,
                brakeForce * Time.fixedDeltaTime
            );
        }
    }
}