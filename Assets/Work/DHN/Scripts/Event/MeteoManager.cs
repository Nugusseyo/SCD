
using System.Collections;
using System.Collections.Generic;
using csiimnida.CSILib.SoundManager.RunTime;
using UnityEngine;
using YGPacks.PoolManager;

namespace Assets.Work.DHN.Scripts.Event
{
    public class MeteoManager : MonoBehaviour, IEvent //TargertPos��������, �̰� public���� �ٲٰ� �Լ��� �츮���� �ϱ�
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

        [ContextMenu("���׿�")]
        public void StartEvent()
        {
            List<Vector3> targetPosList = new List<Vector3>(); //���׿��� ������ ������ �������� ����Ʈ�� ����
            _meteoList.Clear();
            for (int i = 0; i < meteoCount; i++)
            {
                IPoolable mt =  PoolManager.Instance.PopByName("Meteo");
                Meteo meteo = mt as Meteo; //IPoolable�� ���׿��� �̵���Ű�ų�, �׷� ����� �Ҽ� ���� ������
                                           //mt�� ���׿��� ���� ��ȯ�� �ؼ� �� ��ȯ�Ѱ��� meteo�� ��´�
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
                    meteo.targetPos = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(_tilePos); //���߰��� �߰��ؾ� �Ұ�
                    Debug.Log(meteo.targetPos);

                    dis = Random.Range(spawnMinDis, spawnMaxDis);

                    meteo.transform.position = meteo.targetPos + (_meteoPos * dis);
                    SoundManager.Instance.PlaySound("Meteo");
                    StartCoroutine(DelayTime());
                }
                //���׿��� �����Ե�
            }
        }

        private IEnumerator DelayTime()
        {
            yield return new WaitForSeconds(8f);
            IsEnd = true;
        }

        private Vector3Int RandomTilePos()
        {
            return new Vector3Int(Random.Range(0, 8), Random.Range(0, 8), 0);
        }
        //�̺�Ʈ�� ����ȴ�
        /*1. Meteo���� Pop Meteo -> List�� ��´�
         * 2. ���׿����� ���������� SpawnPos, TargetPos �Ҵ�
         * Ÿ�� ��ġ���� Vector(1,1)�� �������� float ���� �����ذ� position���� ���������.
         */
    }
}