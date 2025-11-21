using System;
using System.Collections;
using csiimnida.CSILib.SoundManager.RunTime;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using YGPacks;
using TouchPhase = UnityEngine.TouchPhase;
using Vector3 = UnityEngine.Vector3;

namespace Work.PTY.Scripts.PieceManager
{
    public class PieceManager : Singleton<PieceManager>
    {
        [SerializeField] private PieceListSO pieceList;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private Piece piece;
        public Vector3 dragOffset;
        
        public Piece placingPiece;
        
        public Action OnAttack;

        private Grid _boardTileGrid;
        public bool IsAttacking { get; private set; }
        
        public bool isPlacingPiece;

        private bool _isInput => EventManager.Instance.debugIsOk;

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
            
            if (Input.touchCount == 0) return;
            if (_isInput)
            {
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
            
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            OnAttack -= Attack;
        }

        private void FollowPiece(Vector3 worldPos)
        {
            if (isPlacingPiece)
                placingPiece.transform.position = worldPos + new Vector3(0, 0, 9) + dragOffset;
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
                            tile.ToggleSpriteRenderer(true);
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
                            tile.ToggleSpriteRenderer(false);
                    }
                }
            }
        }
        
        public void SpawnPiece(int index)
        {
            if (isPlacingPiece) return;

            int activedTilesCount = 0;
            for(int y = 0; y < 4; y++)
                for (int x = 0; x < 8; x++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    SpriteRenderer targetTileSpriteRenderer = BoardManager.Instance.TileCompos[tilePos].GetComponent<SpriteRenderer>();
                    if (targetTileSpriteRenderer != null)
                    {
                        if(!targetTileSpriteRenderer.enabled) activedTilesCount++;
                    }
                }
            
            Debug.Log(activedTilesCount);
            if (activedTilesCount <= 0)
            {
                Debug.Log("꽉차잇음");
                return;
            }
            
            Debug.Log("소환완료");
            placingPiece = PoolManager.Instance.PopByName("Piece").GameObject.GetComponent<Piece>();
            placingPiece.pieceData = pieceList.pieces[index];
            placingPiece.pieceVectorLists.Add(pieceList.vectorLists[index - 1]);
            placingPiece.CurrentHealth = placingPiece.GetFinalMaxHealth();
            placingPiece.SetData();
            placingPiece.transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutBack);
            placingPiece.OnHold(true);
            isPlacingPiece = true;
            
            SetHighlight();
        }

        private void PlacePiece(Vector3 worldPos)
        {
            Vector3Int dropTile = BoardManager.Instance.boardTileGrid.WorldToCell(worldPos);
            Vector3 cellCenter = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(dropTile);
            
            if (!BoardManager.Instance.TileCompos.ContainsKey(dropTile))
            {
                Debug.LogWarning($"보드 범위 밖 타일 접근 시도: {dropTile}");
                placingPiece.transform.position = new Vector3(0, 0, -1);
                return;
            }
            
            SpriteRenderer spriteRenderer = BoardManager.Instance.TileCompos[dropTile].GetComponent<SpriteRenderer>();
            if (spriteRenderer.enabled)
            {
                placingPiece.transform.position = cellCenter + new Vector3(0, 0, -1);
                placingPiece.curCellPos = dropTile;
                Debug.Log("이동 성공");
                placingPiece.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                placingPiece.OnHold(false);
                placingPiece.isSelected = false;
                isPlacingPiece = false;
                BoardManager.Instance.TileCompos[dropTile].SetOccupie(placingPiece.gameObject);
                
                placingPiece = null;
                
                ClearHighlight();
            }
            else
            {
                placingPiece.transform.position = new Vector3(0, 0, -1);
                Debug.LogWarning($"이동 실패: {dropTile}, 원위치 복귀");
            }
        }

        private void Attack()
        {
            if (isPlacingPiece) return;
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

                    piece.transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutBack);
                    piece.OnHold(true);
                    Effect(_boardTileGrid.GetCellCenterWorld(slot), "SetTargetParticle");
                    SoundManager.Instance.PlaySound("PieceChange");

                    yield return new WaitForSeconds(1f);

                    bool didSomething = false;

                    foreach (var pieceVectorList in piece.pieceVectorLists)
                    {
                        foreach (var moveVector in pieceVectorList.VectorList)
                        {
                            Vector3Int targetPos = piece.curCellPos + moveVector;

                            if (targetPos.x < 0 || targetPos.x >= 8 || targetPos.y < 0 || targetPos.y >= 8)
                                continue;

                            GameObject occupiePiece = BoardManager.Instance.TileCompos[targetPos].OccupiePiece;
                            if (occupiePiece == null) continue;

                            Enemy targetEnemy = occupiePiece.GetComponent<Enemy>();
                            Piece targetPiece = occupiePiece.GetComponent<Piece>();
                            if (targetEnemy != null)
                            {
                                if (piece.CurrentEnergy > 0)
                                {
                                    targetEnemy.TakeDamage(piece.GetFinalDamage(), piece.gameObject);

                                    Vector3 enemyPosCenter = _boardTileGrid.GetCellCenterWorld(targetPos);
                                    Effect(enemyPosCenter, "AttackParticle");

                                    impulseSource.GenerateImpulse();
                                    SoundManager.Instance.PlaySound("PieceAttack");

                                    didSomething = true;
                                }

                                yield return new WaitForSeconds(0.3f);
                            }
                            else if (targetPiece != null)
                            {
                                if (piece.CurrentEnergy > 0)
                                {
                                    foreach(var a in piece.Attributes)
                                        if (a.canHeal)
                                        {
                                            targetPiece.Heal(piece.pieceData.damage / 4, piece.gameObject);
                                            SoundManager.Instance.PlaySound("PieceH");
                                            didSomething = true;
                                        }
                                }
                                
                                yield return new WaitForSeconds(0.3f);
                            }
                        }

                        if (didSomething)
                        {
                            piece.ReduceEnergy(1);
                        }

                        piece.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                        piece.OnHold(false);
                    }
                    
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