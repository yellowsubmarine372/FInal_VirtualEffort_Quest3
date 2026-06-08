using System.Collections;
using UnityEngine;

namespace ReadyFriendsOne.Core
{
    /// <summary>
    /// 00_Bootstrap 씬 전용. 세팅(SceneLoader 등 초기화) 후 첫 게임 씬으로 자동 전환.
    /// Bootstrap 씬의 오브젝트(예: SceneLoader)에 붙이고 firstScene 지정.
    /// Owner: 박세은
    /// </summary>
    public class BootstrapLoader : MonoBehaviour
    {
        [SerializeField] private string firstScene = "01_Room404";

        [Tooltip("로드 전 대기 시간(초). XR/매니저 초기화 여유.")]
        [SerializeField] private float delay = 0.5f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            SceneLoader.Load(firstScene);
        }
    }
}
