using ReadyFriendsOne.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ReadyFriendsOne.Dialogue;

public class Scene8_GlitchManager : MonoBehaviour
{
    [Header("1. 조명 및 환경 설정")]
    public LightFlicker targetLightFlicker;
    public Light actualLight;
    public List<GameObject> wallPhotos;

    [Header("2. NPC 설정")]
    public Animator npcAnimator;
    public Renderer npcRenderer;
    public Color glitchColor = Color.cyan;

    [Header("3. 대사 시스템 설정")]
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
        yield return new WaitForSeconds(1f);

        if (targetLightFlicker != null)
        {
            targetLightFlicker.AccelerateFlicker(0.02f);
        }

        if (npcAnimator != null)
        {
            npcAnimator.Play("Glitch", 0, 0f);
        }

        yield return new WaitForSeconds(2.0f);

        if (targetLightFlicker != null) targetLightFlicker.enabled = false;
        if (actualLight != null) actualLight.intensity = 0f;

        yield return new WaitForSeconds(0.3f);

        foreach (GameObject photo in wallPhotos)
        {
            if (photo != null)
            {
                photo.SetActive(false);
                yield return new WaitForSeconds(0.3f);
            }
        }

        yield return new WaitForSeconds(0.3f);

        if (actualLight != null) actualLight.intensity = 1f;

        if (npcAnimator != null)
        {
            npcAnimator.Play("Idle", 0, 0f);
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
            GameState.Stage = StoryStage.Return;
            SceneLoader.Load("01_Room404");
            return;
        }

        DialogueLine line = scene8Dialogue.lines[_dialogueIndex];
        if (nameText != null) nameText.text = line.speakerOverride;
        if (dialogueText != null) dialogueText.text = line.text;
    }
}
