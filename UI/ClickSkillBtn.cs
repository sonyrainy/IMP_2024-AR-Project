using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClickSkillBtn : MonoBehaviour
{
    public AudioSource fire;
    public AudioSource water;
    public AudioSource grass;
    public AudioSource electric;
    public AudioSource ground;

    public GameObject panelfire;
    public GameObject panelwater;
    public GameObject panelelectric;
    public GameObject panelgrass;
    public GameObject panelground;


    private void Start()
    {
        panelfire.SetActive(true);
        panelwater.SetActive(true);
        panelelectric.SetActive(true);
        panelgrass.SetActive(true);
        panelground.SetActive(true);
    }

    public void OnClickFireBtn()
    {
        CurrentSkill.currentSkill = "fire";
        Debug.Log("현재 스킬: " + CurrentSkill.currentSkill);
        panelfire.SetActive(false);
        panelwater.SetActive(true);
        panelelectric.SetActive(true);
        panelgrass.SetActive(true);
        panelground.SetActive(true);
        //fire.Play();

    }
    public void OnClickWaterBtn()
    {
        CurrentSkill.currentSkill = "water";
        Debug.Log("현재 스킬: " + CurrentSkill.currentSkill);
        panelfire.SetActive(true);
        panelwater.SetActive(false);
        panelelectric.SetActive(true);
        panelgrass.SetActive(true);
        panelground.SetActive(true);

        // water.Play();
    }
    public void OnClickGrassBtn()
    {
        CurrentSkill.currentSkill = "grass";
        Debug.Log("현재 스킬: " + CurrentSkill.currentSkill);
        panelfire.SetActive(true);
        panelwater.SetActive(true);
        panelelectric.SetActive(true);
        panelgrass.SetActive(false);
        panelground.SetActive(true);
        // grass.Play();
    }
    public void OnClickElectricBtn()
    {
        CurrentSkill.currentSkill = "electric";
        Debug.Log("현재 스킬: " + CurrentSkill.currentSkill);
        panelfire.SetActive(true);
        panelwater.SetActive(true);
        panelelectric.SetActive(false);
        panelgrass.SetActive(true);
        panelground.SetActive(true);
        //electric.Play();
    }
    public void OnClickGroundBtn()
    {
        CurrentSkill.currentSkill = "ground";
        Debug.Log("현재 스킬: " + CurrentSkill.currentSkill);
        panelfire.SetActive(true);
        panelwater.SetActive(true);
        panelelectric.SetActive(true);
        panelgrass.SetActive(true);
        panelground.SetActive(false);

        //ground.Play();
    }
}
