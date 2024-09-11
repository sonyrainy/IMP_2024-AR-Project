using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RESTART : MonoBehaviour
{
    public TMP_Text scoreText;
    scoreController scoreController;
    public void restart()
    {
        scoreController = FindObjectOfType<scoreController>();
        scoreController.refresh();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);

    }
}
    