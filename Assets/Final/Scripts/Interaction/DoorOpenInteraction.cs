using System.Collections;
using UnityEngine;

namespace ReadyFriendsOne.Interaction
{
    /// <summary>
    /// 클릭 시 문이 열렸다가 잠시 후 닫힘. (Scene 1: 문 열어도 빈 복도 연출)
    /// 문 오브젝트의 SimpleInteractable.OnInteract → OpenAndClose() 연결.
    /// ⚠️ door의 피벗(원점)이 경첩(문 가장자리)에 있어야 자연스럽게 열림.
    ///    피벗이 중앙이면, 빈 부모 오브젝트를 경첩 위치에 만들고 문을 그 자식으로 넣어 door에 부모를 지정.
    /// Owner: 박세은
    /// </summary>
    public class DoorOpenInteraction : MonoBehaviour
    {
        [Tooltip("회전시킬 문 Transform. 비우면 자기 자신.")]
        [SerializeField] private Transform door;

        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float openSpeed = 2f;   // 클수록 빠름
        [SerializeField] private float stayOpenSeconds = 1.5f;
        [SerializeField] private Vector3 hingeAxis = Vector3.up; // 보통 Y축

        private bool _busy;
        private Quaternion _closedRot;

        private void Awake()
        {
            if (door == null) door = transform;
            _closedRot = door.localRotation;
        }

        /// <summary>문 SimpleInteractable.OnInteract에 연결.</summary>
        public void OpenAndClose()
        {
            if (_busy) return;
            StartCoroutine(Routine());
        }

        private IEnumerator Routine()
        {
            _busy = true;
            Quaternion openRot = _closedRot * Quaternion.AngleAxis(openAngle, hingeAxis);

            yield return Rotate(_closedRot, openRot);
            yield return new WaitForSeconds(stayOpenSeconds);
            yield return Rotate(openRot, _closedRot);

            _busy = false;
        }

        private IEnumerator Rotate(Quaternion from, Quaternion to)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * openSpeed;
                door.localRotation = Quaternion.Slerp(from, to, t);
                yield return null;
            }
            door.localRotation = to;
        }
    }
}
