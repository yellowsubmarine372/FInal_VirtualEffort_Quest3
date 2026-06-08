using System;
using System.Collections;
using UnityEngine;

namespace ReadyFriendsOne.Core
{
    /// <summary>
    /// Scene 9 마무리 연출: 문 인터랙션 → 화면 페이드 아웃 + 크레딧 표시.
    /// 문 오브젝트의 SimpleInteractable.OnInteract → PlayEnding() 으로 연결.
    /// fadeOverlay / creditsPanel 은 World Space Canvas의 CanvasGroup을 슬롯에 연결.
    /// Owner: 박세은
    /// </summary>
    public class EndingController : MonoBehaviour
    {
        [Header("페이드 오버레이 (검은 화면 CanvasGroup)")]
        [SerializeField] private CanvasGroup fadeOverlay;

        [Header("크레딧 패널 (CanvasGroup)")]
        [SerializeField] private CanvasGroup creditsPanel;

        [Header("타이밍")]
        [SerializeField] private float fadeDuration = 1.5f;
        [SerializeField] private float delayBeforeCredits = 1f;

        public event Action OnEndingComplete;

        private bool _played;

        /// <summary>문 SimpleInteractable.OnInteract에 연결.</summary>
        public void PlayEnding()
        {
            if (_played) return;
            _played = true;
            StopAllCoroutines();
            StartCoroutine(EndingRoutine());
        }

        private IEnumerator EndingRoutine()
        {
            // 1) 화면을 검게
            yield return Fade(fadeOverlay, 0f, 1f);

            // 2) 잠시 정적
            yield return new WaitForSeconds(delayBeforeCredits);

            // 3) 크레딧 페이드 인
            yield return Fade(creditsPanel, 0f, 1f);

            OnEndingComplete?.Invoke();
        }

        private IEnumerator Fade(CanvasGroup group, float from, float to)
        {
            if (group == null) yield break;
            group.gameObject.SetActive(true);
            group.alpha = from;

            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                group.alpha = Mathf.Lerp(from, to, t / fadeDuration);
                yield return null;
            }
            group.alpha = to;
        }
    }
}
