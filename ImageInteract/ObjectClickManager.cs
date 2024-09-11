using UnityEngine;

public class ObjectClickManager : MonoBehaviour
{
    public GameObject fireBtn;
    public GameObject waterBtn;
    public GameObject electricBtn;
    public GameObject grassBtn;
    public GameObject groundBtn;

    void Start()
    {
        UpdateButtonStates();  // 씬 로딩 시 버튼 상태 초기화
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string tag = hit.transform.tag;
                if (SkillManager.Instance.SkillStatus.ContainsKey(tag))
                {
                    SkillManager.Instance.ToggleSkill(tag);
                    UpdateButtonStates();  // 스킬 상태 변경 후 UI 업데이트
                }
            }
        }
    }

    void UpdateButtonStates()
    {
        // 스킬 버튼의 활성화 상태를 SkillManager의 스킬 상태에 맞추어 업데이트
        fireBtn.SetActive(SkillManager.Instance.SkillStatus["Red"]);
        waterBtn.SetActive(SkillManager.Instance.SkillStatus["Blue"]);
        electricBtn.SetActive(SkillManager.Instance.SkillStatus["Yellow"]);
        grassBtn.SetActive(SkillManager.Instance.SkillStatus["Green"]);
        groundBtn.SetActive(SkillManager.Instance.SkillStatus["Brown"]);
    }
}
