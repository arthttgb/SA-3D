using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    [Header("Efeito de Recuo")]
    public CameraRecoil cameraRecoil;

    [Header("Referências")]
    public Camera fpsCam;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    [Header("Status da Arma")]
    public float weaponDamage = 10f;
    public float weaponBulletSpeed = 50f;
    public float fireCooldown = 0.8f;

    [Header("Efeitos")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    [Header("Shotgun")]
    public int shotgunPelletCount = 1;
    public float shotgunSpread = 0f;

    [Header("Áudio")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    [Header("Spawn")]
    public float spawnOffset = 0.4f;

    private float lastShotTime = -10f;

    void Awake()
    {
        if (fpsCam == null)
            fpsCam = Camera.main;
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (Mouse.current != null &&
            Mouse.current.leftButton.isPressed)
        {
            if (Time.time - lastShotTime >= fireCooldown)
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }
    }

    void Shoot()
    {
        if (fpsCam == null)
            return;

        if (cameraRecoil != null)
            cameraRecoil.TriggerRecoil();

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (audioSource != null && shootSound != null)
        {
            audioSource.pitch =
                Random.Range(0.95f, 1.05f);

            audioSource.PlayOneShot(shootSound);
        }

        Transform spawnT =
            bulletSpawnPoint != null
            ? bulletSpawnPoint
            : fpsCam.transform;

        Ray ray =
            fpsCam.ViewportPointToRay(
                new Vector3(0.5f, 0.5f, 0)
            );

        Vector3 targetPoint = ray.GetPoint(100f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
        }

        Vector3 baseDirection =
            (targetPoint - spawnT.position).normalized;

        Vector3 spawnPos =
            spawnT.position +
            baseDirection * spawnOffset;

        for (int i = 0; i < shotgunPelletCount; i++)
        {
            float spreadX =
                Random.Range(
                    -shotgunSpread,
                    shotgunSpread
                );

            float spreadY =
                Random.Range(
                    -shotgunSpread,
                    shotgunSpread
                );

            float spreadZ =
                Random.Range(
                    -shotgunSpread,
                    shotgunSpread
                );

            Vector3 bulletDirection =
                (
                    baseDirection +
                    new Vector3(
                        spreadX,
                        spreadY,
                        spreadZ
                    )
                ).normalized;

            GameObject bullet =
                Instantiate(
                    bulletPrefab,
                    spawnPos,
                    Quaternion.LookRotation(
                        bulletDirection
                    )
                );

            // Passa o dano da arma para a bala
            Bullet bulletScript =
                bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.damageAmount =
                    weaponDamage;
            }

            Rigidbody rb =
                bullet.GetComponent<Rigidbody>();

            if (rb == null)
                rb = bullet.AddComponent<Rigidbody>();

            rb.useGravity = false;

            rb.collisionDetectionMode =
                CollisionDetectionMode.ContinuousDynamic;

            rb.linearVelocity =
                bulletDirection *
                weaponBulletSpeed;

            // Ignora colisão com a arma e player
            Collider[] shooterColliders =
                GetComponentsInChildren<Collider>();

            Collider[] bulletColliders =
                bullet.GetComponentsInChildren<Collider>();

            foreach (Collider bc in bulletColliders)
            {
                foreach (Collider sc in shooterColliders)
                {
                    Physics.IgnoreCollision(
                        bc,
                        sc,
                        true
                    );
                }
            }
        }
    }
}