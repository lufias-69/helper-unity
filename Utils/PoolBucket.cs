using UnityEngine;

namespace Helper.Pool
{
    public class PoolBucket<T> where T : Component
    {
        readonly string itemName;
        readonly T prefab;
        readonly int amount;

        int index;
        T[] prefabList;
        Transform parent;

        /// <summary>
        /// You cannot use gameobject type, use transform instead.
        /// </summary>
        /// <param name="_name">pool will keep the items in a parent named this name</param>
        /// <param name="_prefab">the item you want to reuse</param>
        /// <param name="_amount">initial amount to be generated</param>
        public PoolBucket(string _name, T _prefab, int _amount)
        {
            itemName = _name;
            prefab = _prefab;
            amount = _amount;
            GeneratePool();
        }

        void GeneratePool()
        {
            parent = new GameObject(itemName).transform;

            prefabList = new T[amount];
            
            for (int i = 0; i < amount; i++)
            {
                T g = Object.Instantiate(prefab);
                g.gameObject.SetActive(false);
                g.transform.SetParent(parent);

                prefabList[i] = g;
            }
        }

        /// <summary>
        /// Returns the item
        /// </summary>
        /// <returns></returns>
        public T GetItem()
        {
            index = (index + 1) % prefabList.Length;
            prefabList[index].gameObject.SetActive(true);
            return prefabList[index];
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
        bulletPool = new Helper.Pool.PoolBucket<ParticleSystem>("bullet", bulletPrefab, 10);
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

}
*/
#endregion

