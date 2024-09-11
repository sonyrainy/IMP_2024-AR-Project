using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class monster2S : MonoBehaviour  // 몬스터 프리펩에 들어갈 스크립트
{
    [HideInInspector]
    public float hp;    // 몬스터 HP
                        // 몬스터가 죽을 때 재생할 AudioSource
    public AudioClip deathSound;




    private float range = 1f;   // 몬스터 아래방향 ray의 사거리
    PlaneMonsterSpawner1 MonsterSpawner1;   // PlaneMonsterSpawner1 스크립트 받기위한 변수
    PlaneMonsterSpawner2 MonsterSpawner2;
    PlaneMonsterSpawner3 MonsterSpawner3;

    public LayerMask planeMask;     // plane 마스크
    scoreController scoreController; // 점수 관리

    private float minTime = 2f;
    private float maxTime = 4f;

    private Renderer rend;
    private float timer;
    private Color originalColor;
    public bool isDeffense = false;

    private float speed = 0.1f;
    private float changeDirectionInterval = 2f;

    private Vector3 targetDirection;
    private float timer2;

    private Rigidbody rb;

    private Transform playerCamera; // 플레이어의 메인 카메라
    void Awake()
    {
        hp = 60;    // hp 10으로 초기설정
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            MonsterSpawner1 = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlaneMonsterSpawner1>();  //  "XR Origin (XR Rig)" 오브젝트 안의 PlaneMonsterSpawner1 스크립트를 가져옴

        }
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            MonsterSpawner2 = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlaneMonsterSpawner2>();  //  "XR Origin (XR Rig)" 오브젝트 안의 PlaneMonsterSpawner1 스크립트를 가져옴

        }
        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            MonsterSpawner3 = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlaneMonsterSpawner3>();  //  "XR Origin (XR Rig)" 오브젝트 안의 PlaneMonsterSpawner1 스크립트를 가져옴

        }

        //  rend = GetComponent<Renderer>();
        //  originalColor = rend.material.color;
        timer = Random.Range(minTime, maxTime);

        SetRandomDirection();
        timer2 = changeDirectionInterval;
    }

    private void Start()
    {
        scoreController = FindObjectOfType<scoreController>();
        rb = GetComponent<Rigidbody>();
        transform.Find("shield").gameObject.SetActive(false);
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        if (playerCamera != null)
        {
            // 몬스터와 카메라의 위치 차이를 구합니다.
            Vector3 direction = playerCamera.position - transform.position;
            // y축 방향의 값을 유지하고 나머지 축의 값을 제거합니다.
            direction.y = 0f;
            // 방향 벡터를 회전값으로 변환합니다.
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // 몬스터의 회전 값을 조정합니다.
            transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
        }

        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                if (isDeffense)
                {
                    // rend.material.color = originalColor;
                    transform.Find("shield").gameObject.SetActive(false);

                }
                else
                {
                    //  rend.material.color = Color.gray;
                    transform.Find("shield").gameObject.SetActive(true);
                }
                isDeffense = !isDeffense;
                timer = Random.Range(minTime, maxTime);
            }

        }

        if (SceneManager.GetActiveScene().buildIndex == 7 || SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            timer2 -= Time.deltaTime;

            if (timer2 <= 0f)
            {
                SetRandomDirection();
                timer2 = changeDirectionInterval;
            }

            transform.Translate(targetDirection * speed * Time.deltaTime);
        }




        if (hp <= 0)
        {     // 몬스터 HP가 0이되면..
            Debug.Log("monster die");
            SoundManager.instance.SFXPlay("dieSound", deathSound);
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                MonsterSpawner1.monsterCount -= 1;  // 몬스터가 죽었으므로 monsterCount -1

            }
            else if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                MonsterSpawner2.monsterCount -= 1;  // 몬스터가 죽었으므로 monsterCount -1

            }
            else if (SceneManager.GetActiveScene().buildIndex == 7)
            {
                MonsterSpawner3.monsterCount -= 1;  // 몬스터가 죽었으므로 monsterCount -1

            }


            scoreController.GetScore();
            Destroy(this.gameObject);   // 해당 몬스터 destroy
        }
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);   // 추락을 감지하기 위한 몬스터 아래방향의 ray
        RaycastHit hit;


        if (!Physics.Raycast(ray, out hit, range, planeMask))   // ray가 plane과 닿지않는다면..
        {
            var x_rand = Random.Range(-0.5f, 0.5f);
            var z_rand = Random.Range(-0.5f, 0.5f);
            Vector3 ranpos = new Vector3(x_rand, 0, z_rand);
            Vector3 yoffset = new Vector3(0, 0.1f, 0);
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                gameObject.transform.rotation = Quaternion.identity;
                Vector3 respawnPosition = ranpos + yoffset + MonsterSpawner1.hitPose.position;  // 스테이지 시작시 터치했던 plane 주변의 랜덤 스폰 장소
                gameObject.transform.position = respawnPosition;    // 위치를 그곳으로 다시 옮겨서 추락을 방지
                Debug.Log("Monster respawn.");
            }
            else if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                gameObject.transform.rotation = Quaternion.identity;
                Vector3 respawnPosition = ranpos + yoffset + MonsterSpawner2.hitPose.position;  // 스테이지 시작시 터치했던 plane 주변의 랜덤 스폰 장소
                gameObject.transform.position = respawnPosition;    // 위치를 그곳으로 다시 옮겨서 추락을 방지
                Debug.Log("Monster respawn.");
            }
            else if (SceneManager.GetActiveScene().buildIndex == 7)
            {

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                gameObject.transform.rotation = Quaternion.identity;
                Vector3 respawnPosition = ranpos + yoffset + MonsterSpawner3.hitPose.position;  // 스테이지 시작시 터치했던 plane 주변의 랜덤 스폰 장소
                gameObject.transform.position = respawnPosition;    // 위치를 그곳으로 다시 옮겨서 추락을 방지
                Debug.Log("Monster respawn.");
            }

        }
    }

    void SetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        targetDirection = new Vector3(randomX, 0f, randomZ).normalized;
    }


}
