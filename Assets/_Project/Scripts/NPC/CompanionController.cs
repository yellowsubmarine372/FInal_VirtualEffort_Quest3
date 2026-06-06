using UnityEngine;
using System;
using ReadyFriendsOne.Core;
using ReadyFriendsOne.Dialogue;
using UnityEngine.SceneManagement;

public class CompanionController : MonoBehaviour, IDialogueTrigger
{
    [Header("대사 데이터 (드래그 앤 드롭)")]
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
            Debug.LogError("[CompanionController] 씬에 DialogueSystem 컴포넌트가 없습니다! 확인해 주세요.");
        }
    }

    public void FinishDialogue()
    {
        if (animator != null) animator.SetTrigger("stopTalk");
        OnDialogueEnd?.Invoke();

        string scene = SceneManager.GetActiveScene().name;
        switch (scene)
        {
            case "03_MemoryMusic":
                ReadyFriendsOne.Core.SceneLoader.Load("04_MemoryMovie");
                break;
            case "04_MemoryMovie":
                ReadyFriendsOne.Core.SceneLoader.Load("05_MemorySports");
                break;
            case "05_MemorySports":
                GameState.Stage = StoryStage.Promise;
                ReadyFriendsOne.Core.SceneLoader.Load("06_Promise");
                break;
            case "06_Promise":
                GameState.Stage = StoryStage.GlitchSubtle;
                ReadyFriendsOne.Core.SceneLoader.Load("07_Breakdown");
                break;
        }
    }
}
