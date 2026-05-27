# Ready Friends One — 팀 세팅 가이드

> 기말 프로젝트 | Quest 3 VR | Unity 2022.x

---

## 1. 클론 후 최초 세팅

```bash
git clone https://github.com/yellowsubmarine372/FInal_VirtualEffort_Quest3.git
cd FInal_VirtualEffort_Quest3
```

Unity Hub에서 **Add project from disk** → 이 폴더 선택 후 열기.  
패키지 임포트가 끝날 때까지 기다린 뒤 작업 시작.

### Asset Serialization 확인 (필수)

**Edit → Project Settings → Editor → Asset Serialization → Mode: Force Text**  
이 설정이 되어 있어야 씬/프리팹 diff가 가능하고 머지 충돌 해결이 쉬워집니다.

---

## 2. 브랜치 전략

| 브랜치 | 담당 | 용도 |
|---|---|---|
| `main` | 전체 | 직접 push 금지. PR로만 머지 |
| `feature/core-system` | 박세은 | Core + Scene 1, 9 |
| `feature/environment` | 최이준 | 환경 프리팹 + Scene 2, 6 |
| `feature/npc-interaction` | 송승희 | NPC + Dialogue + Memory + Scene 7, 8 |

```bash
# 각자 브랜치 체크아웃
git checkout -b feature/core-system      # 세은
git checkout -b feature/environment      # 이준
git checkout -b feature/npc-interaction  # 승희
```

---

## 3. 폴더 구조

```
Assets/
├── _Project/                   ← 기말 프로젝트 (여기서만 작업)
│   ├── Scenes/                 ← .unity 씬 파일
│   ├── Scripts/
│   │   ├── Core/               ← GameState, SceneLoader (Owner: 세은)
│   │   ├── Dialogue/           ← IDialogueTrigger, DialogueData, DialogueSystem (Owner: 승희)
│   │   ├── Interaction/        ← XR 인터랙션 (Owner: 세은)
│   │   ├── NPC/                ← NPC 행동 (Owner: 승희)
│   │   └── Memory/             ← MemoryItem, MemoryManager (Owner: 승희)
│   ├── Prefabs/
│   │   ├── Player/             ← XRPlayerRig (Owner: 세은, 수정 금지)
│   │   ├── NPC/                ← AmbientNPC, CompanionNPC (Owner: 승희)
│   │   ├── Interactables/      ← MemoryItem 프리팹들 (Owner: 승희)
│   │   ├── UI/                 ← FadeCanvas, 말풍선 (Owner: 세은)
│   │   └── Environment/        ← 방, 광장, 추억보관소 (Owner: 이준)
│   ├── Art/                    ← 모델/머티리얼/텍스처/애니메이션
│   ├── Audio/                  ← BGM / SFX
│   └── ScriptableObjects/
│       ├── Dialogues/          ← 씬별 대사 SO (각자 담당 씬 폴더에)
│       └── MemoryItems/        ← MemoryItem SO (Owner: 승희)
├── ProfessorScripts/           ← 교수님 제공 스크립트 (절대 수정 금지)
└── (나머지 기존 폴더들)        ← 중간 프로젝트 파일 — 건드리지 말 것
```

---

## 4. 씬 파일 목록

| 파일명 | 스토리 씬 | Owner |
|---|---|---|
| `00_Bootstrap.unity` | 앱 시작, 매니저 초기화 | 세은 |
| `01_Room404.unity` | Scene 1 (Intro) + Scene 9 (Return) | 세은 |
| `02_Plaza.unity` | Scene 2 (가상 광장) | 이준 |
| `03_MemoryRoom.unity` | Scene 3~5 (보관소 빌딩) + 7 (글리치) + 8 (균열) | 이준(환경) / 승희(인터랙션) |
| `06_Promise.unity` | Scene 6 (약속) | 이준 |

**⚠️ 같은 씬 파일을 동시에 수정하지 말 것.** 작업 전 카톡으로 "03_MemoryRoom 작업 시작" 공유.

---

## 5. 코어 시스템 사용법

### GameState (씬 간 데이터 공유)

```csharp
using ReadyFriendsOne.Core;

// 스테이지 변경
GameState.Stage = StoryStage.MemoryBuilding;

// 선택한 관심사 저장 (Scene 2)
GameState.SelectedInterest = "음악";

// 추억 추가 (Scene 3~5)
GameState.Memories.Add(myMemoryItem);
```

### SceneLoader (씬 전환 + 페이드)

```csharp
using ReadyFriendsOne.Core;

// 페이드 아웃 → 씬 로드 → 페이드 인
SceneLoader.Load("02_Plaza");
```

씬에 `SceneLoader` 컴포넌트가 붙은 GameObject가 있어야 함.  
`00_Bootstrap` 씬에서 DontDestroyOnLoad로 유지됨.

### IDialogueTrigger (대사 트리거)

```csharp
using ReadyFriendsOne.Dialogue;

// NPC에서 대사 시작
var npc = companionNpcGameObject.GetComponent<IDialogueTrigger>();
npc.OnDialogueEnd += () => SceneLoader.Load("02_Plaza");
npc.PlayDialogue("scene01_greeting");
```

---

## 6. 네이밍 / 코딩 규칙

- **namespace**: 반드시 `ReadyFriendsOne.*` 사용
- **스크립트**: PascalCase (예: `MemoryManager.cs`)
- **프리팹**: PascalCase (예: `CompanionNPC.prefab`)
- **ScriptableObject**: 파일명 = dialogueId (예: `scene02_greeting.asset`)
- **주석**: 영어 또는 한국어 모두 OK. 코드 내 Owner 표기 유지.

---

## 7. Day 0 완료 체크리스트 (세은)

- [x] `Assets/_Project/` 폴더 구조 생성
- [x] `GameState.cs` 작성 & push
- [x] `SceneLoader.cs` 작성 & push
- [x] `IDialogueTrigger.cs` 작성 & push
- [x] `MemoryItem.cs` 작성 & push
- [x] `DialogueData.cs` 작성 & push
- [ ] Unity에서 빈 씬 5개 생성 후 push (`00_Bootstrap` ~ `06_Promise`)
- [ ] `XRPlayerRig.prefab` 셋업 후 push
- [ ] `FadeCanvas.prefab` 셋업 후 push

## Day 1~2 작업 시작 조건

| 담당 | 시작 가능 조건 |
|---|---|
| 이준 | 세은의 Day 0 push 완료 후 바로 시작 가능 |
| 승희 | 세은의 Day 0 push 완료 후 바로 시작 가능 |

---

## 8. 자주 묻는 것들

**Q. 에셋스토어 패키지 어디에 임포트해요?**  
A. 그냥 기본 위치에 임포트하세요. Asset Store 패키지는 `Assets/` 루트 직하에 생성됨. `_Project/` 언더스코어 덕분에 우리 파일과 섞이지 않음.

**Q. ProfessorScripts 수정하면 안 되나요?**  
A. 수정 금지. 교수님 코드에 기능을 추가하고 싶으면 Wrapper 클래스를 `_Project/Scripts/` 안에 새로 만드세요.

**Q. 씬 전환할 때 GameState 데이터가 날아가요.**  
A. `GameState`는 static이라 씬 전환해도 유지됩니다. `SceneLoader`는 DontDestroyOnLoad. 문제 생기면 세은에게 문의.

**Q. Quest 빌드는 어떻게 해요?**  
A. 빌드는 세은이 담당. Day 8~10에 통합 빌드 검증 예정.
