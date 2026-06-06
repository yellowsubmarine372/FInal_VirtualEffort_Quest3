using UnityEngine;

public class EX_OVRInput_Grab_Right : MonoBehaviour
{
    [Header("Hand")]
    public Transform RightHand;

    [Header("Grab")]
    public float grabRadius = 0.2f;
    public LayerMask GrabLayer;

    Rigidbody GrabbedRB;
    Collider PlayerCollider;
    Collider ObjectCollider;

    Vector3 PosOffset;
    Quaternion RotOffset;

    void Start()
    {
        PlayerCollider = GetComponent<CharacterController>().GetComponent<Collider>();
    }

    void Update()
    {
        bool grab = OVRInput.Get(OVRInput.RawButton.RHandTrigger); // 오른손 트리거

        if (grab)
        {
            if (GrabbedRB == null) TryGrab();
            else UpdateGrab();
        }
        else
        {
            if (GrabbedRB != null) Release();
        }
    }

    void TryGrab()
    {
        Collider[] hits = Physics.OverlapSphere(RightHand.position, grabRadius, GrabLayer);
        if (hits.Length == 0) return;

        float minDist = float.MaxValue;
        Rigidbody closest = null;

        foreach (Collider c in hits)
        {
            Rigidbody rb = c.attachedRigidbody;
            if (rb == null) continue;
            float d = Vector3.Distance(RightHand.position, rb.position);
            if (d < minDist) { minDist = d; closest = rb; }
        }

        if (closest == null) return;

        GrabbedRB = closest;
        ObjectCollider = GrabbedRB.GetComponent<Collider>();
        GrabbedRB.isKinematic = true;
        Physics.IgnoreCollision(ObjectCollider, PlayerCollider, true);
        PosOffset = Quaternion.Inverse(RightHand.rotation) * (GrabbedRB.position - RightHand.position);
        RotOffset = Quaternion.Inverse(RightHand.rotation) * GrabbedRB.rotation;
    }

    void UpdateGrab()
    {
        GrabbedRB.MovePosition(RightHand.position + RightHand.rotation * PosOffset);
        GrabbedRB.MoveRotation(RightHand.rotation * RotOffset);
    }

    void Release()
    {
        GrabbedRB.isKinematic = false;
        GrabbedRB.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        GrabbedRB.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
        Physics.IgnoreCollision(ObjectCollider, PlayerCollider, false);
        GrabbedRB = null;
        ObjectCollider = null;
    }

    void OnDrawGizmos()
    {
        if (RightHand == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(RightHand.position, grabRadius);
    }
}