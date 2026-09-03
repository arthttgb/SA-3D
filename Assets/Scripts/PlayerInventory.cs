using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Referências")]
    public Transform weaponHolder;

    [Header("Prefabs do chão")]
    public GameObject ak47PickupPrefab;
    public GameObject revolverPickupPrefab;
    public GameObject escopetaPickupPrefab;

    public void EquipWeapon(GameObject weaponPrefab)
    {
        // Se já existe arma equipada, dropa ela primeiro
        DropCurrentWeapon();

        // Equipa nova arma
        GameObject weapon = Instantiate(
            weaponPrefab,
            weaponHolder.position,
            weaponHolder.rotation,
            weaponHolder
        );

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        Debug.Log("Equipada: " + weapon.name);
    }

    void DropCurrentWeapon()
    {
        if (weaponHolder.childCount == 0)
            return;

        Transform currentWeapon = weaponHolder.GetChild(0);

        Vector3 dropPos =
            transform.position +
            transform.forward * 2f +
            Vector3.up * 1f;

        string weaponName =
            currentWeapon.name
            .Replace("(Clone)", "")
            .Trim()
            .ToLower();

        Debug.Log("Dropando arma: " + weaponName);

        GameObject pickupToSpawn = null;

        if (weaponName.Contains("ak47"))
        {
            pickupToSpawn = ak47PickupPrefab;
        }
        else if (weaponName.Contains("revolver"))
        {
            pickupToSpawn = revolverPickupPrefab;
        }
        else if (weaponName.Contains("escopeta"))
        {
            pickupToSpawn = escopetaPickupPrefab;
        }

        if (pickupToSpawn != null)
        {
            GameObject dropped =
                Instantiate(
                    pickupToSpawn,
                    dropPos,
                    Quaternion.identity
                );

            Rigidbody rb = dropped.GetComponent<Rigidbody>();

            if (rb == null)
                rb = dropped.AddComponent<Rigidbody>();

            rb.useGravity = true;
            rb.collisionDetectionMode =
                CollisionDetectionMode.Continuous;

            rb.AddForce(
                transform.forward * 4f +
                Vector3.up * 2f,
                ForceMode.Impulse
            );

            Debug.Log("Spawnado pickup: " + pickupToSpawn.name);
        }
        else
        {
            Debug.LogError(
                "Não encontrei prefab para: " + weaponName
            );
        }

        Destroy(currentWeapon.gameObject);
    }
}