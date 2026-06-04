using UnityEngine;

public class PlayerExitDetector : MonoBehaviour
{
    public RoomAnomalyManager anomalyManager;

    private void OnTriggerEnter(Collider other)
    {
        // 들어온 오브젝트가 플레이어인지 확인합니다.
        // OVRCameraRig나 NetworkPlayer 오브젝트의 태그가 "Player"로 되어 있거나, 
        // 혹은 HasInputAuthority를 가진 내 캐릭터 레이어여야 합니다.
        if (other.CompareTag("Player") || other.GetComponentInChildren<OVRCameraRig>() != null)
        {
            if (anomalyManager != null)
            {
                anomalyManager.OnPlayerExitedRoom();
            }
        }
    }
}