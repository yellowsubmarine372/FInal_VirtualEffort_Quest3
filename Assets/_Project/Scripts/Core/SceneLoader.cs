using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReadyFriendsOne.Core
{
    /// <summary>
    /// 씬 전환 + 페이드 인/아웃 관리.
    /// FadeCanvas 프리팹이 씬에 존재해야 페이드가 동작함.
    /// Owner: 박세은
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [SerializeField] private float fadeDuration = 0.8f;

        private CanvasGroup _fadeCanvas;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            FindFadeCanvas();
            StartCoroutine(FadeIn());
        }

        public void LoadWithFade(string sceneName)
        {
            StartCoroutine(FadeAndLoad(sceneName));
        }

        public static void Load(string sceneName)
        {
            if (Instance != null)
                Instance.LoadWithFade(sceneName);
            else
                SceneManager.LoadScene(sceneName);
        }

        private IEnumerator FadeAndLoad(string sceneName)
        {
            yield return StartCoroutine(FadeOut());
            SceneManager.LoadScene(sceneName);
            yield return null;
            FindFadeCanvas();
            yield return StartCoroutine(FadeIn());
        }

        private IEnumerator FadeOut()
        {
            if (_fadeCanvas == null) yield break;
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                _fadeCanvas.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }
            _fadeCanvas.alpha = 1f;
        }

        private IEnumerator FadeIn()
        {
            if (_fadeCanvas == null) yield break;
            float t = fadeDuration;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                _fadeCanvas.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }
            _fadeCanvas.alpha = 0f;
        }

        private void FindFadeCanvas()
        {
            var go = GameObject.FindWithTag("FadeCanvas");
            if (go != null)
                _fadeCanvas = go.GetComponent<CanvasGroup>();
        }
    }
}
