using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private int enemyCount = 1;
    [SerializeField] private GameObject enemyPrefab = default;
    [SerializeField] private Vector3 offsetBetweenSpawns = Vector3.zero;
    [SerializeField] private GameObject path = default;
    [SerializeField] private Vector2 setArenaBounds = Vector2.zero;
    [SerializeField] private Arena.Mode setArenaMode = Arena.Mode.Horizontal;
    [SerializeField] private bool shouldMoveWithArena = true;
    [SerializeField] private Vector3 spawnInLocation = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        var spawnRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        var parent = shouldMoveWithArena ? Arena.Instance.transform : null;
        var offsetAccumulator = Vector3.zero;
        for (var i = 0; i < enemyCount; i++)
        {
            var newEnemy = Instantiate(enemyPrefab, transform.position + spawnInLocation + offsetAccumulator,
                spawnRotation, parent);
            var pathFollower = newEnemy.GetComponent<SplineFollower>();
            pathFollower.SetPathFromObject(path);

            offsetAccumulator += offsetBetweenSpawns;
        }

        Arena.Instance.SetMode(setArenaMode);
        if (setArenaBounds.x > 0.0f)
            Arena.Instance.horizontalArena.x = setArenaBounds.x;
        if (setArenaBounds.y > 0.0f)
            Arena.Instance.horizontalArena.y = setArenaBounds.y;
    }
}