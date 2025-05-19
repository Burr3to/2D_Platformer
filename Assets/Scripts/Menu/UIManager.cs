using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField] TextMeshProUGUI txtVictoryCondition;
    private float wincon = 0;

    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI cashUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else DestroyImmediate(this);
    }

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();

            return instance;
        }
    }
    public void ShowVictoryCondition()
    {

        txtVictoryCondition.enabled = true;
        txtVictoryCondition.text = "You need " + (wincon - ScoreManager.instance.GetScore())
            + " more Score points to continue to next Level (Kill all enemies)";
    }
    public void HideVictoryCondition()
    {
        txtVictoryCondition.enabled = false;
    }
  
    public void Finish(float value, string NextLevelName)
    {
        wincon = value;
        if (ScoreManager.instance.GetScore() >= value)
        {
            ScoreManager.instance.SaveData();
            SceneManager.LoadScene(NextLevelName);
            
        }
        else
        {
            ShowVictoryCondition();
        }
    }
}
