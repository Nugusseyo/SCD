using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YGPacks.PoolManager
{
    public class YgPool
    {
        private Stack<IYgPoolable> _pool = new Stack<IYgPoolable>();
        private string _name;
        private GameObject _gameObject;
        private IYgPoolable _iYgPoolable;
        private Transform _objectParent;

        public YgPool(IYgPoolable ygPoolable, GameObject prefab, int itemCount, Transform parent)
        {
            _gameObject = prefab;
            _iYgPoolable = ygPoolable;
            _name = ygPoolable.Name;
            _objectParent = parent;
            
            for (int i = 0; i < itemCount; i++)
            {
                AddItem(true);
            }
        }


        private IYgPoolable AddItem(bool setActiveFalse)
        {
            GameObject item = Object.Instantiate(_gameObject, _objectParent);
            item.name = _name;

            IYgPoolable poolable = item.GetComponent<IYgPoolable>();
            if (setActiveFalse)
            {
                item.SetActive(false);
                _pool.Push(poolable);
            }

            return poolable;
        }

        public IYgPoolable Pop()
        {
            IYgPoolable poolable = null;

            if (_pool.Count > 0)
            {
                poolable = _pool.Pop();
                poolable.GameObject.SetActive(true);
                return poolable;
            }
            
            return poolable = AddItem(false);
        }

        public void Push(IYgPoolable poolable)
        {
            poolable.GameObject.SetActive(false);
            _pool.Push(poolable);
        }
    }
}
