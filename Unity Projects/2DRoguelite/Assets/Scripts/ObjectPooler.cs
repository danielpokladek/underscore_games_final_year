using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private GameObject poolItem;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // for (int i = 0; i < pool.size; i++)
            // {
            //     GameObject obj = Instantiate(pool.prefab);
                
            //     obj.SetActive(false);
            //     obj.transform.SetParent(projectileContainer);

            //     objectPool.Enqueue(obj);
            // }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject PoolItem(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Pool with tag: " + tag + " does not exist!");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            foreach (Pool pool in pools)
            {
                if (pool.tag == tag)
                {
                    poolItem = Instantiate(pool.prefab);
                }
            }
        }
        else
        {
            poolItem = poolDictionary[tag].Dequeue();
        }

        IPooledObject pooledObj = poolItem.GetComponent<IPooledObject>();

        poolItem.transform.position = position;
        poolItem.transform.rotation = rotation;

        if (pooledObj != null)
            pooledObj.OnObjectSpawn();

        poolDictionary[tag].Enqueue(poolItem);

        Debug.Log(poolItem);
        return poolItem;
    }

    public void AddItem(string tag, GameObject objectToAdd)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Pool with tag: " + tag + " does not exist!");
            return;
        }

        poolDictionary[tag].Enqueue(objectToAdd);
    }
}
