using System;
using UnityEngine;

namespace Work.JYG.DHNTestFile
{
    public class TestMeteor : MonoBehaviour //메테오 쪼가리 (운석)
    {
        [SerializeField] private float speed;
        public Vector3 targetPos;
        
        private void FixedUpdate()
        {
            MoveTarget(targetPos);
        }

        private void MoveTarget(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
            //만약 거리가 가까워졌으면 터트리기
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                DestroyMeteor();
            }
        }

        private void DestroyMeteor()
        {
            //파티클, 이미지 바뀜??, 애니메이션??
            Debug.Log("운석 삭제됨");
            gameObject.SetActive(false);
        }
    }
}
