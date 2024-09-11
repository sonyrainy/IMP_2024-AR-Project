using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public Scrollbar scrollbar; // 사용하는 스크롤 바
    public float shrinkDuration = 60f; // 줄어들기가 완료되는데 걸리는 시간 (초)

    private float startTime;

    void Start()
    {
        startTime = Time.time; // 시작 시간 기록
    }

    void Update()
    {
        // 현재 시간과 시작 시간 사이의 경과 시간 계산
        float elapsedTime = Time.time - startTime;

        // 경과 시간이 줄어들기의 지속 시간을 초과하지 않도록 제한
        float clampedTime = Mathf.Clamp(elapsedTime, 0f, shrinkDuration);

        // 스크롤 바의 크기를 줄이는 비율 계산
        float shrinkAmount = 1f - (clampedTime / shrinkDuration);

        // 스크롤 바의 크기를 조정하여 스크롤을 줄여나감
        scrollbar.size = shrinkAmount;

        // 경과 시간이 지속 시간을 초과하면 스크롤이 줄어드는 애니메이션을 종료
        if (elapsedTime >= shrinkDuration)
        {
            enabled = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }


    }
}
