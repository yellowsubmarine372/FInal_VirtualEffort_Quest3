using UnityEngine;

namespace ReadyFriendsOne.Interaction
{
    /// <summary>
    /// 컨트롤러 PointerPose에서 직접 레이캐스트를 쏴서, 맞은 오브젝트의
    /// SimpleInteractable.Trigger()를 호출. (Meta Interaction SDK RayInteractable 설정 불필요)
    /// XRPlayerRig 루트에 붙이고 rayOrigin에 각 컨트롤러의 ControllerPointerPose 연결.
    /// Owner: 박세은
    /// </summary>
    public class RayClickInteractor : MonoBehaviour
    {
        [Header("레이 시작점 (ControllerPointerPose Transform)")]
        [SerializeField] private Transform rightRayOrigin;
        [SerializeField] private Transform leftRayOrigin;

        [Header("설정")]
        [SerializeField] private float maxDistance = 10f;
        [SerializeField] private LayerMask hitMask = ~0;
        [SerializeField] private bool debugLog = true;

        private void Start()
        {
            if (debugLog)
                Debug.Log($"[RayClick] ACTIVE. rightOrigin={(rightRayOrigin ? rightRayOrigin.name : "NULL")}, leftOrigin={(leftRayOrigin ? leftRayOrigin.name : "NULL")}");
        }

        private void Update()
        {
            // 어느 컨트롤러든 검지 트리거 눌림 감지
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (debugLog) Debug.Log("[RayClick:Right] trigger DOWN");
                TryInteract(rightRayOrigin, "Right");
            }
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
            {
                if (debugLog) Debug.Log("[RayClick:Left] trigger DOWN");
                TryInteract(leftRayOrigin, "Left");
            }
        }

        private void TryInteract(Transform origin, string side)
        {
            if (origin == null)
            {
                if (debugLog) Debug.Log($"[RayClick:{side}] origin이 NULL — 슬롯 연결 안 됨");
                return;
            }

            if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, maxDistance, hitMask))
            {
                if (debugLog) Debug.Log($"[RayClick:{side}] HIT: {hit.collider.name}");

                var interactable = hit.collider.GetComponentInParent<SimpleInteractable>();
                if (interactable != null)
                {
                    if (debugLog) Debug.Log($"[RayClick:{side}] → SimpleInteractable.Trigger() 호출: {interactable.name}");
                    interactable.Trigger();
                }
                else if (debugLog)
                {
                    Debug.Log($"[RayClick:{side}] 맞췄지만 SimpleInteractable 없음: {hit.collider.name}");
                }
            }
            else if (debugLog)
            {
                Debug.Log($"[RayClick:{side}] 레이 발사했지만 아무것도 안 맞음");
            }
        }
    }
}
