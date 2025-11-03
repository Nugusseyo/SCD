
using System.Collections.Generic;
using UnityEngine;
using YGPacks.PoolManager;

namespace Assets.Work.DHN.Scripts.Event
{
    public class MeteoManager : MonoBehaviour, IEvent //TargertPos가져오고, 이걸 public으로 바꾸고 함수에 띠리따가 하기
    {
        List<Meteo> meteoList = new List<Meteo>();
        [SerializeField]private int _meteoCount;

        public bool IsEnd { get; set; }

        [ContextMenu("메테오")]
        public void StartEvent()
        {
            meteoList.Clear();
            for (int i = 0; i < _meteoCount; i++)
            {
                IPoolable mt =  PoolManager.Instance.PopByName("Meteo");
                Meteo meteo = mt as Meteo;
                meteoList.Add(meteo);
                meteo.gameObject.SetActive(true);
                //메테오를 가져왔따
            }
        }



        //이벤트가 실행된다
        /*1. Meteo에서 Pop Meteo -> List에 담는다
         * 2. 메테오에게 개별적으로 SpawnPos, TargetPos 할당
         * 
         */


    }
}