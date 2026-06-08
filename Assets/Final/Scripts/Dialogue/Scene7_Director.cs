using UnityEngine;

namespace ReadyFriendsOne.Dialogue
{
    public class Scene7_Director : MonoBehaviour
    {
        public static Scene7_Director Instance { get; private set; }

        [Header("대사 데이터 에셋")]
        public DialogueData beforeExitDialogue; // Scene7_BeforeExit 지정
        public DialogueData afterReturnDialogue; // Scene7_AfterReturn 지정

        [Header("NPC 스크립트 참조")]
        public CompanionController companionNPC; // IDialogueTrigger 상속받은 NPC

        private bool _isFirstDialogueDone = false;
        private bool _isAnomalyDialogueTriggered = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // 씬에 접속하면 아주 잠깐의 유예(0.5초)를 두고 NPC가 첫 대사를 시작합니다.
            Invoke(nameof(StartFirstDialogue), 0.5f);
        }

        private void StartFirstDialogue()
        {
            if (beforeExitDialogue != null && companionNPC != null)
            {
                Debug.Log("[Scene7] 씬 접속 - NPC가 첫 대사를 시작합니다.");
                DialogueSystem.Instance.StartDialogue(beforeExitDialogue, companionNPC);
                _isFirstDialogueDone = true;
            }
        }

        /// <summary>
        /// 플레이어가 방을 나갔다가 다시 돌아와 문이 열릴 때 호출될 함수
        /// </summary>
        public void OnPlayerReturned()
        {
            // 첫 대사가 끝났고, 방이 붕괴되었으며, 아직 후속 대사가 안 나왔을 때만 실행
            if (_isFirstDialogueDone && !_isAnomalyDialogueTriggered)
            {
                _isAnomalyDialogueTriggered = true;
                Debug.Log("[Scene7] 플레이어 복귀 감지 - 뒤틀린 공간 대사 시작.");

                // 두 번째 대사 시퀀스 재생
                DialogueSystem.Instance.StartDialogue(afterReturnDialogue, companionNPC);
            }
        }
    }
}