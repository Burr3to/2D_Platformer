using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI;
using System.Data;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI cashUI;
    public int Cash;
    public int Score;

    public int Skore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
       
        //asdaas
        
        if (InGameMenu.RestartDefault)
        {
            Score = 0;
            Cash = 0;
            scoreUI.text = Score.ToString();
            cashUI.text = Cash.ToString();
            //InGameMenu.RestartDefault = false;
        }
        else
        {
            if (PlayerPrefs.HasKey("Score") || PlayerPrefs.HasKey("Cash")) LoadData();
        }
    }
    public void SaveData()
    {
        Skore = Score;
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.SetInt("Cash", Cash);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {

        if (PlayerPrefs.HasKey("Score"))
        {

            Score = PlayerPrefs.GetInt("Score");
            scoreUI.text = PlayerPrefs.GetInt("Score").ToString();
        }

        if (PlayerPrefs.HasKey("Cash"))
        {

            Cash = (PlayerPrefs.GetInt("Cash"));
            cashUI.text = PlayerPrefs.GetInt("Cash").ToString();
        }
        
        //InGameMenu.RestartDefault = false;
    }

    public void ChangeScore(int value)
    {
        if (value == 0) Score = 0;
        else Score += value;

        PlayerPrefs.SetInt("Score", Score);
        scoreUI.text = Score.ToString();
       
    }

    public int GetScore()
    {
        return Score;
    }
    //cash

    public void ChangeCash(int value)
    {
        if (value == 0) Cash = 0;
        else Cash += value;

        cashUI.text = Cash.ToString();
        
    }

    public int GetCash()
    {
        return Cash;
    }

}
