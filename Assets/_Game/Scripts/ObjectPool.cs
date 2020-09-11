using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    private List<GameObject> used;
    private List<GameObject> free;
    
    private int _sizeOfPool;

    private void Awake()
    {
        used = new List<GameObject>(_sizeOfPool);
        free = new List<GameObject>(_sizeOfPool);

        for (var i = 0; i < free.Count; i++)
        {
            var newObject = Instantiate(prefab);
            newObject.SetActive(false);
            free.Add(newObject);
        }
    }

    public GameObject Get()
    {
        if (used.Count == _sizeOfPool)
        {
            Debug.LogWarning($"{this} Object pool might be too small!");
            return null;
        }

        var indexToPull = free.Count - 1;
        var newObject = free[indexToPull];
        free.RemoveAt(indexToPull);
        used.Add(newObject);
        return newObject;
    }

    public void Return(GameObject returningObject)
    {
        Debug.Assert(used.Contains(returningObject), "We tried to return an object to the pool, but it isn't in the pool.");

        returningObject.SetActive(false);
        used.Remove(returningObject);
        free.Add(returningObject);
    }
}
