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
        public GameObject parent;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton

    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.parent.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string _tag, Vector3 _position, Quaternion _rotation)
    {
        if (!poolDictionary.ContainsKey(_tag))
        {
            Debug.LogWarning(_tag + "pool doesn't exist");
            return null;
        }
        
        GameObject objectToSpawn = poolDictionary[_tag].Dequeue();
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = _position;
        objectToSpawn.transform.rotation = _rotation;
        
        poolDictionary[_tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
