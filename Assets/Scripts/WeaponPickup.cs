using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Prefab da arma da mão")]
    public GameObject equippedWeaponPrefab;

    public string weaponName;

    private bool playerNearby;
    private bool foiColetada = false;

    private PlayerInventory inventory;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            inventory = other.GetComponent<PlayerInventory>();

            Debug.Log($"Pressione E para pegar {weaponName}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            inventory = null;
        }
    }

    void Update()
    {
        if (foiColetada)
            return;

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            foiColetada = true;

            Debug.Log("Pegando arma: " + weaponName);

            inventory.EquipWeapon(equippedWeaponPrefab);

            Debug.Log($"Você coletou: {weaponName}");

            Destroy(gameObject);
        }
    }
}