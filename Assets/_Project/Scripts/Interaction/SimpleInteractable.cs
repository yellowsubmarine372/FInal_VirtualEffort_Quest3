using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ReadyFriendsOne.Interaction
{
    /// <summary>
    /// 컨트롤러 레이로 가리키고 트리거 당기면 OnInteract 발동.
    /// Inspector에서 OnInteract에 원하는 함수 연결해서 씀.
    /// OVR Ray Interactor의 EventSystem 이벤트를 받음.
    /// </summary>
    public class SimpleInteractable : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UnityEvent onInteract;

        [Tooltip("한 번만 반응하고 비활성화 (헤드셋 등 일회성 트리거용)")]
        [SerializeField] private bool oneShot = false;

        private bool _triggered = false;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (oneShot && _triggered) return;
            _triggered = true;
            onInteract?.Invoke();
            if (oneShot) enabled = false;
        }

        // 코드에서 직접 트리거할 때 사용
        public void Trigger()
        {
            if (oneShot && _triggered) return;
            _triggered = true;
            onInteract?.Invoke();
            if (oneShot) enabled = false;
        }
    }
}
