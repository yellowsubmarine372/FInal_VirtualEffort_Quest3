using System.Collections;
using UnityEngine;

namespace ReadyFriendsOne.Core
{
    /// <summary>
    /// 01_Room404.unity 씬 전용 컨트롤러.
    /// GameState.Stage가 Intro면 Scene 1, Return이면 Scene 9 연출.
    /// Hierarchy에서 각 슬롯에 오브젝트 연결해서 씀.
    /// </summary>
    public class Room404Controller : MonoBehaviour
    {
        [Header("Scene 1 전용 오브젝트")]
        [SerializeField] private GameObject[] scene1OnlyObjects;

        [Header("Scene 9 전용 오브젝트 (추억 흔적들)")]
        [SerializeField] private GameObject[] scene9OnlyObjects;

        [Header("말풍선")]
        [SerializeField] private ThoughtBubble scene1Bubble;
        [SerializeField] private ThoughtBubbleSequencer scene9Sequencer;

        [Header("Scene 1 말풍선 텍스트")]
        [SerializeField] private string scene1BubbleText = "오늘도 혼자 먹네…";

        private void Start()
        {
            if (GameState.Stage == StoryStage.Return)
                SetupScene9();
            else
                SetupScene1();
        }

        private void SetupScene1()
        {
            SetActive(scene1OnlyObjects, true);
            SetActive(scene9OnlyObjects, false);

            if (scene1Bubble != null)
                scene1Bubble.Show(scene1BubbleText);
        }

        private void SetupScene9()
        {
            SetActive(scene1OnlyObjects, false);
            SetActive(scene9OnlyObjects, true);

            if (scene9Sequencer != null)
                StartCoroutine(DelayedSequence(2f));
        }

        private IEnumerator DelayedSequence(float delay)
        {
            yield return new WaitForSeconds(delay);
            scene9Sequencer.StartSequence();
        }

        private void SetActive(GameObject[] objects, bool active)
        {
            if (objects == null) return;
            foreach (var obj in objects)
                if (obj != null) obj.SetActive(active);
        }
    }
}
