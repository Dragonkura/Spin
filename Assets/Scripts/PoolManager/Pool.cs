using UnityEngine;
using System;
using System.Collections.Generic;

//Pool module v 1.5.0

namespace Watermelon
{
    [Serializable]
    public class PoolInfo
    {
        public string name;
        public List<GameObject> objectsToPool;
        public int poolSize;
        public bool willGrow;
    }

    /// <summary>
    /// Basic pool class. Contains pool settings and references to pooled objects.
    /// </summary>
    [Serializable]
    public class Pool
    {
        /// <summary>
        /// Pool name, use it get pool reference at PoolManager.
        /// </summary>
        public string name;

        /// <summary>
        /// Reference to object which shood be pooled.
        /// </summary>
        public GameObject objectToPool;
        /// <summary>
        /// List of multiple pool prefabs.
        /// </summary>
        public List<GameObject> objectsToPoolList = new List<GameObject>();
        /// <summary>
        /// List of multiple pool weights.
        /// </summary>
        public List<int> weightsList = new List<int>();
        /// <summary>
        /// Number of objects which be created be deffault.
        /// </summary>
        public int poolSize = 10;
        /// <summary>
        /// True means: if there is no available object, the new one will be added to a pool.
        /// Otherwise will be returned null.
        /// </summary>
        public bool willGrow = true;
        /// <summary>
        /// Type of pool.
        /// Single - classic pool with one object. Multiple - pool with multiple objects returned randomly using weights.
        /// </summary>
        public PoolType poolType = PoolType.Single;
        /// <summary>
        /// Custom objects container for this pool's objects.
        /// </summary>
        public Transform objectsContainer;
        /// <summary>
        /// List of pooled objects.
        /// </summary>
        public List<GameObject> pooledObjects;
        /// <summary>
        /// List of pooled objects for multiple pull.
        /// </summary>
        public List<List<GameObject>> multiPooledObjects;

#if UNITY_EDITOR
        /// <summary>
        /// Number of objects that where active at one time.
        /// </summary>
        public int maxItemsUsedInOneTime = 0;
#endif

        public enum PoolType
        {
            Single = 0,
            Multiple = 1,
        }

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public Pool()
        {
            name = string.Empty;
        }

        public Pool(PoolInfo data)
        {
            name = data.name;
            poolSize = data.poolSize;
            willGrow = data.willGrow;
            poolType = (data.objectsToPool.Count > 1) ? PoolType.Multiple : PoolType.Single;

            if (data.objectsToPool.Count > 0)
            {
                if (poolType == PoolType.Multiple)
                {
                    objectsToPoolList.Clear();
                    objectsToPoolList.AddRange(data.objectsToPool);

                    weightsList.Clear();
                    int averagePoints = 100 / objectsToPoolList.Count;
                    int additionalPoints = 100 - averagePoints * objectsToPoolList.Count;

                    for (int j = 0; j < objectsToPoolList.Count; j++)
                    {
                        weightsList.Add(averagePoints + (additionalPoints > 0 ? 1 : 0));
                        additionalPoints--;
                    }
                }
                else
                {
                    objectToPool = data.objectsToPool[0];
                }
            }
        }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetPooledObject(bool activateObject = true)
        {
            return GetPooledObject(true, activateObject, false, Vector3.zero);
        }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="position">Sets object to specified position.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetPooledObject(Vector3 position, bool activateObject = true)
        {
            return GetPooledObject(true, activateObject, true, position);
        }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetHierarchyPooledObject(bool activateObject = true)
        {
            return GetPooledObject(false, activateObject, false, Vector3.zero);
        }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="position">Sets object to specified position.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetHierarchyPooledObject(Vector3 position, bool activateObject = true)
        {
            return GetPooledObject(false, activateObject, true, position);
        }

