
using System.Collections.Generic;
using csiimnida.CSILib.SoundManager.RunTime;
using UnityEngine;
using YGPacks.PoolManager;

namespace Assets.Work.DHN.Scripts.Event
{
    public class MeteoManager : MonoBehaviour, IEvent //TargertPos가져오고, 이걸 public으로 바꾸고 함수에 띠리따가 하기
    {
        List<Meteo> _meteoList = new List<Meteo>();

        public CameraShaker CameraShaker { get; private set; }

        [SerializeField]private int meteoCount;

        private Vector3 _meteoPos = new Vector3(1, 1, 0);

        private Vector3Int _tilePos;

        private float dis;

        [SerializeField] private float spawnMinDis;
        [SerializeField] private float spawnMaxDis;

        public bool IsEnd { get; set; }

        private void Start()
        {
            EventManager.Instance.AddList(this);
        }

        [ContextMenu("메테오")]
        public void StartEvent()
        {
            List<Vector3> targetPosList = new List<Vector3>(); //메테오가 떨어질 지점의 포지션을 리스트로 만듬
            _meteoList.Clear();
            for (int i = 0; i < meteoCount; i++)
            {
                IPoolable mt =  PoolManager.Instance.PopByName("Meteo");
                Meteo meteo = mt as Meteo; //IPoolable은 메테오를 이동시키거나, 그런 기능을 할수 없기 때문에
                                           //mt를 메테오로 형식 변환을 해서 그 변환한것을 meteo에 담는다
                _meteoList.Add(meteo);
                _tilePos = RandomTilePos();
                if (BoardManager.Instance.TileCompos.TryGetValue(_tilePos, out Tile tile))
                {
                    bool isReset = false;
                    while(true)
                    {
                        _tilePos = RandomTilePos();
                        isReset = false;
                        foreach (Vector3 pos in targetPosList)
                        {
                            if(pos == _tilePos)
                            {
                                isReset = true;
                            }

                        }
                        if (!isReset)
                            break;
                    }
                    targetPosList.Add(_tilePos);
                    
                    tile = BoardManager.Instance.TileCompos[_tilePos];
                    meteo.targetPos = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(_tilePos); //나중가서 추가해야 할거
                    Debug.Log(meteo.targetPos);

                    dis = Random.Range(spawnMinDis, spawnMaxDis);

                    meteo.transform.position = meteo.targetPos + (_meteoPos * dis);
                    SoundManager.Instance.PlaySound("Meteo");
                }
                //메테오를 가져왔따
            }
        }
        private Vector3Int RandomTilePos()
        {
            return new Vector3Int(Random.Range(0, 8), Random.Range(0, 8), 0);
        }
        //이벤트가 실행된다
        /*1. Meteo에서 Pop Meteo -> List에 담는다
         * 2. 메테오에게 개별적으로 SpawnPos, TargetPos 할당
         * 타겟 위치에서 Vector(1,1)의 방향으로 float 값을 곱해준걸 position으로 적용시켜줌.
         */
    }
}