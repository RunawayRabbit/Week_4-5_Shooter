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
        var spawnRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        var parent = shouldMoveWithArena ? Arena.Instance.transform : null;
        var offsetAccumulator = Vector3.zero;
        for (var i = 0; i < enemyCount; i++)
        {
            var newEnemy = Instantiate(enemyPrefab, spawnInLocation + offsetAccumulator,
                spawnRotation, parent);
            var pathFollower = newEnemy.GetComponent<SplineFollower>();
            pathFollower.SetPathFromObject(path);

            offsetAccumulator += offsetBetweenSpawns;
        }
    }
}