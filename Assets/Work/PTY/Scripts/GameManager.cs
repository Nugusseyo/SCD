using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Work.PTY.Scripts.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        
        public Action OnAttack;
        private bool _attackedEnemy = false;
        
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
            Debug.Log("가나디 복복복복복복");

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector3Int slot =  new Vector3Int(i, j, 0);
                    if (BoardManager.Instance.TileCompos[slot].OccupiePiece != null)
                    {
                        Piece piece = BoardManager.Instance.TileCompos[slot].OccupiePiece.GetComponent<Piece>();
                        if (piece != null)
                        {
                            foreach (var moveVector in piece.pieceVectorList.VectorList)
                            {
                                Vector3Int enemyPos = piece.curCellPos + moveVector;
                                if (enemyPos.x >= 0 && enemyPos.x < 8 && enemyPos.y >= 0 && enemyPos.y < 8)
                                {
                                    GameObject occupiePiece = BoardManager.Instance.TileCompos[enemyPos].OccupiePiece;
                                    if (occupiePiece != null)
                                    {
                                        if (occupiePiece.GetComponent<EnemyTest>() != null)
                                        {
                                            GameObject enemy = BoardManager.Instance.TileCompos[piece.curCellPos + moveVector].OccupiePiece;
                                            Destroy(enemy);
                                            if (!_attackedEnemy)
                                                _attackedEnemy = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (_attackedEnemy)
            {
                impulseSource.GenerateImpulse();
                _attackedEnemy = false;
            }
        }
    }
}