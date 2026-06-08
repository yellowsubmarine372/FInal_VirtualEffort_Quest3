using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("대사 고유 ID")]
    public string dialogueID;

    [Header("선택 시 실행될 이벤트")]
    public UnityEvent OnInteract;

    // VR 레이포인터나 그랩 스크립트가 NPC를 클릭/트리거했을 때 이 함수를 호출하게 합니다.
    public void Interact()
    {
        Debug.Log($"interacting with {gameObject.name}. dialogue ID: {dialogueID}");

        // 대사창 띄우기
        if (!string.IsNullOrEmpty(dialogueID))
        {
            // DialogueSystem.Instance.StartDialogue(dialogueID);
        }

        // 다른 스크립트가 구동되도록 이벤트 전송
        OnInteract?.Invoke();
    }
}