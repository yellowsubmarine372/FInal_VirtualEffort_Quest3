using System.Collections.Generic;
using ReadyFriendsOne.Memory;

namespace ReadyFriendsOne.Core
{
    public enum StoryStage
    {
        Intro,          // Scene 1: 404호 초기
        Plaza,          // Scene 2: 가상 광장
        MemoryBuilding, // Scene 3~5: 추억 보관소 (쌓기)
        Promise,        // Scene 6: 약속
        GlitchSubtle,   // Scene 7: 흔적 변경 (미세 변화)
        GlitchCollapse, // Scene 8: 균열 (붕괴)
        Return          // Scene 9: 404호 복귀
    }

    /// <summary>
    /// 씬 간 공유 데이터. 씬 전환 시에도 유지됨.
    /// Owner: 박세은 — 필드 추가 필요 시 PR로 요청
    /// </summary>
    public static class GameState
    {
        public static StoryStage Stage = StoryStage.Intro;

        // Scene 2에서 선택한 NPC의 관심사 ("음악" / "운동" / "영화")
        public static string SelectedInterest = "";

        // NPC 이름 — Scene 8 이후 "희동이"로 교체됨
        public static string NpcName = "둘리";

        // Scene 3~5에서 쌓은 추억 목록 — Scene 9에서 재현
        public static List<MemoryItem> Memories = new List<MemoryItem>();

        // Scene 9 분기: true = 희망 엔딩 연출 활성화
        public static bool IsHopefulEnding = false;

        public static void Reset()
        {
            Stage = StoryStage.Intro;
            SelectedInterest = "";
            NpcName = "둘리";
            Memories.Clear();
            IsHopefulEnding = false;
        }
    }
}
