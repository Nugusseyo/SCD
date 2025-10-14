using UnityEngine;

namespace YGPacks
{
    public class YgSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        singleton.name = typeof(T).Name;
                        _instance = singleton.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            T[] managers = FindObjectsByType<T>(FindObjectsSortMode.None);

            if (managers.Length > 1)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
