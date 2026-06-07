using ReadyFriendsOne.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyFriendsOne.Core
{
    /// <summary>
    /// 02_Plaza.unity 전용. MusicNPC의 "응!" 버튼을 자동으로 씬 전환에 연결.
    /// Inspector 설정 없이 Start()에서 자동 와이어링.
    /// </summary>
    public class PlazaController : MonoBehaviour
    {
        private void Start()
        {
            WireNPC("MusicNPC", "음악", "03_MemoryMusic");
            WireNPC("MovieNPC", "영화", "04_MemoryMovie");
            WireNPC("SportsNPC", "운동", "05_MemorySports");
        }

        private void WireNPC(string npcName, string interest, string targetScene)
        {
            GameObject npcObj = GameObject.Find(npcName);
            if (npcObj == null) return;

            Button btn = npcObj.GetComponentInChildren<Button>(true);
            if (btn == null)
            {
                Debug.LogWarning($"[PlazaController] {npcName}에서 Button 컴포넌트를 찾지 못했습니다.");
                return;
            }

            string capturedInterest = interest;
            string capturedScene = targetScene;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                GameState.SelectedInterest = capturedInterest;
                GameState.Stage = StoryStage.MemoryBuilding;
                SceneLoader.Load(capturedScene);
            });

            Debug.Log($"[PlazaController] {npcName} 버튼 → {capturedScene} 연결 완료");
        }
    }
}
