using ReadyFriendsOne.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ReadyFriendsOne.Dialogue;

public class Scene8_GlitchManager : MonoBehaviour
{
    [Header("1. 조명 및 환경 세팅")]
    public LightFlicker targetLightFlicker;
    public Light actualLight;
    public List<GameObject> wallPhotos;

    [Header("2. NPC 세팅")]
    public Animator npcAnimator;
    public Renderer npcRenderer; // GetComponentsInChildren을 쓰므로 인스펙터에서 비워둬도 작동합니다.
    public Color glitchColor = Color.cyan;

    [Header("3. 대사 시스템 연동")]
    public DialogueData scene8Dialogue;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueUIObject;

    private int _dialogueIndex = 0;
    private bool _isGlitchTriggered = false;

    void Start()
    {
        StartCoroutine(Co_PlayScene8Sequence());
    }

    private IEnumerator Co_PlayScene8Sequence()
    {
        yield return new WaitForSeconds(1f); // 씬 진입 후 잠시 대기

        if (targetLightFlicker != null)
        {
            Debug.Log("[Scene8] 조명 불안정 시작 + NPC 글리치 동시 발생!");
            targetLightFlicker.AccelerateFlicker(0.02f);
        }

        if (npcAnimator != null)
        {
            npcAnimator.Play("Glitch", 0, 0f);
        }

        yield return new WaitForSeconds(2.0f);

        if (targetLightFlicker != null) targetLightFlicker.enabled = false;
        if (actualLight != null) actualLight.intensity = 0f; // 방을 완전히 컴컴하게 암전

        yield return new WaitForSeconds(0.3f);

        // 어둠 속에서 벽 사진들이 순식간에 전부 사라집니다.
        foreach (GameObject photo in wallPhotos)
        {
            if (photo != null)
            {
                photo.SetActive(false);
                yield return new WaitForSeconds(0.3f);
            }
        }

        yield return new WaitForSeconds(0.3f);

        if (actualLight != null) actualLight.intensity = 1f; // 불 다시 켜짐

        if (npcAnimator != null)
        {
            npcAnimator.Play("Idle", 0, 0f); // 글리치 모션 중지
        }

        if (npcAnimator != null)
        {
            Renderer[] allRenderers = npcAnimator.GetComponentsInChildren<Renderer>();
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

            foreach (Renderer rend in allRenderers)
            {
                rend.GetPropertyBlock(propBlock);

                propBlock.SetColor("_Color", glitchColor);
                propBlock.SetColor("_BaseColor", glitchColor);
                propBlock.SetColor("_MainColor", glitchColor);

                rend.SetPropertyBlock(propBlock);
            }
            Debug.Log($"[Scene8] MaterialPropertyBlock을 사용하여 {allRenderers.Length}개 렌더러의 색상을 강제 고정했습니다.");
        }

        _isGlitchTriggered = true;

        StartScene8Dialogue();

    }

    private void StartScene8Dialogue()
    {
        if (dialogueUIObject != null) dialogueUIObject.SetActive(true);
        _dialogueIndex = 0;
        DisplayLine();
    }

    private void Update()
    {
        // 이제 스위치가 켜졌으므로 스페이스바나 VR A 버튼을 누르면 정상적으로 대사가 넘어갑니다.
        if (_isGlitchTriggered && (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.Space)))
        {
            _dialogueIndex++;
            DisplayLine();
        }
    }

    private void DisplayLine()
    {
        if (scene8Dialogue == null || _dialogueIndex >= scene8Dialogue.lines.Length)
        {
            if (dialogueUIObject != null) dialogueUIObject.SetActive(false);
            Debug.Log("[Scene8] 대사 종료 - 플레이어 퇴장.");
            return;
        }

        DialogueLine line = scene8Dialogue.lines[_dialogueIndex];
        if (nameText != null) nameText.text = line.speakerOverride;
        if (dialogueText != null) dialogueText.text = line.text;
    }
}