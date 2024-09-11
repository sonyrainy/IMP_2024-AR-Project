using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Raycast : MonoBehaviour    // 플레이어의 터치 raycast 기능 스크립트
{
    public LayerMask monsterMask;   // 몬스터 레이어

    private float range = 20f;
    private bool touched;   // 터치했는가?
    private float damage = 10f;
    private float effective = 1.0f;
    private string currentSkill;

    public ParticleSystem hitFire; // 맞았을 때 재생할 이펙트 프리팹
    public ParticleSystem hitWater;
    public ParticleSystem hitElectric;
    public ParticleSystem hitGround;
    public ParticleSystem hitGrass;

    public AudioSource fireSound;
    public AudioSource waterSound;
    public AudioSource electricSound;
    public AudioSource groundSound;
    public AudioSource grassSound;
    public AudioSource deffenseSound;

    private void Awake()
    {

    }

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)   // 터치하면
        {
            touched = true; // true로
        }

    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (touched && Physics.Raycast(ray, out hit, range, monsterMask))   // 몬스터를 터치한거라면,
        {
            if (hit.collider.tag == "fire")
            {
                
                switch (CurrentSkill.currentSkill)
                {
                    case "water":
                        effective = 2.0f; break;

                    case "grass":
                        effective = 0.5f; break;

                    default: break;
                }
            }
            if (hit.collider.tag == "water")
            {
                switch (CurrentSkill.currentSkill)
                {
                    case "electric":
                        effective = 2.0f; break;

                    case "fire":
                        effective = 0.5f; break;

                    default: break;
                }
            }
            if (hit.collider.tag == "electric")
            {
                switch (CurrentSkill.currentSkill)
                {
                    case "ground":
                        effective = 2.0f; break;

                    case "water":
                        effective = 0.5f; break;

                    default: break;
                }
            }
            if (hit.collider.tag == "ground")
            {
                switch (CurrentSkill.currentSkill)
                {
                    case "grass":
                        effective = 2.0f; break;

                    case "electric":
                        effective = 0.5f; break;

                    default: break;
                }
            }
            if (hit.collider.tag == "grass")
            {
                switch (CurrentSkill.currentSkill)
                {
                    case "fire":
                        effective = 2.0f; break;

                    case "ground":
                        effective = 0.5f; break;

                    default: break;
                }
            }

            damage *= effective;
            if (hit.collider.gameObject.GetComponent<monster2S>().isDeffense == false)
            {
                hit.collider.gameObject.GetComponent<monster2S>().hp -= damage;
                hit.collider.gameObject.GetComponent<HPUI>().HpSlider.value -= damage;

                Debug.Log(damage + " 만큼의 데미지를 주었습니다!");
                switch (CurrentSkill.currentSkill)
                {
                    case "fire":
                        fireSound.Play();
                        break;
                    case "water":
                        waterSound.Play();
                        break;
                    case "electric":
                        electricSound.Play();
                        break;
                    case "grass":
                        grassSound.Play();
                        break;
                    case "ground":
                        groundSound.Play();
                        break;

                }
                PlayHitEffect(hit.point); // 충돌 지점을 전달하여 맞았을 때 이펙트를 재생합니다.
            }
            else if (hit.collider.gameObject.GetComponent<monster2S>().isDeffense == true)
            {
                Debug.Log("상대가 방어상태입니다!");
                deffenseSound.Play();
            }
            damage = 10f;
            effective = 1.0f;


        }




        touched = false;    // 터치 여부 초기화   
    }


    private void PlayHitEffect(Vector3 position)
    {
        // 이펙트를 생성하고 지정된 위치에 재생합니다.
        switch (CurrentSkill.currentSkill)
        {
            case "fire":
                Instantiate(hitFire, position, Quaternion.identity); break;
                Debug.Log("불이 나가요");

            case "water":
                Instantiate(hitWater, position, Quaternion.identity); break;

            case "electric":
                Instantiate(hitElectric, position, Quaternion.identity); break;

            case "grass":
                Instantiate(hitGrass, position, Quaternion.identity); break;

            case "ground":
                Instantiate(hitGround, position, Quaternion.identity); break;

            default: break;
        }

    }
}
