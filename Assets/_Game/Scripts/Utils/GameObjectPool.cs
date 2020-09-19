using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Object Pool", menuName = "Scriptables/Object Pool", order = 0)]
public class GameObjectPool : ScriptableObject
{
    [SerializeField, HideInInspector] public Hash128 HashedName;
    [SerializeField] public GameObject prefab;
    [SerializeField] public int maxCount;
}