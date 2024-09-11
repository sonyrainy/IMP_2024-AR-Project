using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 관리에 필요

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    public Dictionary<string, bool> SkillStatus { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시 객체 파괴 방지
            InitializeSkills();  // 스킬 초기화
            SceneManager.sceneLoaded += OnSceneLoaded;  // 씬 로드 이벤트 핸들러 등록
        }
        else if (Instance != this)
        {
            Destroy(gameObject);  // 중복 인스턴스 제거
        }
    }

    private void OnDestroy()
    {
        // 객체 파괴 시 이벤트 핸들러 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeSkills()
    {
        SkillStatus = new Dictionary<string, bool>
        {
            {"Green", false},
            {"Blue", false},
            {"Red", false},
            {"Brown", false},
            {"Yellow", false}
        };
    }

    public void ToggleSkill(string skill)
    {
        if (SkillStatus.ContainsKey(skill))
        {
            int activeCount = CountActiveSkills();

            if (!SkillStatus[skill] && activeCount >= 3)
            {
                Debug.Log("Cannot activate more than 3 skills.");
                return;
            }

            SkillStatus[skill] = !SkillStatus[skill];
            Debug.Log(skill + " skill is now " + (SkillStatus[skill] ? "activated" : "deactivated"));
        }
        else
        {
            Debug.Log("Skill key not found: " + skill);
        }
    }

    private int CountActiveSkills()
    {
        int count = 0;
        foreach (var entry in SkillStatus)
        {
            if (entry.Value)
            {
                count++;
            }
        }
        return count;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene '{scene.name}' loaded. Active skills are:");
        foreach (var skill in SkillStatus)
        {
            if (skill.Value)  // 스킬이 활성화된 경우
            {
                Debug.Log(skill.Key);
            }
        }
    }
}
