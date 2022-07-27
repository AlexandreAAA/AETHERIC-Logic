using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EveController
{
    public class PlayerObjectPool : MonoBehaviour
    {
        public static PlayerObjectPool SharedInstance;

        public Pool fireball = new Pool();
        public Pool explosion = new Pool();

        #region Init
        private void Awake()
        {
            SharedInstance = this;
        }

        private void Start()
        {
            InitPool(fireball);
            InitPool(explosion);
        }
        #endregion

        #region Method
        private void InitPool(Pool p)
        {
            if (!p.isInit)
            {
                p.pooledObjects = new List<GameObject>();
                GameObject tmp;

                for (int i = 0; i < p.amountToPool; i++)
                {
                    tmp = Instantiate(p.objectToPool);
                    tmp.SetActive(false);
                    p.pooledObjects.Add(tmp);
                }

                p.isInit = true;
            }
        }

        public GameObject GetPooledObject(Pool p)
        {
            for (int i = 0; i < p.amountToPool; i++)
            {
                if (!p.pooledObjects[i].activeInHierarchy)
                {
                    return p.pooledObjects[i];
                }
            }
            return null;
        }
        #endregion
    }

    [Serializable]
    public class Pool
    {
        public bool isInit;
        public List<GameObject> pooledObjects;
        public GameObject objectToPool;
        public int amountToPool;
    }
}
