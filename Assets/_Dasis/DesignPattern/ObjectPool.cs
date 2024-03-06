using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dasis.DesignPattern
{
    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly Action<T> pullObject;
        private readonly Action<T> pushObject;
        private readonly Stack<T> pooledObjects = new Stack<T>();
        private readonly List<T> releasedObjects = new List<T>();
        private readonly GameObject prefab;
        private readonly Transform pooledObjectParent;

        public int PooledCount => pooledObjects.Count;

        public ObjectPool(GameObject pooledObject, Transform parent)
        {
            prefab = pooledObject;
            pooledObjectParent = parent;
        }

        public ObjectPool(GameObject pooledObject, int numToSpawn = 0)
        {
            prefab = pooledObject;
            Spawn(numToSpawn);
        }

        public ObjectPool(GameObject pooledObject, Action<T> pullObject, Action<T> pushObject, int numToSpawn = 0)
        {
            prefab = pooledObject;
            this.pullObject = pullObject;
            this.pushObject = pushObject;
            Spawn(numToSpawn);
        }

        public T Pull()
        {
            T t;
            if (PooledCount > 0)
                t = pooledObjects.Pop();
            else
                t = UnityEngine.Object.Instantiate(prefab, pooledObjectParent).GetComponent<T>();

            t.gameObject.SetActive(true); //ensure the object is on
            t.Initial(Push);

            //allow default behavior and turning object back on
            pullObject?.Invoke(t);

            releasedObjects.Add(t);

            return t;
        }

        public T Pull(Transform parent)
        {
            T t = Pull();
            t.transform.SetParent(parent);
            return t;
        }

        public T Pull(Vector3 position)
        {
            T t = Pull();
            t.transform.localPosition = position;
            return t;
        }

        public GameObject PullGameObject()
        {
            return Pull().gameObject;
        }

        public GameObject PullGameObject(Vector3 position)
        {
            GameObject go = Pull().gameObject;
            go.transform.position = position;
            return go;
        }

        public GameObject PullGameObject(Vector3 position, Quaternion rotation)
        {
            GameObject go = Pull().gameObject;
            go.transform.SetPositionAndRotation(position, rotation);
            return go;
        }

        public void Push(T t)
        {
            pooledObjects.Push(t);
            releasedObjects.Remove(t);

            //create default behavior to turn off objects
            pushObject?.Invoke(t);

            t.gameObject.SetActive(false);
        }

        private void Spawn(int number)
        {
            T t;

            for (int i = 0; i < number; i++)
            {
                t = UnityEngine.Object.Instantiate(prefab).GetComponent<T>();
                pooledObjects.Push(t);
                t.gameObject.SetActive(false);
            }
        }

        public void PushAll()
        {
            while (releasedObjects.Count > 0)
            {
                releasedObjects[0].ReturnToPool();
            }
        }
    }

    public interface IPool<T>
    {
        T Pull();
        void Push(T t);
    }

    public interface IPoolable<T>
    {
        Action<T> ReturnAction { get; set; }
        void Initial(Action<T> returnAction);
        void ReturnToPool();
    }
}
