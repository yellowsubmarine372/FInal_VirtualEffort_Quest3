using UnityEngine;
using System.Collections.Generic;

public class RoomAnomalyManager : MonoBehaviour
{
    [System.Serializable]
    public struct AnomalyTarget
    {
        public GameObject targetObject;   // 이상 현상을 일으킬 가구 (소파, 컵 등)
        public Vector3 anomalyLocalPos;   // 공중에 뜰 목표 로컬 위치 (예: 원래 위치보다 Y를 1.5 올림)
        public Vector3 anomalyRotation;   // 기괴하게 돌아갈 회전 값 (예: X축으로 45도 회전)
    }

    [Header("이상 현상을 일으킬 가구 리스트")]
    public List<AnomalyTarget> anomalyFurnitureList;

    private bool _hasPlayerExited = false; // 플레이어가 방을 나갔었는지 체크하는 플래그

    /// <summary>
    /// 복도 트리거(`Exit_Trigger`)를 밟았을 때 호출될 함수
    /// </summary>
    public void OnPlayerExitedRoom()
    {
        if (_hasPlayerExited) return; // 이미 나갔다 왔다면 중복 실행 방지

        _hasPlayerExited = true;
        Debug.Log("[Scene7] 플레이어가 방을 나갔습니다. 가상 공간 붕괴 준비...");

        // 플레이어가 안 보는 사이에 방 안의 가구들을 공중에 띄우고 뒤틉니다.
        TriggerAnomaly();
    }

    private void TriggerAnomaly()
    {
        foreach (var furniture in anomalyFurnitureList)
        {
            if (furniture.targetObject == null) continue;

            // 1. 물리 엔진이 방해하지 못하도록 Is Kinematic을 켭니다.
            Rigidbody rb = furniture.targetObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false; // 중력 법칙 무시
            }

            // 2. 미리 설정해 둔 기괴한 공중 좌표와 회전값으로 강제 이동시킵니다.
            furniture.targetObject.transform.localPosition = furniture.anomalyLocalPos;
            furniture.targetObject.transform.localRotation = Quaternion.Euler(furniture.anomalyRotation);
        }

        Debug.Log("[Scene7] 가상 공간의 물리 법칙이 붕괴되었습니다.");
    }
}