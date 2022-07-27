using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    public class PoolManager
    {
        public List<GameObject> fireballPool = new List<GameObject>();
        public int poolSize;

        public GameObject GetObj(List<GameObject> pool, GameObject instance)
        {
            if (pool.Contains(instance))
            {
                pool.Remove(instance);
                instance.SetActive(true);
                return instance;
            }
            else
            {
                return GameObject.Instantiate(instance);
            }
        }

        public void AddObj(List<GameObject> pool, GameObject instance)
        {
            if (pool.Count < poolSize)
            {
                instance.SetActive(false);
                pool.Add(instance);
            }
        }
    }
}
