using System;
using System.Collections;
using System.Numerics;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Work.JYG.Code.Chessboard.Pieces;
using YGPacks;
using Quaternion = UnityEngine.Quaternion;
using TouchPhase = UnityEngine.TouchPhase;
using Vector3 = UnityEngine.Vector3;

namespace Work.PTY.Scripts.GameManager
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private Piece piece;
        public PieceSO testPieceData;
        public ObjectVectorListSO testVectorList;
        public Vector3 dragOffset;
        
        public Piece _placingPiece;
        
        public Action OnAttack;

        private Grid _boardTileGrid;
        public bool IsAttacking { get; private set; }
        
        public bool isPlacingPiece = false;
        
        public static GameManager Instance;
        
        private void Awake()
        {
            base.Awake();
            
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

            if (Keyboard.current.vKey.wasPressedThisFrame)
            {
                SpawnPiece(testPieceData, testVectorList);
            }
            
            if (Input.touchCount == 0) return;

            Touch touch = Input.GetTouch(0);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
            
            if(isPlacingPiece)
                switch (touch.phase)
                {
                    case TouchPhase.Moved:
                        FollowPiece(worldPos);
                        break;

                    case TouchPhase.Ended:
                        PlacePiece(worldPos);
                        break;
                }
        }

        private void OnDestroy()
        {
            OnAttack -= Attack;
        }

        private void FollowPiece(Vector3 worldPos)
        {
            if (isPlacingPiece)
                _placingPiece.transform.position = worldPos + new Vector3(0, 0, 9) + dragOffset;
        }

        private void SetHighlight()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    
                    if (BoardManager.Instance.TileCompos.ContainsKey(tilePos))
                    {
                        var tile = BoardManager.Instance.TileCompos[tilePos];
                        if (tile.GetComponent<Tile>().OccupiePiece == null)
                            tile.ToggleSpriteRenderer();
                    }
                }
            }
        }
        
        private void ClearHighlight()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    
                    if (BoardManager.Instance.TileCompos.ContainsKey(tilePos))
                    {
                        var tile = BoardManager.Instance.TileCompos[tilePos];
                        if (tile.GetComponent<SpriteRenderer>().enabled)
                            tile.ToggleSpriteRenderer();
                    }
                }
            }
        }
        
        public void SpawnPiece(PieceSO pieceData, ObjectVectorListSO vectorList)
        {
            piece.pieceData = pieceData;
            piece.pieceVectorList = vectorList;
            _placingPiece = Instantiate(piece.gameObject, transform.position, Quaternion.identity).GetComponent<Piece>();
            isPlacingPiece = true;
            
            SetHighlight();
        }

        private void PlacePiece(Vector3 worldPos)
        {
            Vector3Int dropTile = BoardManager.Instance.boardTileGrid.WorldToCell(worldPos);
            Vector3 cellCenter = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(dropTile);
            bool moved = false;
            
            if (!BoardManager.Instance.TileCompos.ContainsKey(dropTile))
            {
                Debug.LogWarning($"보드 범위 밖 타일 접근 시도: {dropTile}");
                _placingPiece.transform.position = new Vector3(0, 0, -1);
                _placingPiece.transform.Find("Visual").DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                _placingPiece.isSelected = false;
                _placingPiece = null;
                ClearHighlight();
                return;
            }
            
            SpriteRenderer spriteRenderer = BoardManager.Instance.TileCompos[dropTile].GetComponent<SpriteRenderer>();
            if (spriteRenderer.enabled)
            {
                _placingPiece.transform.position = cellCenter + new Vector3(0, 0, -1);
                _placingPiece.curCellPos = dropTile;
                moved = true;
                Debug.Log("이동 성공");
                _placingPiece.transform.Find("Visual").DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                _placingPiece.isSelected = false;
                isPlacingPiece = false;
                
                BoardManager.Instance.TileCompos[dropTile].SetOccupie(_placingPiece.gameObject);
                
                _placingPiece = null;
                
                ClearHighlight();
            }
            else
            {
                _placingPiece.transform.position = new Vector3(0, 0, -1);
                Debug.LogWarning($"이동 실패: {dropTile}, 원위치 복귀");
            }
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