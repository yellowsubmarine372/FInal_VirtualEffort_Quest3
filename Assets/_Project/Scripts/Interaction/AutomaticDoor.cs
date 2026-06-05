using ReadyFriendsOne.Dialogue;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    private Animator _animator;
    private int _playerCount = 0;
    private bool _hasReturnedDialoguePlayed = false;

    public RoomAnomalyManager anomalyManager;
    public CompanionController companionNPC;
    public DialogueData afterReturnDialogue;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        _playerCount++;
        if (_playerCount == 1)
        {
            _animator.SetBool("IsOpen", true);

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
            _animator.SetBool("IsOpen", false);
            if (anomalyManager != null)
            {
                anomalyManager.OnPlayerExitedRoom();
            }
        }
    }
}