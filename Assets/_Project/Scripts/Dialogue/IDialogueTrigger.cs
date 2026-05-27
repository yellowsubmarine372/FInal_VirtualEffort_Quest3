using System;

namespace ReadyFriendsOne.Dialogue
{
    /// <summary>
    /// 대사 트리거 인터페이스.
    /// CompanionNPC, AmbientNPC 모두 이걸 구현함.
    /// 씬 작업자는 OnDialogueEnd를 구독해서 다음 씬 전환 처리.
    /// </summary>
    public interface IDialogueTrigger
    {
        /// <summary>대사 ID를 받아 해당 대사 시퀀스를 시작함.</summary>
        void PlayDialogue(string dialogueId);

        /// <summary>대사 시퀀스가 모두 끝났을 때 발생.</summary>
        event Action OnDialogueEnd;
    }
}
