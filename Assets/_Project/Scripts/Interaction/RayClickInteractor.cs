using UnityEngine;

namespace ReadyFriendsOne.Interaction
{
    /// <summary>
    /// 컨트롤러 PointerPose에서 직접 레이캐스트를 쏴서, 맞은 오브젝트의
    /// SimpleInteractable.Trigger()를 호출. Meta Interaction SDK의 RayInteractable
    /// 설정 없이도 SimpleInteractable(IPointerClickHandler)을 동작시키는 단순·확실한 방식.
    ///
    /// XRPlayerRig 루트에 붙이고, rayOrigin 슬롯에 각 컨트롤러의 ControllerPointerPose를 연결.
    /// Owner: 박세은
    /// </summary>
    public class RayClickInteractor : MonoBehaviour
    {
        [Header("레이 시작점 (ControllerPointerPose Transform)")]
        [SerializeField] private Transform rightRayOrigin;
        [SerializeField] private Transform leftRayOrigin;

        [Header("설정")]
        [SerializeField] private float maxDistance = 10f;
        [SerializeField] private LayerMask hitMask = ~0; // 기본: 전부

        [Tooltip("디버그: 맞춘 오브젝트를 Console에 출력")]
        [SerializeField] private bool debugLog = true;

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                TryInteract(rightRayOrigin, "Right");

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
                TryInteract(leftRayOrigin, "Left");
        }

        private void TryInteract(Transform origin, string side)
        {
            if (origin == null) return;

            if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, maxDistance, hitMask))
            {
                if (debugLog)
                    Debug.Log($"[RayClick:{side}] hit: {hit.collider.name}");

                var interactable = hit.collider.GetComponentInParent<SimpleInteractable>();
                if (interactable != null)
                    interactable.Trigger();
            }
            else if (debugLog)
            {
                Debug.Log($"[RayClick:{side}] 트리거 눌렀지만 아무것도 안 맞음");
            }
        }
    }
}
