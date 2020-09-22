using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    [SerializeField] private GameObjectPool[] poolDefinitions = default;
    [SerializeField] private List<GameObject>[] pools;

    private void Awake()
    {
        Instance = this;
        pools = new List<GameObject>[poolDefinitions.Length];

        for (var i = 0; i < poolDefinitions.Length; i++)
        {
            var poolDefinition = poolDefinitions[i];

            poolDefinition.HashedName = Hash128.Compute(poolDefinition.prefab.name);
            pools[i] = new List<GameObject>(poolDefinition.maxCount);
            var pool = pools[i];
            for (var j = 0; j < poolDefinition.maxCount; j++)
            {
                var newObject = Instantiate(poolDefinition.prefab, gameObject.transform);
                newObject.SetActive(false);
                pool.Add(newObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    public GameObject Get(string poolCategory)
    {
        //@NOTE: Returns an inactive gameobject. You have to initialize and activate it yourself.
        var poolIndex = GetPoolFromString(poolCategory);
        if (poolIndex < 0) return null;

        foreach (var pooledObject in pools[poolIndex])
            for (var i = 0; i < poolDefinitions[poolIndex].maxCount; i++)
                if (!pooledObject.activeInHierarchy)
                    return pooledObject;

        Debug.LogWarning($"{name} didn't have enough pool objects to return one!");
        return null;
    }

    private int GetPoolFromString(string poolName)
    {
        var hashedPoolName = Hash128.Compute(poolName);
        for (var i = 0; i < poolDefinitions.Length; i++)
            if (hashedPoolName == poolDefinitions[i].HashedName)
                return i;

        Debug.LogWarning($"Someone requested a pool \"{poolName}\", but no such pool exists.");
        return -1;
    }

    public void Return(GameObject returningObject)
    {
        returningObject.SetActive(false);
    }
}