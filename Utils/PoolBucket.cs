using System.Collections.Generic;
using UnityEngine;

namespace Helper.Pool
{
    public class PoolBucket<T> where T : Component
    {
        public enum PoolType { List, Queue }

        string itemName;
        T prefab;
        int amount;
        PoolType poolType;

        List<T> prefabList;
        Queue<T> prefabQueue;
        Transform parent;

        /// <summary>
        /// You cannot use gameobject type, use transform instead.
        /// PoolType.List will generate new prefab if existing ones are already in use.
        /// PoolType.Queue will return the first prefab even if it was in use.        
        /// </summary>
        /// <param name="_name">pool will keep the items in a parent named this name</param>
        /// <param name="_prefab">the item you want to reuse</param>
        /// <param name="_amount">initial amount to be generated</param>
        /// <param name="_poolType">how should this pool behave</param>
        public PoolBucket(string _name, T _prefab, int _amount, PoolType _poolType)
        {
            itemName = _name;
            prefab = _prefab;
            amount = _amount;
            poolType = _poolType;
            GeneratePool();
        }

        void GeneratePool()
        {
            parent = new GameObject(itemName).transform;

            if (poolType == PoolType.List) prefabList = new List<T>(amount);
            else prefabQueue = new Queue<T>(amount);

            for (int i = 0; i < amount; i++)
            {
                T g = Object.Instantiate(prefab);
                g.gameObject.SetActive(false);
                g.transform.SetParent(parent);

                if (poolType == PoolType.List) prefabList.Add(g);
                else prefabQueue.Enqueue(g);
            }
        }

        /// <summary>
        /// Returns the item
        /// </summary>
        /// <returns></returns>
        public T GetItem()
        {
            if (poolType == PoolType.Queue)
            {
                return prefabQueue.Dequeue();
            }
            else
            {
                foreach (T item in prefabList)
                {
                    if (!item.gameObject.activeInHierarchy)
                    {
                        item.gameObject.SetActive(true);
                        return item;
                    }
                }
                T g = MonoBehaviour.Instantiate(prefab);
                g.transform.SetParent(parent);
                prefabList.Add(g);
                return g;
            }
        }

        /// <summary>
        /// Returns the item and sets it to a target position
        /// </summary>
        /// <param name="position">target position</param>
        /// <returns></returns>
        public T GetItem(Vector3 position)
        {
            T item = GetItem();
            item.transform.position = position;
            return item;
        }

        /// <summary>
        /// Returns the item and sets it to a target position
        /// </summary>
        /// <param name="position">Target position</param>
        /// <param name="isActive">Should the returned gameobject be active?</param>
        /// <returns></returns>
        public T GetItem(Vector3 position, bool isActive)
        {
            T item = GetItem();
            item.transform.position = position;
            item.gameObject.SetActive(isActive);
            return item;
        }

        /// <summary>
        /// Returns the item
        /// </summary>
        /// <param name="isActive">Should the returned gameobject be active?</param>
        /// <returns></returns>
        public T GetItem(bool isActive)
        {
            T item = GetItem();            
            item.gameObject.SetActive(isActive);
            return item;
        }
    }
}


#region Demo
/*
public class Demo : MonoBehaviour
{
    //the prefab that will be reused
    [SerializeField] ParticleSystem bulletPrefab;

    //declare the pool
    Helper.Pool.PoolBucket<ParticleSystem> bulletPool;

    private void Start()
    {
        //setup the pool
        bulletPool = new Helper.Pool.PoolBucket<ParticleSystem>("bullet", bulletPrefab, 10, Helper.Pool.PoolBucket<ParticleSystem>.PoolType.Queue);
    }

    void GetBullet()
    {
        //usual way [not recomended]
        ParticleSystem b = Instantiate(bulletPrefab);

        //optimised way
        ParticleSystem _b = bulletPool.GetItem(); 
        ParticleSystem _b1 = bulletPool.GetItem(true);
        ParticleSystem _b2 = bulletPool.GetItem(Vector3.zero);
        ParticleSystem _b3 = bulletPool.GetItem(Vector3.zero, true);        
    }

    void DestoryBullet(GameObject bullet)
    {
        //usual way [not recomended]
        Destroy(bullet);

        //optimised way
        bullet.SetActive(false);
    }
}
*/
#endregion

