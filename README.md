# VirtualEffort — Quest 3 VR 기말 프로젝트

> 팀명: Ready Friends One | Unity 2022.x + Meta Quest 3

---

## 프로젝트 소개

현실에서 관계 맺기에 어려움을 겪는 사람이 VR 공간에서 NPC와 추억을 쌓고, 그 관계가 점차 균열되는 과정을 체험하는 VR 인터랙티브 스토리입니다.

**핵심 질문:** *VR이 보완해주는 결핍이 무엇인지, 그리고 어디서 실패하는지를 플레이어가 직접 경험할 수 있는가?*

---

## 씬 흐름

```
00_Bootstrap
    └─> 01_Room404  (Scene 1: 고시원, 고립된 현실)
            └─> 02_Plaza  (Scene 2: 가상 광장, NPC 만남)
                    └─> 03_MemoryMusic / 04_MemoryMovie / 05_MemorySports
                            (Scene 3~5: 추억 보관소, NPC와 함께 공간 채우기)
                            └─> 06_Promise  (Scene 6: 약속, NPC의 한계 드러남)
                                    └─> 07_Breakdown  (Scene 7: 흔적 변경, 미세한 이상)
                                            └─> 08_Crack  (Scene 8: 균열, 기억 붕괴)
                                                    └─> 01_Room404  (Scene 9: 현실 복귀)
```

---

## 팀원 역할

| 이름 | 담당 |
|---|---|
| 박세은 | Core 시스템 (GameState, SceneLoader), Scene 1/9 (404호), 씬 통합 및 빌드 |
| 송승희 | 환경 프리팹 (GoshiwonRoom, MemoryRoom, PlazaIsland), Scene 2/3~5/6 레이아웃 |
| 최이준 | NPC 프리팹 (AmbientNPC, CompanionNPC), Dialogue/Memory 시스템, Scene 7/8 |

---

## 기술 스택

- **엔진:** Unity 2022.3 LTS
- **타겟 기기:** Meta Quest 3 (Android, OpenXR)
- **주요 SDK/패키지:** OVR SDK, Photon Voice 2, TextMeshPro, Unity XR Interaction Toolkit
- **스크립트 네임스페이스:** `ReadyFriendsOne.*`

---

## 빌드 및 실행 방법

### 에디터 실행

```bash
git clone https://github.com/yellowsubmarine372/FInal_VirtualEffort_Quest3.git
```

1. Unity Hub → **Add project from disk** → 클론한 폴더 선택
2. Unity 2022.3.x 버전으로 열기 (패키지 임포트 완료까지 대기)
3. **Edit → Project Settings → Editor → Asset Serialization → Force Text** 확인
4. `Assets/_Project/Scenes/00_Bootstrap.unity` 열고 Play

### Quest 빌드

1. **File → Build Settings** → Android 플랫폼으로 Switch
2. Player Settings → `com.ReadyFriendsOne.VirtualEffort` 패키지명 확인
3. **Build And Run** → Quest 연결 후 빌드

---

## 프로젝트 구조

```
Assets/
├── _Project/                  ← 기말 프로젝트 작업 폴더
│   ├── Scenes/                ← 00_Bootstrap ~ 08_Crack
│   ├── Scripts/
│   │   ├── Core/              ← GameState, SceneLoader, PlazaController, Room404Controller
│   │   ├── Dialogue/          ← DialogueSystem, DialogueData, Scene7_Director, Scene8_GlitchManager
│   │   ├── Interaction/       ← SceneTrigger, SimpleInteractable, AutomaticDoor, DoorZoneTrigger
│   │   ├── NPC/               ← CompanionController, SwappableMaterial
│   │   └── Memory/            ← MemoryItem, RoomAnomalyManager, PlayerExitDetector
│   ├── Prefabs/
│   │   ├── Player/            ← XRPlayerRig (수정 금지)
│   │   ├── NPC/               ← AmbientNPC, CompanionNPC
│   │   ├── Interactables/     ← MemoryItem_Music/Movie/Sports
│   │   ├── UI/                ← FadeCanvas, ThoughtBubble, TextPopup
│   │   └── Environment/       ← GoshiwonRoom, MemoryRoom, PlazaIsland
│   ├── Audio/
│   │   └── BGM/               ← 배경음악
│   └── ScriptableObjects/
│       └── Dialogues/         ← 씬별 대사 데이터 (DialogueData SO)
└── (기타 에셋스토어 패키지들)
```

---

## 핵심 스크립트

### `GameState.cs`
씬 간 공유 데이터. static이므로 씬 전환 후에도 유지됩니다.

```csharp
GameState.Stage          // 현재 스토리 단계 (StoryStage enum)
GameState.SelectedInterest  // 플레이어가 선택한 NPC 관심사 ("음악"/"영화"/"운동")
GameState.NpcName        // NPC 이름 ("둘리" → Scene8 이후 "희동이")
GameState.Memories       // 쌓은 추억 목록
```

### `SceneLoader.cs`
페이드 인/아웃과 함께 씬 전환. `00_Bootstrap`에서 `DontDestroyOnLoad`로 유지됩니다.

```csharp
SceneLoader.Load("02_Plaza");  // 어디서든 호출 가능
```

### `CompanionController.cs`
NPC 대화를 자동 시작하고, 대화 종료 시 다음 씬으로 자동 전환합니다.
- Memory 씬(03~05) → `06_Promise`
- `06_Promise` → `07_Breakdown`

### `DialogueSystem.cs`
대사 ID로 `DialogueData` ScriptableObject를 로드해 World Space UI에 출력합니다. OVR A버튼 또는 Space키로 다음 대사로 넘어갑니다.

---

## 씬 전환 로직 요약

| 씬 | 전환 조건 | 다음 씬 |
|---|---|---|
| 00_Bootstrap | 자동 (0.5초 후) | 01_Room404 |
| 01_Room404 | VR 헤드셋 오브젝트 클릭 | 02_Plaza |
| 02_Plaza | MusicNPC "응!" 버튼 클릭 | 03_MemoryMusic |
| 03~05 Memory | CompanionNPC 대화 종료 | 06_Promise |
| 06_Promise | CompanionNPC 대화 종료 | 07_Breakdown |
| 07_Breakdown | 플레이어가 복도 퇴장 후 20초 | 08_Crack |
| 08_Crack | Scene8 글리치 대화 종료 | 01_Room404 (Stage=Return) |
| 01_Room404 (Return) | 문으로 걸어가면 | 엔딩 크레딧 |

---

## Git 브랜치 전략

```
main                     ← PR로만 머지, 직접 push 금지
feature/core-system      ← 박세은
feature/environment      ← 송승희
feature/npc-interaction  ← 최이준
feature/final-integration ← 씬 연결 및 최종 통합
```
