using System.Collections;
using TMPro;
using UnityEngine;

namespace ReadyFriendsOne.Core
{
    /// <summary>
    /// World Space Canvas 말풍선 1개.
    /// 씬 시작 시 자동 표시하려면 autoPlayOnStart = true + lines 채우기.
    /// 코드에서 직접 쓰려면 Show() / Hide() 호출.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class ThoughtBubble : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI label;

        [Header("Auto Play")]
        [SerializeField] private bool autoPlayOnStart = false;
        [SerializeField] private float autoPlayDelay = 1f;

        [Header("Fade")]
        [SerializeField] private float fadeDuration = 0.5f;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            gameObject.SetActive(true);
        }

        private void Start()
        {
            if (autoPlayOnStart)
                StartCoroutine(DelayedShow(autoPlayDelay));
        }

        public void Show(string text)
        {
            StopAllCoroutines();
            if (label != null) label.text = text;
            StartCoroutine(FadeTo(1f));
        }

        public void Hide()
        {
            StopAllCoroutines();
            StartCoroutine(FadeTo(0f));
        }

        public void SetText(string text)
        {
            if (label != null) label.text = text;
        }

        private IEnumerator DelayedShow(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(FadeTo(1f));
        }

        private IEnumerator FadeTo(float target)
        {
            float start = _canvasGroup.alpha;
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(start, target, t / fadeDuration);
                yield return null;
            }
            _canvasGroup.alpha = target;
        }
    }
}
