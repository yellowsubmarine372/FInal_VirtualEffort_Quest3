using System.Collections;
using UnityEngine;

namespace ReadyFriendsOne.Core
{
    /// <summary>
    /// 형광등 깜빡임.
    /// Light 컴포넌트에 붙이거나, 별도 오브젝트에서 targetLight 슬롯에 연결.
    /// Scene 8 붕괴 연출 시 AccelerateFlicker() 호출하면 점점 빠르게 깜빡임.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private float minIntensity = 0f;
        [SerializeField] private float maxIntensity = 1f;
        [SerializeField] private float minInterval = 0.05f;
        [SerializeField] private float maxInterval = 0.3f;

        private Light _light;
        private float _baseMax;
        private float _baseMaxInterval;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _baseMax = maxInterval;
            _baseMaxInterval = maxInterval;
        }

        private void OnEnable()
        {
            StartCoroutine(Flicker());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _light.intensity = maxIntensity;
        }

        // Scene 8에서 호출 — 점점 빠르게
        public void AccelerateFlicker(float newMaxInterval)
        {
            maxInterval = newMaxInterval;
        }

        public void ResetFlicker()
        {
            maxInterval = _baseMaxInterval;
        }

        private IEnumerator Flicker()
        {
            while (true)
            {
                _light.intensity = Random.Range(minIntensity, maxIntensity);
                yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            }
        }
    }
}
