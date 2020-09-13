using UnityEngine;

public class WeaponPowerUp : MonoBehaviour
{
    public GameObject weaponPrefab = default;
    
    private void OnCollisionEnter(Collision other) => Destroy(gameObject);
}
