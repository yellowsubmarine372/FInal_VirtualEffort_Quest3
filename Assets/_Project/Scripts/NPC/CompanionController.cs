using UnityEngine;
using System;
using ReadyFriendsOne.Dialogue;

public class CompanionController : MonoBehaviour, IDialogueTrigger
{
    [Header("대사 데이터 파일 (드래그 앤 드롭)")]
    public DialogueData companionDialogueData;

    private Animator animator;
    public event Action OnDialogueEnd;

    void Start()
    {
        animator = GetComponent<Animator>();

        Invoke(nameof(AutoStartDialogue), 0.5f);
    }

    private void AutoStartDialogue()
    {
        PlayDialogue("");
    }

    public void PlayDialogue(string dialogueId)
    {
        if (animator != null) animator.SetTrigger("doTalk");

        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.StartDialogue(companionDialogueData, this);
        }
        else
        {
            Debug.LogError("[CompanionController] 씬에 DialogueSystem 오브젝트가 안 보입니다! 배치해 주세요.");
        }
    }

    public void FinishDialogue()
    {
        if (animator != null) animator.SetTrigger("stopTalk");
        if (OnDialogueEnd != null) OnDialogueEnd.Invoke();
    }
}