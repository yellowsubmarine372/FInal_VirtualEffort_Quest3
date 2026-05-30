using System;
using System.Collections;
using UnityEngine;

namespace ReadyFriendsOne.Core
{
    [Serializable]
    public class BubbleEntry
    {
        [TextArea(1, 3)]
        public string text;
        [Tooltip("이 말풍선이 표시된 후 다음으로 넘어가기까지 대기 시간(초)")]
        public float displayDuration = 3f;
        [Tooltip("이전 말풍선 사라진 뒤 이 말풍선 등장까지 간격(초)")]
        public float delayBefore = 0.5f;
    }

    /// <summary>
    /// 말풍선을 순서대로 재생.
    /// Scene 9처럼 첫 번째 말풍선 후 시간차로 두 번째가 나와야 할 때 사용.
    /// ThoughtBubble 컴포넌트와 같은 오브젝트 또는 자식에 붙임.
    /// </summary>
    public class ThoughtBubbleSequencer : MonoBehaviour
    {
        [SerializeField] private ThoughtBubble bubble;
        [SerializeField] private BubbleEntry[] entries;
        [SerializeField] private bool playOnStart = false;

        public event Action OnSequenceEnd;

        private void Start()
        {
            if (playOnStart)
                StartSequence();
        }

        public void StartSequence()
        {
            StopAllCoroutines();
            StartCoroutine(PlaySequence());
        }

        private IEnumerator PlaySequence()
        {
            foreach (var entry in entries)
            {
                yield return new WaitForSeconds(entry.delayBefore);
                bubble.Show(entry.text);
                yield return new WaitForSeconds(entry.displayDuration);
                bubble.Hide();
            }

            // 마지막 페이드 아웃 기다림
            yield return new WaitForSeconds(0.6f);
            OnSequenceEnd?.Invoke();
        }
    }
}
