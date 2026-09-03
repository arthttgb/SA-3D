using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public float damageAmount;

    public float maxDistance = 100f;

    private Vector3 startPos;
    private Rigidbody rb;

    [Header("Visual")]
    public GameObject damagePopupPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
            Debug.LogError("Bala sem Rigidbody!");
    }

    void Start()
    {
        startPos = transform.position;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.collisionDetectionMode =
                CollisionDetectionMode.ContinuousDynamic;
        }

        // Ignora outras balas
        GameObject[] outrasBalas =
            GameObject.FindGameObjectsWithTag("Bullet");

        Collider meuCollider =
            GetComponent<Collider>();

        if (meuCollider != null)
        {
            foreach (GameObject outraBala in outrasBalas)
            {
                if (outraBala == gameObject)
                    continue;

                Collider outroCollider =
                    outraBala.GetComponent<Collider>();

                if (outroCollider != null)
                {
                    Physics.IgnoreCollision(
                        meuCollider,
                        outroCollider,
                        true
                    );
                }
            }
        }

        // Ignora Player
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Collider[] bulletCols =
                GetComponentsInChildren<Collider>();

            Collider[] playerCols =
                player.GetComponentsInChildren<Collider>();

            foreach (Collider bc in bulletCols)
            {
                foreach (Collider pc in playerCols)
                {
                    Physics.IgnoreCollision(
                        bc,
                        pc,
                        true
                    );
                }
            }
        }
    }

    void Update()
    {
        if (rb == null)
            return;

        if (rb.linearVelocity.magnitude > 0.1f)
        {
            transform.rotation =
                Quaternion.LookRotation(
                    rb.linearVelocity
                );
        }

        if (
            Vector3.Distance(
                startPos,
                transform.position
            ) >= maxDistance
        )
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (
            collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Bullet") ||
            collision.collider.isTrigger
        )
        {
            return;
        }

        float finalDamage = 0;

        Hitbox hitbox =
            collision.gameObject.GetComponent<Hitbox>();

        if (hitbox != null)
        {
            finalDamage =
                damageAmount *
                hitbox.multiplicadorDano;

            hitbox.AplicarDanoDiferenciado(
                damageAmount
            );
        }
        else if (
            collision.gameObject.TryGetComponent<Target>(
                out Target target
            )
        )
        {
            finalDamage = damageAmount;

            target.TakeDamage(
                damageAmount
            );
        }

        if (finalDamage > 0)
        {
            ShowDamage(
                finalDamage,
                collision.contacts[0].point
            );
        }

        Destroy(gameObject);
    }

    void ShowDamage(
        float amount,
        Vector3 position
    )
    {
        if (damagePopupPrefab == null)
            return;

        GameObject popup =
            Instantiate(
                damagePopupPrefab,
                position,
                Quaternion.identity
            );

        Color textColor =
            amount > damageAmount
            ? Color.yellow
            : Color.white;

        popup.GetComponent<DamagePopup>()
            .Setup(amount, textColor);
    }
}