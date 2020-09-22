using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private int enemyCount;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector3 offsetBetweenSpawns;
    [SerializeField] private GameObject path;
    [SerializeField] private bool shouldMoveWithArena = true;
    [SerializeField] private Vector3 spawnInLocation;

    private void OnTriggerEnter(Collider other)
    {
        for (var i = 0; i < enemyCount; i++) { }
    }
}