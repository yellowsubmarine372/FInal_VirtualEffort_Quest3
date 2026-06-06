using ReadyFriendsOne.Core;
using ReadyFriendsOne.Dialogue;
using System.Collections;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    private Animator _animator;
    private int _playerCount = 0;
    private bool _hasReturnedDialoguePlayed = false;
    private bool _transitionScheduled = false;

    public RoomAnomalyManager anomalyManager;
    public CompanionController companionNPC;
    public DialogueData afterReturnDialogue;

    [Tooltip("Scene7: anomaly 발생 후 Scene8로 전환할 때까지 대기 시간(초)")]
    public float scene8TransitionDelay = 20f;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        _playerCount++;
        if (_playerCount == 1)
        {
            if (_animator != null) _animator.SetBool("IsOpen", true);

            if (!_hasReturnedDialoguePlayed && companionNPC != null && afterReturnDialogue != null)
            {
                _hasReturnedDialoguePlayed = true;
                DialogueSystem.Instance.StartDialogue(afterReturnDialogue, companionNPC);
            }
        }
    }

    public void CloseDoor()
    {
        _playerCount--;
        if (_playerCount < 0) _playerCount = 0;

        if (_playerCount == 0)
        {
            if (_animator != null) _animator.SetBool("IsOpen", false);

            if (anomalyManager != null)
            {
                anomalyManager.OnPlayerExitedRoom();

                if (!_transitionScheduled)
                {
                    _transitionScheduled = true;
                    StartCoroutine(Co_TransitionToScene8());
                }
            }
        }
    }

    private IEnumerator Co_TransitionToScene8()
    {
        yield return new WaitForSeconds(scene8TransitionDelay);
        GameState.Stage = StoryStage.GlitchCollapse;
        SceneLoader.Load("08_Crack");
    }
}
