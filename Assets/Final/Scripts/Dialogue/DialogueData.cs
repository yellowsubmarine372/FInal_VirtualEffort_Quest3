using System;
using UnityEngine;

namespace ReadyFriendsOne.Dialogue
{
    [Serializable]
    public class DialogueLine
    {
        [Tooltip("발화자 이름 (비워두면 NPC 현재 이름 사용)")]
        public string speakerOverride;

        [TextArea(2, 5)]
        public string text;

        [Tooltip("다음 대사까지 대기 시간(초). 0이면 버튼 누를 때까지 대기.")]
        public float autoAdvanceDelay = 0f;
    }

    /// <summary>
    /// 대사 시퀀스 데이터.
    /// Assets/_Project/ScriptableObjects/Dialogues/<씬폴더>/ 에 저장.
    /// 대사 ID는 파일명으로 관리 (예: scene02_greeting).
    /// Owner: 씬 담당자가 각자 SO 에셋 채움
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue_New",
                     menuName = "ReadyFriendsOne/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        [Tooltip("PlayDialogue()에 넘길 ID — 파일명과 동일하게 유지 권장")]
        public string dialogueId;

        public DialogueLine[] lines;
    }
}
