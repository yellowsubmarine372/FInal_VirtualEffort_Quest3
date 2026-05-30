using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace ReadyFriendsOne.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance { get; private set; }

        [Header("UI References")]
        public GameObject dialogueUIObject; // 대사창 전체 패널 (Canvas 하위 UI)
        public TextMeshProUGUI nameText;     // 발화자 이름 텍스트
        public TextMeshProUGUI dialogueText; // 대사 본문 텍스트

        private Queue<DialogueLine> _linesQueue; // 출력할 대사들을 순서대로 담는 큐
        private IDialogueTrigger _currentNPC;    // 현재 대사 중인 NPC 주체
        private Coroutine _autoAdvanceCoroutine; // autoAdvanceDelay 처리를 위한 코루틴 변수
        private bool _isWaitingForInput = false; // 플레이어 버튼 입력을 대기 중인지 여부

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _linesQueue = new Queue<DialogueLine>();

                // 게임 시작 시 대사창 UI는 숨김 처리
                if (dialogueUIObject != null) dialogueUIObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // 입력 대기 상태(_isWaitingForInput)일 때, VR 컨트롤러 A 버튼 또는 PC 스페이스바 입력 처리
            if (dialogueUIObject.activeSelf && _isWaitingForInput)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.Space))
                {
                    DisplayNextSentence();
                }
            }
        }

        /// <summary>
        /// 대사 시퀀스를 시작하는 메인 함수.
        /// 이제 문자열 경로 대신, 인스펙터에 꽂아둔 DialogueData 에셋을 통째로 넘겨받습니다.
        /// </summary>
        public void StartDialogue(DialogueData data, IDialogueTrigger npc)
        {
            if (data == null)
            {
                Debug.LogError("[DialogueSystem] 넘겨받은 DialogueData 에셋이 null입니다! 인스펙터를 확인하세요.");
                return;
            }

            _currentNPC = npc;

            // 기존에 돌고 있던 자동 넘김 코루틴이 있다면 안전하게 정지
            if (_autoAdvanceCoroutine != null) StopCoroutine(_autoAdvanceCoroutine);

            dialogueUIObject.SetActive(true);
            _linesQueue.Clear();

            // 대사 파일에 들어있는 lines 배열을 큐에 차례대로 채워넣음
            foreach (DialogueLine line in data.lines)
            {
                _linesQueue.Enqueue(line);
            }

            DisplayNextSentence();
        }

        /// <summary>
        /// 다음 대사 한 줄을 꺼내와 UI에 출력하는 함수
        /// </summary>
        private void DisplayNextSentence()
        {
            // 더 이상 보여줄 대사가 없다면 시퀀스 종료
            if (_linesQueue.Count == 0)
            {
                EndDialogue();
                return;
            }

            // 새로운 대사를 틀기 전 기존 자동 진행 예약 코루틴은 취소
            if (_autoAdvanceCoroutine != null) StopCoroutine(_autoAdvanceCoroutine);

            _isWaitingForInput = false;
            DialogueLine currentLine = _linesQueue.Dequeue();

            // 1. 발화자 이름 결정 (speakerOverride가 비어있으면 말을 건 NPC 오브젝트의 이름 사용)
            if (!string.IsNullOrEmpty(currentLine.speakerOverride))
            {
                nameText.text = currentLine.speakerOverride;
            }
            else if (_currentNPC is Component npcComponent)
            {
                nameText.text = npcComponent.gameObject.name;
            }
            else
            {
                nameText.text = "???";
            }

            // 2. 대사 텍스트 출력
            dialogueText.text = currentLine.text;

            // 3. autoAdvanceDelay 값에 따른 넘김 방식 분기
            if (currentLine.autoAdvanceDelay > 0f)
            {
                // 설정된 시간이 지나면 타이머처럼 자동으로 넘어가도록 코루틴 실행
                _autoAdvanceCoroutine = StartCoroutine(Co_AutoAdvance(currentLine.autoAdvanceDelay));
            }
            else
            {
                // 0초이면 플레이어가 버튼을 직접 누를 때까지 Update에서 감지하도록 대기 상태 전환
                _isWaitingForInput = true;
            }
        }

        /// <summary>
        /// 지정된 딜레이 시간(초)만큼 대기한 후 자동으로 다음 대사를 호출하는 코루틴
        /// </summary>
        private IEnumerator Co_AutoAdvance(float delay)
        {
            yield return new WaitForSeconds(delay);
            DisplayNextSentence();
        }

        /// <summary>
        /// 대사 리스트를 모두 소모하여 대사창을 닫고 종료하는 함수
        /// </summary>
        private void EndDialogue()
        {
            _isWaitingForInput = false;
            dialogueUIObject.SetActive(false);
            Debug.Log("[DialogueSystem] 대사 시퀀스 완료.");

            // 대사를 시작했던 NPC에게 마쳤다는 신호를 전달 
            // ➡️ NPC 스크립트 내부에서 OnDialogueEnd 이벤트를 트리거하여 승희/세은이가 감지하게 함
            if (_currentNPC != null)
            {
                if (_currentNPC is CompanionController companion)
                {
                    companion.FinishDialogue();
                }
            }
        }
    }
}