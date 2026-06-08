using UnityEngine;

namespace ReadyFriendsOne.Memory
{
    public enum MemoryCategory
    {
        Music,   // 음악
        Sports,  // 운동
        Movie,   // 영화
        Star,    // 별 스티커
        Photo    // 사진
    }

    /// <summary>
    /// 씬 3~5에서 수집한 추억 아이템 하나.
    /// ScriptableObject로 만들어서 Assets/_Project/ScriptableObjects/MemoryItems/ 에 저장.
    /// GameState.Memories 리스트에 누적됨 → Scene 9에서 재현.
    /// Owner: 송승희
    /// </summary>
    [CreateAssetMenu(fileName = "MemoryItem_New",
                     menuName = "ReadyFriendsOne/Memory Item")]
    public class MemoryItem : ScriptableObject
    {
        [Tooltip("고유 ID (예: music_001, star_wall_left)")]
        public string id;

        public MemoryCategory category;

        [Tooltip("아이템 아이콘 (UI 표시용)")]
        public Sprite icon;

        [Tooltip("메모/설명 텍스트 (포스트잇에 표시)")]
        [TextArea(2, 4)]
        public string note;

        // 씬 3에서 부착한 월드 위치 — Scene 9에서 동일 위치에 재현
        [HideInInspector] public Vector3 attachedWorldPosition;
        [HideInInspector] public Quaternion attachedWorldRotation;

        // Scene 7에서 변형 여부 추적
        [HideInInspector] public bool isGlitched = false;
    }
}
