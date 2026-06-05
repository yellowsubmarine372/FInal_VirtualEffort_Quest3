using UnityEngine;

public class HeadsetClick : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 오른손 트리거로 클릭
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            RaycastHit hit;
            // RayClickInteractor가 이미 있으니 레이캐스트 직접
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    if (audioSource.isPlaying)
                        audioSource.Stop();
                    else
                        audioSource.Play();
                }
            }
        }
    }
}