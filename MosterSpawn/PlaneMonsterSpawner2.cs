using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ARRaycastManager))]

public class PlaneMonsterSpawner2 : MonoBehaviour   // 스테이지 1 몬스터 스폰 스크립트
{
    private ARRaycastManager raycastManager;
    
    [SerializeField] private GameObject prefabFire;   // 몬스터 종류 1
    [SerializeField] private GameObject prefabWater;   // 몬스터 종류 2
    [SerializeField] private GameObject prefabElectric;   // 몬스터 종류 3
    [SerializeField] private GameObject prefabGrass;   // 몬스터 종류 3
    [SerializeField] private GameObject prefabGround;   // 몬스터 종류 3

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();   // ARRaycast 결과 저장 리스트

    [HideInInspector]
    public int phase;   // 스테이지 1의 n번째 페이즈 (ex. 1-1, 1-2 .. 1-n)
    [HideInInspector]
    public bool created; // 페이즈에 나올 몬스터를 모두 스폰했는가? true or false
    [HideInInspector]
    public int monsterCount;    // 스폰된 몬스터가 모두 죽었는지 카운트
    [HideInInspector]
    public Ray ray;     
    [HideInInspector]
    public Pose hitPose;        // 평면위 터치된 위치를 저장

    public GameObject ClickPlaneText;

    scoreController scoreController;
    void Awake()
    {
        monsterCount = 0;   // 초기화 부분들
        raycastManager = GetComponent<ARRaycastManager>();
        phase = 1;  
        created = false;
        ClickPlaneText.SetActive(true);

        scoreController = FindObjectOfType<scoreController>();
        //scoreController.refresh();
    }

    void Update()
    {
        if (created) {  // 만약 해당 페이즈에 나올 몬스터를 모두 스폰했고,
            if (monsterCount == 0) {    // 스폰된 몬스터가 모두 파괴되어 0마리 남아있다면,
                Debug.Log("go next phase"); 
                phase +=1;      // 다음 페이즈로.
                created = false;    // 다음 페이즈에 해당하는 몬스터를 아직 스폰하지 않은 상태이므로 false로 돌려놓음
            }
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && !created && phase == 1)     // 터치 되었고, 1 페이즈이고, 아직 몬스터 스폰을 안했으며,
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // 플레이어의 ray가
            if (raycastManager.Raycast(ray, hitResults, TrackableType.PlaneWithinPolygon))      // plane에 닿았다면, (터치한 곳이 플레인 위라면)
            {
                Debug.Log("phase 1!");  
                hitPose = hitResults[0].pose;   // 터치된 곳 위치 저장하고,  (해당 스테이지에서는 이 위치를 기반으로 주변에 랜덤 스폰함)
                create1();  // 1페이즈 몬스터 스폰
                ClickPlaneText.SetActive(false);
             }
        }
        else if (!created && phase == 2)    // 2페이즈이고, 아직 스폰을 안했다면,
        {
            Debug.Log("phase 2!");
            create2();  // 2페이즈 몬스터 스폰
        }
        else if (!created && phase == 3)    // 3페이즈이고, 아직 스폰을 안했다면,
        {
            Debug.Log("phase 3!");
            create3();  // 3페이즈 몬스터 스폰
        }
        else if (!created && phase == 4)    // 4페이즈는 없으므로
        {
            Debug.Log("Stage 2 Clear!!");  //  스테이지 1 클리어!!
            phase += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }


    void create1() 
    {
        GameObject createRandom = null;


        for (int i = 0; i < 3; i++)     // 1페이즈는 몬스터 3마리 스폰되는데
        {
            int type = Random.Range(0, 5);
            switch (type)
            {
                case 0:
                    createRandom = prefabFire; break;

                case 1:
                    createRandom = prefabWater; break;

                case 2:
                    createRandom = prefabElectric; break;

                case 3:
                    createRandom = prefabGrass; break;

                case 4:
                    createRandom = prefabGround; break;

                default: break;
            }

            var x_rand = Random.Range(-0.7f, 0.7f);
            var z_rand = Random.Range(-0.7f, 0.7f);
            Vector3 ranpos = new Vector3(x_rand, 0, z_rand);
            Vector3 yoffset = new Vector3(0, 0.1f, 0);

            Vector3 spawnPosition = ranpos + yoffset + hitPose.position;
            GameObject monster = Instantiate(createRandom, spawnPosition, Quaternion.identity);  // prefab1P 몬스터 프리펩이 랜덤위치에 스폰
            Rigidbody rb = monster.GetComponent<Rigidbody>();   
            spawnedObjects.Add(monster);    
            monsterCount++;     // 스폰했으므로 카운트 +1
        }
      
        created = true; // 해당 페이즈의 스폰 완료했으므로 true
    }

    void create2()
    {
        GameObject createRandom = null;

        for (int i = 0; i < 6; i++)     // 2페이즈는 몬스터 6마리
        {
            int type = Random.Range(0, 5);
            switch (type)
            {
                case 0:
                    createRandom = prefabFire; break;

                case 1:
                    createRandom = prefabWater; break;

                case 2:
                    createRandom = prefabElectric; break;

                case 3:
                    createRandom = prefabGrass; break;

                case 4:
                    createRandom = prefabGround; break;

                default: break;
            }

            var x_rand = Random.Range(-0.7f, 0.7f);
            var z_rand = Random.Range(-0.7f, 0.7f);
            Vector3 ranpos = new Vector3(x_rand, 0, z_rand);
            Vector3 yoffset = new Vector3(0, 0.1f, 0);

            Vector3 spawnPosition = ranpos + yoffset + hitPose.position;
            GameObject monster = Instantiate(createRandom, spawnPosition, Quaternion.identity); // prefab2P 몬스터 프리펩이 랜덤위치에 스폰
            Rigidbody rb = monster.GetComponent<Rigidbody>();
            spawnedObjects.Add(monster);
            monsterCount++;
        }
        created = true;  
    }
    void create3()
    {
        GameObject createRandom = null;

        for (int i = 0; i < 9; i++)      // 3페이즈는 몬스터 9마리
        {
            int type = Random.Range(0, 5);
            switch (type)
            {
                case 0:
                    createRandom = prefabFire; break;

                case 1:
                    createRandom = prefabWater; break;

                case 2:
                    createRandom = prefabElectric; break;

                case 3:
                    createRandom = prefabGrass; break;

                case 4:
                    createRandom = prefabGround; break;

                default: break;
            }

            var x_rand = Random.Range(-0.7f, 0.7f);
            var z_rand = Random.Range(-0.7f, 0.7f);
            Vector3 ranpos = new Vector3(x_rand, 0, z_rand);
            Vector3 yoffset = new Vector3(0, 0.1f, 0);

            Vector3 spawnPosition = ranpos + yoffset + hitPose.position;
            GameObject monster = Instantiate(createRandom, spawnPosition, Quaternion.identity);    // prefab3P 몬스터 프리펩이 랜덤위치에 스폰
            Rigidbody rb = monster.GetComponent<Rigidbody>();
            spawnedObjects.Add(monster);
            monsterCount++; 
        }
        created = true;  
    }


}
