using ReadyFriendsOne.Core;
using UnityEngine;

namespace ReadyFriendsOne.Interaction
{
    /// <summary>
    /// 인터랙트 시 지정한 씬으로 페이드 전환.
    /// VR 헤드셋 오브젝트, 문 등에 SimpleInteractable과 함께 붙임.
    /// SimpleInteractable.OnInteract → TriggerLoad() 연결.
    /// </summary>
    public class SceneTrigger : MonoBehaviour
    {
        [SerializeField] private string targetScene;
        [SerializeField] private StoryStage nextStage;

        public void TriggerLoad()
        {
            GameState.Stage = nextStage;
            SceneLoader.Load(targetScene);
        }
    }
}
