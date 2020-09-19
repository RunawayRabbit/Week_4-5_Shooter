using UnityEngine;

public class WeaponPowerUp : MonoBehaviour
{
    public GameObject weaponPrefab = default;
    public bool active;
    private void OnCollisionEnter(Collision other) => Destroy(gameObject);
}
