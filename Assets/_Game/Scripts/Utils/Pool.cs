using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pool : MonoBehaviour
{
    public static Pool Instance;

    public List<GameObject> pooledObjects;
    public GameObject prefab;
    public int maxCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < maxCount; i++)
        {
            GameObject newObject = Instantiate(prefab, gameObject.transform);
            var bulletComponent = newObject.GetComponent<SimpleBullet>();
            bulletComponent.pool = this;
            newObject.SetActive(false);
            pooledObjects.Add(newObject);
        }
    }

    public GameObject Get()
    {
        foreach (var pooledObject in pooledObjects)
        {
            for (int i = 0; i < maxCount; i++)
            {
                if (!pooledObject.activeInHierarchy)
                    return pooledObject;
            }
        }

        // No object to give!
        Debug.LogWarning($"{this.name} didn't have enough pool objects to return one!");
        return null;
    }

    public void Return(GameObject returningObject)
    {
        returningObject.SetActive(false);
    }
}
