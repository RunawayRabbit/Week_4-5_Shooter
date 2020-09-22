using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab = default;
    private List<(float, PlayerWeaponSlot)> _hits = new List<(float, PlayerWeaponSlot)>();
    
    private Coroutine _resolveAtEndOfFrame;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Avatar") ||
            !other.gameObject.TryGetComponent<PlayerWeaponSlot>(out var weaponSlot))
            return;

        float hitDistance = Vector3.SqrMagnitude(other.transform.position - transform.position);

        _hits.Add((hitDistance, weaponSlot));

        if (_resolveAtEndOfFrame == null)
            _resolveAtEndOfFrame = StartCoroutine(ResolveHits());
    }

    private IEnumerator ResolveHits()
    {
        yield return new WaitForEndOfFrame();
        
        Debug.Assert(_hits.Count > 0, "Something went horribly wrong with WeaponPowerUp collision.");
        var closestSlot = _hits[0];
        for (int i = 1; i < _hits.Count; i++)
        {
            if (_hits[i].Item1 < closestSlot.Item1)
                closestSlot = _hits[i];
        }
        
        closestSlot.Item2.EquipWeapon(weaponPrefab);
        Destroy(gameObject);
    }
}