        /// <summary>
        /// Internal implementation of GetPooledObject and GetHierarchyPooledObject methods.
        /// </summary>
        /// <param name="checkTypeActiveSelf">Which type of checking object's activation state is used: active self or active in hierarchy.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private GameObject GetPooledObject(bool checkTypeActiveSelf, bool activateObject, bool setPosition, Vector3 position)
        {
            if (poolType == PoolType.Single)
            {
                return GetPooledObjectSingleType(checkTypeActiveSelf, activateObject, setPosition, position);
            }
            else
            {
                return GetPooledObjectMultiType(checkTypeActiveSelf, activateObject, setPosition, position);
            }
        }

        /// <summary>
        /// Internal implementation of GetPooledObject and GetHierarchyPooledObject methods for Single type pool.
        /// </summary>
        /// <param name="checkTypeActiveSelf">Which type of checking object's activation state is used: active self or active in hierarchy.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private GameObject GetPooledObjectSingleType(bool checkTypeActiveSelf, bool activateObject, bool setPosition, Vector3 position)
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (pooledObjects[i] != null && (checkTypeActiveSelf ? !pooledObjects[i].activeSelf : !pooledObjects[i].activeInHierarchy))
                {
                    if (setPosition)
                    {
                        pooledObjects[i].transform.position = position;
                    }
                    pooledObjects[i].SetActive(activateObject);

                    return pooledObjects[i];
                }
            }

            if (willGrow)
            {
                GameObject inst = PoolManager.AddObjectToPoolSingleType(this);
                if (setPosition)
                {
                    inst.transform.position = position;
                }
                inst.SetActive(activateObject);

#if UNITY_EDITOR
                Debug.LogWarning("[Pool Note] Out of Cache :: " + poolSize + " cache name :: " + name);
#endif
                return inst;
            }

            return null;
        }

        /// <summary>
        /// Internal implementation of GetPooledObject and GetHierarchyPooledObject methods for Multi type pool.
        /// </summary>
        /// <param name="checkTypeActiveSelf">Which type of checking object's activation state is used: active self or active in hierarchy.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private GameObject GetPooledObjectMultiType(bool checkTypeActiveSelf, bool activateObject, bool setPosition, Vector3 position)
        {
            int randomPoolIndex = 0;
            bool randomValueWasInRange = false;
            int randomValue = UnityEngine.Random.Range(0, 101);
            int currentValue = 0;

            for (int i = 0; i < weightsList.Count; i++)
            {
                currentValue += weightsList[i];

                if (randomValue <= currentValue)
                {
                    randomPoolIndex = i;
                    randomValueWasInRange = true;
                    break;
                }
            }

            if (!randomValueWasInRange)
            {
                Debug.LogError("[Pool Manager] Random value(" + randomValue + ") is out of weights sum range at pool: \"" + name + "\"");
            }

            Debug.Log("Random index: " + randomPoolIndex);

            List<GameObject> objectsList = multiPooledObjects[randomPoolIndex];

            for (int i = 0; i < objectsList.Count; i++)
            {
                if (checkTypeActiveSelf ? !objectsList[i].activeSelf : !objectsList[i].activeInHierarchy)
                {
                    if (setPosition)
                    {
                        objectsList[i].transform.position = position;
                    }
                    objectsList[i].SetActive(activateObject);

                    return objectsList[i];
                }
            }

            if (willGrow)
            {
                GameObject inst = PoolManager.AddObjectToPoolMultiType(this, randomPoolIndex);
                if (setPosition)
                {
                    inst.transform.position = position;
                }
                inst.SetActive(activateObject);

                return inst;
            }

            return null;
        }

        /// <summary>
        /// Disables all active objects from this pool.
        /// </summary>
        public void ReturnToPoolEverything()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if(pooledObjects[i] != null) pooledObjects[i].SetActive(false);
            }
        }
        public void ClearPool()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                GameObject.Destroy(pooledObjects[i]);
            }
            pooledObjects.Clear();
        }
        public List<GameObject> GetActivePooledObject()
        {
            List<GameObject> lst = new List<GameObject>();
            foreach (var obj in pooledObjects)
            {
                if (obj.activeSelf) lst.Add(obj);
            }
            return lst;
        }
    }
}