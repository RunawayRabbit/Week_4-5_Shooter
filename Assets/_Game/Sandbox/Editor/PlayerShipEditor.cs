
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerShip))]
public class PlayerShipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var ship = (PlayerShip) target;

        var weaponSlots = new List<GameObject>();
        
        for (var childIndex = 0;
            childIndex < ship.transform.childCount;
            childIndex++)
        {
            var child = ship.transform.GetChild(childIndex);
            if(child.TryGetComponent<WeaponSlot>(out _))
                weaponSlots.Add(child.gameObject);
        }

        ship.weaponSlots = weaponSlots.ToArray();
    }
}
