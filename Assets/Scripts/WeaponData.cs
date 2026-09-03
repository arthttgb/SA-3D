using UnityEngine;

[System.Serializable]
public class WeaponData
{
    [Header("Prefab equipado na mão")]
    public GameObject equippedPrefab;

    [Header("Prefab do pickup no chão")]
    public GameObject pickupPrefab;
}