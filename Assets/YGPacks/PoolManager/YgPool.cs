using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YGPacks.PoolManager
{
    public class YgPool
    {
        private Stack<IPoolable> _pool = new Stack<IPoolable>();
        private string _name;
        private GameObject _gameObject;
        private IPoolable _iPoolable;
        private Transform _objectParent;

        public YgPool(IPoolable poolable, GameObject prefab, int itemCount, Transform parent)
        {
            _gameObject = prefab;
            _iPoolable = poolable;
            _name = poolable.Name;
            _objectParent = parent;
            
            for (int i = 0; i < itemCount; i++)
            {
                AddItem(true);
            }
        }


        private IPoolable AddItem(bool setActiveFalse)
        {
            GameObject item = Object.Instantiate(_gameObject, _objectParent);
            item.name = _name;

            IPoolable poolable = item.GetComponent<IPoolable>();
            if (setActiveFalse)
            {
                item.SetActive(false);
                _pool.Push(poolable);
            }

            return poolable;
        }

        public IPoolable Pop()
        {
            IPoolable poolable = null;

            if (_pool.Count > 0)
            {
                poolable = _pool.Pop();
                poolable.GameObject.SetActive(true);
                return poolable;
            }
            
            return poolable = AddItem(false);
        }

        public void Push(IPoolable poolable)
        {
            poolable.GameObject.SetActive(false);
            _pool.Push(poolable);
        }
    }
}
