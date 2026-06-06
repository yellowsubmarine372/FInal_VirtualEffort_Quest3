using UnityEngine;

public class HeadsetClick : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Quest 컨트롤러 레이가 이 오브젝트에 닿은 상태에서 트리거 누르면 호출
    public void OnClick()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
        else
            audioSource.Play();
    }
}