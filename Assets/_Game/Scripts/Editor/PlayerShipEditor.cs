﻿
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(PlayerShip))]
public class PlayerShipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var ship = (PlayerShip) target;

        var weaponSlots = new List<PlayerWeaponSlot>();
        
        for (var childIndex = 0;
            childIndex < ship.transform.childCount;
            childIndex++)
        {
            var child = ship.transform.GetChild(childIndex);
            if(child.TryGetComponent<PlayerWeaponSlot>(out var weaponSlot))
                weaponSlots.Add(weaponSlot);
        }

        ship.weaponSlots = weaponSlots.ToArray();
    }
}
