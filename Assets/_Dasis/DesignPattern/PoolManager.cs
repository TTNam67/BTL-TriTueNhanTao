using Dasis.Enum;
using Dasis.Extensions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Dasis.DesignPattern
{
    [HideMonoScript]
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField]
        private List<PoolPrefab> prefabs;

        private readonly List<Pool> pools = new List<Pool>();

        [Button(SdfIconType.Recycle), GUIColor(0, 1, 0, 1)]
        public void Reorganize()
        {
            EnumGenerator.Generate("Entity", Stringify.ToStringList(prefabs));
        }

        public override void OnInitialization()
        {
            pools.Clear();
            for (int i = 0; i < System.Enum.GetValues(typeof(Entity)).Length; i++)
            {
                Transform parent = prefabs[i].parent;
                if (parent == null)
                    parent = transform;
                pools.Add(new Pool(prefabs[i].gameObject, parent));
            }
        }

        public GameObject Summon(Entity entity)
        {
            GameObject gameObject = pools[(int)entity].Summon();
            return gameObject;
        }

        public void Recall(Entity entity, GameObject gameObject)
        {
            pools[(int)entity].Recall(gameObject);
        }

        public void RecallAll(Entity entity)
        {
            pools[(int)entity].RecallAll();
        }
    }

    [System.Serializable]
    public class PoolPrefab
    {
        public GameObject gameObject;
        public Transform parent;
    }

    public class Pool
    {
        private readonly Stack<GameObject> _pooledObjects = new Stack<GameObject>();
        private readonly List<GameObject> _releasedObjects = new List<GameObject>();
        private readonly Transform _parent;
        private readonly GameObject _prefab;

        public int PooledCount => _pooledObjects.Count;

        public Pool(GameObject prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public GameObject Summon()
        {
            GameObject gameObject;

            if (PooledCount > 0)
            {
                gameObject = _pooledObjects.Pop();
                gameObject.transform.SetParent(_parent);
            }
            else
            {
                gameObject = Object.Instantiate(_prefab, _parent);
            }

            if (gameObject.TryGetComponent(out IPoolSummonHandler handler))
            {
                handler.OnSummon();
            }

            gameObject.SetActive(true);
            _releasedObjects.Add(gameObject);

            return gameObject;
        }

        public void Recall(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out IPoolRecallHandler handler))
            {
                handler.OnRecall();
            }

            _pooledObjects.Push(gameObject);
            _releasedObjects.Remove(gameObject);
            gameObject.SetActive(false);
            //gameObject.transform.SetParent(_parent);
        }

        public void RecallAll()
        {
            while (_releasedObjects.Count > 0)
            {
                Recall(_releasedObjects[0]);
            }
        }
    }

    public interface IPoolRecallHandler
    {
        public void OnRecall();
    }

    public interface IPoolSummonHandler
    {
        public void OnSummon();
    }
}
