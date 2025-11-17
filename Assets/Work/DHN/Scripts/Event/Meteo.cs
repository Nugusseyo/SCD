using System;
using csiimnida.CSILib.SoundManager.RunTime;
using UnityEngine;
using UnityEngine.Events;
using YGPacks.PoolManager;

namespace Assets.Work.DHN.Scripts.Event
{
    public class Meteo : DropEvent, IPoolable
    {
        [SerializeField] private float speed;
        [SerializeField] private float spawnDistance;
        public Vector3 targetPos;
        public Vector3 spawnPos;

        private MeteoRenderer _meteoRenderer;

        private bool _explosion = false;

        public string Name => "Meteo";

        public GameObject GameObject => gameObject;

        
        private void Awake()
        {
            _meteoRenderer = GetComponentInChildren<MeteoRenderer>();
            if (_meteoRenderer == null )
            {
                Debug.Log("니 성공");
            }
        }
        public void WriteWord(char ch)
        {
            Debug.Log(ch);
        }

        private void FixedUpdate()
        {
            //Update =>? -- > 1프레임마다 실행
            // FixedUpdate -> 고정된 시간 마다 실행

            MoveMT(targetPos);
        }

        private void MoveMT(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, target) < 0.1f && !_explosion)
            {
                DestroyMt();
            }

        }
        private void DestroyMt()
        {
            _explosion = true;
            //데미지 입히기 (나중에 구현)
            //푸시 해주기
            SoundManager.Instance.PlaySound("Meteo");
            _meteoRenderer.ExplosionMeteo();

        }

        public void ResetMeteo()
        {
            PoolManager.Instance.Push(this);
            _explosion = false;
        }
        public void ResetItem()
        {
            
        }
        public void AppearanceItem()
        {
            
        }
    }
}