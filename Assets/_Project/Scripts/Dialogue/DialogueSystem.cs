using ReadyFriendsOne.Dialogue;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    public GameObject dialogueUIObject;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public GameObject thoughtBubblePrefab;
    public Transform playerHeadTransform;

    private GameObject _activeBubbleInstance;
    private TextMeshProUGUI _bubbleTextComponent;

    private Queue<DialogueLine> _linesQueue;
    private IDialogueTrigger _currentNPC;
    private Coroutine _autoAdvanceCoroutine;
    private bool _isWaitingForInput = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _linesQueue = new Queue<DialogueLine>();
            if (dialogueUIObject != null) dialogueUIObject.SetActive(false);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (_isWaitingForInput)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.Space))
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(DialogueData data, IDialogueTrigger npc)
    {
        if (data == null) return;

        _currentNPC = npc;
        if (_autoAdvanceCoroutine != null) StopCoroutine(_autoAdvanceCoroutine);

        _linesQueue.Clear();
        foreach (DialogueLine line in data.lines)
        {
            _linesQueue.Enqueue(line);
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (_linesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (_autoAdvanceCoroutine != null) StopCoroutine(_autoAdvanceCoroutine);
        _isWaitingForInput = false;

        DialogueLine currentLine = _linesQueue.Dequeue();
        string speaker = currentLine.speakerOverride;

        if (speaker == "나" || speaker == "플레이어")
        {
            if (dialogueUIObject != null) dialogueUIObject.SetActive(false);
            ShowPlayerThought(currentLine.text);
        }
        else
        {
            HidePlayerThought();
            if (dialogueUIObject != null) dialogueUIObject.SetActive(true);
            if (nameText != null) nameText.text = !string.IsNullOrEmpty(speaker) ? speaker : "NPC";
            if (dialogueText != null) dialogueText.text = currentLine.text;
        }

        if (currentLine.autoAdvanceDelay > 0f)
        {
            _autoAdvanceCoroutine = StartCoroutine(Co_AutoAdvance(currentLine.autoAdvanceDelay));
        }
        else
        {
            _isWaitingForInput = true;
        }
    }

    private void ShowPlayerThought(string text)
    {
        if (_activeBubbleInstance == null && thoughtBubblePrefab != null)
        {
            Vector3 spawnPos;
            Quaternion spawnRot;

            // 카메라(눈) 위치를 정상적으로 찾았다면
            if (playerHeadTransform != null)
            {
                // 카메라 정면 방향으로 1.2미터 앞, 살짝 위(0.1)에 배치
                spawnPos = playerHeadTransform.position + playerHeadTransform.forward * 1.2f + playerHeadTransform.up * 0.1f;
                // 카메라를 바라보도록 회전 정렬
                spawnRot = Quaternion.LookRotation(spawnPos - playerHeadTransform.position);
            }
            else
            {
                // 만약 인스펙터에서 깜빡하고 안 넣었다면 메인 카메라 기준 구출 서포트
                Transform mainCam = Camera.main != null ? Camera.main.transform : null;
                if (mainCam != null)
                {
                    spawnPos = mainCam.position + mainCam.forward * 1.2f;
                    spawnRot = Quaternion.LookRotation(spawnPos - mainCam.position);
                }
                else
                {
                    spawnPos = new Vector3(0, 1.5f, 1.2f);
                    spawnRot = Quaternion.identity;
                }
            }

            _activeBubbleInstance = Instantiate(thoughtBubblePrefab, spawnPos, spawnRot);
            _bubbleTextComponent = _activeBubbleInstance.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (_activeBubbleInstance != null)
        {
            _activeBubbleInstance.SetActive(true);

            // 생성 후에도 플레이어가 고개를 돌렸을 수 있으니 위치와 회전을 플레이어 정면으로 강제 재조정
            if (playerHeadTransform != null)
            {
                _activeBubbleInstance.transform.position = playerHeadTransform.position + playerHeadTransform.forward * 1.2f;
                _activeBubbleInstance.transform.rotation = Quaternion.LookRotation(_activeBubbleInstance.transform.position - playerHeadTransform.position);
            }

            if (_bubbleTextComponent != null) _bubbleTextComponent.text = text;
        }
    }

    private void HidePlayerThought()
    {
        if (_activeBubbleInstance != null) _activeBubbleInstance.SetActive(false);
    }

    private IEnumerator Co_AutoAdvance(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        _isWaitingForInput = false;
        if (dialogueUIObject != null) dialogueUIObject.SetActive(false);
        HidePlayerThought();

        if (_currentNPC is CompanionController companion)
        {
            companion.FinishDialogue();
        }
    }
}