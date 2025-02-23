using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Queue<GameObject> pool = new Queue<GameObject>();
    private GameObject prefab;
    private Transform parent;

	public ObjectPool(GameObject prefab, int size, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        ExpandPool(size);
    }

    private void ExpandPool(int size)
    {
        for(int i = 0; i < size; i++)
        {
			GameObject newObject = Object.Instantiate(prefab, parent);
            newObject.SetActive(false);
			pool.Enqueue(newObject);
        }
    }

    public GameObject GetFromPool()
    {
        if(pool.Count == 0)
        {
            ExpandPool(1);
        }

		GameObject poolObject = pool.Dequeue();
        poolObject.SetActive(true);
        return poolObject;
    }

    public void ReturnToPool(GameObject poolObject)
    {
        poolObject.SetActive(false);
        pool.Enqueue(poolObject);
	}
}
