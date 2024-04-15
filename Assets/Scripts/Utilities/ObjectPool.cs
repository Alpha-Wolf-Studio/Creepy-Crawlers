using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class ObjectPool : MonoBehaviour
    {
        public GameObject prefab;
        public int poolSize = 10;

        private List<GameObject> pool;

        private void Start()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            pool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        public GameObject GetObjectFromPool()
        {
            foreach (GameObject obj in pool)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            GameObject newObj = Instantiate(prefab, transform.position, Quaternion.identity);
            pool.Add(newObj);
            return newObj;
        }

        public void ReturnObjectToPool(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}
