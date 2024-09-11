using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
           // Debug.LogError("AudioSource component is missing on this object");
        }
    }

    void OnMouseDown()
    {
        // 클릭 이벤트 발생 시 소리 재생
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
