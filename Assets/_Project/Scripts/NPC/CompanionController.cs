using UnityEngine;
using System;
using ReadyFriendsOne.Dialogue;

public class CompanionController : MonoBehaviour, IDialogueTrigger
{
    [Header("대사 데이터 파일 (드래그 앤 드롭)")]
    [Tooltip("대사 에셋 삽입")]
    public DialogueData companionDialogueData;

    private Animator animator;
    public event Action OnDialogueEnd;

    // 인터페이스 계약 이행
    public void PlayDialogue(string dialogueId)
    {
        if (animator != null) animator.SetTrigger("doTalk");

        // 내가 품고 있는 데이터 에셋 자체를 DialogueSystem 싱글톤에 그대로 토스!
        DialogueSystem.Instance.StartDialogue(companionDialogueData, this);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FinishDialogue()
    {
        if (animator != null) animator.SetTrigger("stopTalk");

        // 대사가 끝났음을 승희와 세은이에게 전파
        if (OnDialogueEnd != null)
        {
            OnDialogueEnd.Invoke();
        }
    }
}