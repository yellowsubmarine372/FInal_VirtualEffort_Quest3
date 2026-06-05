using UnityEngine;

public class DoorZoneTrigger : MonoBehaviour
{
    [Header("연결할 자동문 오브젝트")]
    public AutomaticDoor doorController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInChildren<OVRCameraRig>() != null)
        {
            if (doorController != null)
            {
                doorController.OpenDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInChildren<OVRCameraRig>() != null)
        {
            if (doorController != null)
            {
                doorController.CloseDoor();
            }
        }
    }
}