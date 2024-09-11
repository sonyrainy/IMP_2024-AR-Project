using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class scoreController : MonoBehaviour
{
    public TMP_Text scoreText;
    private static int score = 0;
    void Start()
    {
        displayText();
    }

    public void GetScore()
    {
        score += 100;
        displayText();
    }
    public void displayText()
    {
        scoreText.text = $"<color=#ff0000>{score:#,##0}</color>";
    }
    public void refresh() {
        score = 0;
    }
}
