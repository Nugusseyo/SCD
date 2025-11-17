using System;
using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Work.PTY.Scripts.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        
        public Action OnAttack;

        private Grid _boardTileGrid;
        public bool IsAttacking { get; private set; }
        
        public static GameManager Instance;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            OnAttack += Attack;

            _boardTileGrid = BoardManager.Instance.boardTileGrid;
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                OnAttack?.Invoke();
            }
        }

        private void OnDestroy()
        {
            OnAttack -= Attack;
        }

        private void Attack()
        {
            if (IsAttacking)
            {
                Debug.Log("아직패는중임ㅋ");
                return;
            }
            IsAttacking = true;
            Debug.Log("가나디 복복복복복복");
            StartCoroutine(AttackSequence());
        }

        private IEnumerator AttackSequence()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector3Int slot = new Vector3Int(i, j, 0);
                    if (BoardManager.Instance.TileCompos[slot].OccupiePiece == null) continue;

                    Piece piece = BoardManager.Instance.TileCompos[slot].OccupiePiece.GetComponent<Piece>();
                    if (piece == null) continue;

                    piece.transform.DOScale(2f, 0.3f).SetEase(Ease.OutBack);

                    yield return new WaitForSeconds(1f);
                    
                    foreach (var moveVector in piece.pieceVectorList.VectorList)
                    {
                        Vector3Int enemyPos = piece.curCellPos + moveVector;
                        if (enemyPos.x < 0 || enemyPos.x >= 8 || enemyPos.y < 0 || enemyPos.y >= 8) continue;

                        GameObject occupiePiece = BoardManager.Instance.TileCompos[enemyPos].OccupiePiece;
                        if (occupiePiece == null) continue;

                        EnemyTest enemy = occupiePiece.GetComponent<EnemyTest>();
                        if (enemy != null)
                        {
                            Destroy(occupiePiece);

                            Vector3 enemyPosCenter = _boardTileGrid.GetCellCenterWorld(enemyPos);
                            Effect(enemyPosCenter, "AttackParticle");

                            impulseSource.GenerateImpulse();
                            
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    piece.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                }
            }
            IsAttacking = false;
        }


        private void Effect(Vector3 pos, string particleName)
        {
            var poolItem = PoolManager.Instance.PopByName(particleName); 
            if (poolItem == null)
            {
                Debug.LogError($"[Effect] Pool item not found: {particleName}");
                return;
            }

            var particle = poolItem as ParticlePool;
            if (particle != null)
            {
                particle.GameObject.transform.position = pos;
                particle.AppearanceItem();
            }
            else
            {
                var go = poolItem.GameObject;
                go.transform.position = pos;
                go.SetActive(true);

                var ps = go.GetComponent<ParticleSystem>();
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}