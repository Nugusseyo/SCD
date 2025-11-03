using UnityEngine;
using YGPacks.PoolManager;

namespace Assets.Work.DHN.Scripts.Event
{
    public class Meteo : MonoBehaviour, IPoolable
    {
        [SerializeField] private float speed;
        [SerializeField] private float spawnDistance;
        public Vector3 targetPos;
        public Vector3 spawnPos;

        public string Name => "Meteo";

        public GameObject GameObject => gameObject;

        private void FixedUpdate()
        {
            //Update =>? -- > 1프레임마다 실행
            // FixedUpdate -> 고정된 시간 마다 실행

            MoveMT(targetPos);
        }

        private void MoveMT(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);


            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                DestroyMt();
            }
        }
        private void DestroyMt()
        {
            PoolManager.Instance.Push(this);
        }


        public void ResetItem()
        {
            
        }
        public void AppearanceItem()
        {
            
        }
    }
}