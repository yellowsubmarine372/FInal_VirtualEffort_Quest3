using System.Collections;
using TMPro;
using UnityEngine;

namespace ReadyFriendsOne.Interaction
{
    /// <summary>
    /// "복도가 비어있다", "연결되지 않습니다" 같은 짧은 텍스트 팝업.
    /// World Space Canvas에 붙여서 씀.
    /// SimpleInteractable.OnInteract → Show() 연결.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class TextPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private float displayDuration = 2.5f;

        private CanvasGroup _group;
        private Coroutine _routine;

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
            _group.alpha = 0f;
        }

        public void Show()
        {
            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(ShowRoutine());
        }

        public void Show(string text)
        {
            if (label != null) label.text = text;
            Show();
        }

        private IEnumerator ShowRoutine()
        {
            yield return StartCoroutine(Fade(0f, 1f));
            yield return new WaitForSeconds(displayDuration);
            yield return StartCoroutine(Fade(1f, 0f));
        }

        private IEnumerator Fade(float from, float to)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                _group.alpha = Mathf.Lerp(from, to, t / fadeDuration);
                yield return null;
            }
            _group.alpha = to;
        }
    }
}
