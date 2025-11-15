
using System.Collections.Generic;
using UnityEngine;
using YGPacks.PoolManager;

namespace Assets.Work.DHN.Scripts.Event
{
    public class MeteoManager : MonoBehaviour, IEvent //TargertPos가져오고, 이걸 public으로 바꾸고 함수에 띠리따가 하기
    {
        List<Meteo> _meteoList = new List<Meteo>();
        [SerializeField]private int meteoCount;

        private Vector3 _meteoPos = new Vector3(1, 1, 0);

        [SerializeField] private float dis;

        public bool IsEnd { get; set; }

        [ContextMenu("메테오")]
        public void StartEvent()
        {
            _meteoList.Clear();
            for (int i = 0; i < meteoCount; i++)
            {
                IPoolable mt =  PoolManager.Instance.PopByName("Meteo");
                Meteo meteo = mt as Meteo;
                _meteoList.Add(meteo);
                meteo.targetPos = Vector3.zero; //나중가서 추가해야 할거
                meteo.transform.position = meteo.targetPos + (_meteoPos * dis);
                meteo.gameObject.SetActive(true);
                //메테오를 가져왔따
            }
        }



        //이벤트가 실행된다
        /*1. Meteo에서 Pop Meteo -> List에 담는다
         * 2. 메테오에게 개별적으로 SpawnPos, TargetPos 할당
         * 타겟 위치에서 Vector(1,1)의 방향으로 float 값을 곱해준걸 position으로 적용시켜줌.
         */
    }
}