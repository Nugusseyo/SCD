using System;
using csiimnida.CSILib.SoundManager.RunTime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Work.PTY.Scripts;
using YGPacks.PoolManager;

namespace Assets.Work.DHN.Scripts.Event
{
    public class Meteo : DropEvent, IPoolable
    {
        [SerializeField] private float speed;
        public Vector3 targetPos;
        public Vector3 spawnPos;

        private MeteoRenderer _meteoRenderer;

        public bool Explosion { get; private set; } = false;

        public string Name => "Meteo";

        public GameObject GameObject => gameObject;



        private void Awake()
        {
            _meteoRenderer = GetComponentInChildren<MeteoRenderer>();
            if (_meteoRenderer == null )
            {
                Debug.Log("�� ����");
            }
        }

        private void FixedUpdate()
        {
            //Update =>? -- > 1�����Ӹ��� ����
            // FixedUpdate -> ������ �ð� ���� ����
            if (Vector3.Distance(transform.position, targetPos) >= 0.1f && !Explosion)
            {
                MoveMT(targetPos);
            }
        }

        private void MoveMT(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
            
            if (Vector3.Distance(transform.position, target) < 0.1f && !Explosion)
            {
                DestroyMt();
            }

        }
        private void DestroyMt()
        {
            Explosion = true;
            //������ ������ (���߿� ����)
            //Ǫ�� ���ֱ�
            SoundManager.Instance.PlaySound("Meteo");
            _meteoRenderer.ExplosionMeteo();
            Vector3 cellPoint = BoardManager.Instance.boardTileGrid.WorldToCell(targetPos);
            if (BoardManager.Instance.TileCompos.ContainsKey(cellPoint))
            {
                if (BoardManager.Instance.TileCompos[cellPoint].OccupiePiece != null &&
                    BoardManager.Instance.TileCompos[cellPoint].OccupiePiece
                        .TryGetComponent<IDamageable>(out IDamageable damageable) && BoardManager.Instance
                        .TileCompos[cellPoint]
                        .OccupiePiece.TryGetComponent<IAgentHealth>(out IAgentHealth agentHealth))
                {
                    damageable.TakeDamage(agentHealth.MaxHealth / 4, gameObject);
                }
            }

        }

        public void ResetMeteo()
        {
            PoolManager.Instance.Push(this);
            Explosion = false;
        }
        public void ResetItem()
        {
            
        }
        public void AppearanceItem()
        {
            
        }
    }
}