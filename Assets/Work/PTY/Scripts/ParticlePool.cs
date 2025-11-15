using System.Collections;
using UnityEngine;
using YGPacks.PoolManager;

namespace Work.PTY.Scripts
{
    public class ParticlePool : MonoBehaviour, IPoolable
    {
        public string Name => gameObject.name;
        public GameObject GameObject => gameObject;

        private ParticleSystem _ps;

        private void Awake()
        {
            _ps = GetComponent<ParticleSystem>();
            gameObject.SetActive(false);
        }

        public void AppearanceItem()
        {
            gameObject.SetActive(true);
            if (_ps != null)
            {
                _ps.Play();
                StartCoroutine(ReturnAfter(_ps));
            }
            else
            {

                StartCoroutine(ReturnAfter(1f));
            }
        }

        private IEnumerator ReturnAfter(ParticleSystem ps)
        {
            var main = ps.main;
            // startLifetime은 MinMaxCurve일 수 있으므로 최대값을 계산
            float lifetime = main.startLifetime.constant;
            if (main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants)
                lifetime = Mathf.Max(main.startLifetime.constantMin, main.startLifetime.constantMax);

            float duration = main.duration + lifetime;
            yield return new WaitForSeconds(duration);
            PoolManager.Instance.Push(this);
        }

        private IEnumerator ReturnAfter(float sec)
        {
            yield return new WaitForSeconds(sec);
            PoolManager.Instance.Push(this);
        }

        public void ResetItem()
        {
            if (_ps != null)
            {
                _ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            gameObject.SetActive(false);
        }
    }
}
