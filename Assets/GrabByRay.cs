using UnityEngine;

public class GrabByRay : MonoBehaviour
{
    [Header("왼손 레이 시작점")]
    public Transform LeftRayOrigin;

    [Header("Grab 설정")]
    public float maxDistance = 5f;
    public LayerMask GrabLayer;

    Rigidbody GrabbedRB;
    Collider ObjectCollider;
    Transform GrabAnchor; // 손에 붙이는 기준점

    void Update()
    {
        bool grabDown = OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger);
        bool grabUp = OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger);

        if (grabDown && GrabbedRB == null)
            TryGrab();

        if (grabUp && GrabbedRB != null)
            Release();

        if (GrabbedRB != null)
            UpdateGrab();
    }

    void TryGrab()
    {
        if (LeftRayOrigin == null) return;

        Ray ray = new Ray(LeftRayOrigin.position, LeftRayOrigin.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, GrabLayer))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb == null) return;

            GrabbedRB = rb;
            ObjectCollider = hit.collider;
            GrabbedRB.isKinematic = true;

            // 빈 앵커 오브젝트를 손에 생성해서 오프셋 유지
            GrabAnchor = new GameObject("GrabAnchor").transform;
            GrabAnchor.SetParent(LeftRayOrigin);
            GrabAnchor.position = GrabbedRB.position;
            GrabAnchor.rotation = GrabbedRB.rotation;
        }
    }

    void UpdateGrab()
    {
        GrabbedRB.MovePosition(GrabAnchor.position);
        GrabbedRB.MoveRotation(GrabAnchor.rotation);
    }

    void Release()
    {
        GrabbedRB.isKinematic = false;
        GrabbedRB.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        GrabbedRB.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);

        Destroy(GrabAnchor.gameObject);
        GrabbedRB = null;
        ObjectCollider = null;
        GrabAnchor = null;
    }

    void OnDrawGizmos()
    {
        if (LeftRayOrigin == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(LeftRayOrigin.position, LeftRayOrigin.forward * maxDistance);
    }
}