using UnityEngine;

namespace ReadyFriendsOne.Core
{
    /// <summary>
    /// 월드 스페이스 UI(말풍선/팝업)가 항상 플레이어 카메라 정면에 부드럽게 따라오고
    /// 카메라를 바라보게 함. VR에서 "고정 위치라 뒤돌면 안 보이는" 문제 해결.
    /// ThoughtBubble / TextPopup 오브젝트에 붙임.
    /// Owner: 박세은
    /// </summary>
    public class FollowInFrontOfCamera : MonoBehaviour
    {
        [Tooltip("비워두면 Camera.main 자동 사용")]
        [SerializeField] private Transform cameraOverride;

        [Header("배치")]
        [SerializeField] private float distance = 2f;
        [Tooltip("카메라 기준 좌우(x)/상하(y) 오프셋")]
        [SerializeField] private Vector2 offset = Vector2.zero;

        [Header("따라오기")]
        [Tooltip("클수록 빠르게 따라옴. 너무 크면 멀미 유발")]
        [SerializeField] private float followSpeed = 4f;
        [SerializeField] private bool faceCamera = true;

        private Transform _cam;

        private void OnEnable()
        {
            ResolveCamera();
            // 켜질 때 바로 정면으로 스냅 (팝업이 뜰 때 즉시 보이게)
            if (_cam != null)
                transform.position = TargetPosition();
        }

        private void LateUpdate()
        {
            if (_cam == null) { ResolveCamera(); if (_cam == null) return; }

            transform.position = Vector3.Lerp(transform.position, TargetPosition(), Time.deltaTime * followSpeed);

            if (faceCamera)
                transform.rotation = Quaternion.LookRotation(transform.position - _cam.position);
        }

        private Vector3 TargetPosition()
        {
            return _cam.position
                 + _cam.forward * distance
                 + _cam.right * offset.x
                 + _cam.up * offset.y;
        }

        private void ResolveCamera()
        {
            if (cameraOverride != null) { _cam = cameraOverride; return; }
            if (Camera.main != null) _cam = Camera.main.transform;
        }
    }
}
