
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

        for (int i = 0; i < poolDefinitions.Length; i++)
        {
            var poolDefinition = poolDefinitions[i];
            
            poolDefinition.HashedName = Hash128.Compute(poolDefinition.prefab.name);
            pools[i] = new List<GameObject>(poolDefinition.maxCount);
            var pool = pools[i];
            for (int j = 0; j < poolDefinition.maxCount; j++)
            {
                GameObject newObject = Instantiate(poolDefinition.prefab, gameObject.transform);
                newObject.SetActive(false);
                pool.Add(newObject);
            }  
        }
    }

    public GameObject Get(string poolCategory)
    {
        //@NOTE: Returns an inactive gameobject. You have to initialize and activate it yourself.
        var poolIndex = GetPoolFromString(poolCategory);
        if (poolIndex < 0) return null;
        
        foreach (var pooledObject in pools[poolIndex])
        {
            for (int i = 0; i < poolDefinitions[poolIndex].maxCount; i++)
            {
                if (!pooledObject.activeInHierarchy)
                    return pooledObject;
            }
        }

        Debug.LogWarning($"{this.name} didn't have enough pool objects to return one!");
        return null;
    }

    private int GetPoolFromString(string poolName)
    {
        Hash128 hashedPoolName = Hash128.Compute(poolName);
        for (int i = 0; i < poolDefinitions.Length; i++)
        {
            if (hashedPoolName == poolDefinitions[i].HashedName) return i;
        }
        
        Debug.LogWarning($"Someone requested a pool \"{poolName}\", but no such pool exists.");
        return -1;
    }

    public void Return(GameObject returningObject)
    {
        returningObject.SetActive(false);
    }
}
