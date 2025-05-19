using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{

    public bool isPaused = false;
    [SerializeField] public GameObject pauseUI; //parent ingamemenu
    [SerializeField] private GameObject MenuAfterDeath;
    PlayerRespawn playerRespawn;  //na hracovi
    public bool CheckpointActivated { get; set; }
    public static bool RestartDefault;
    private bool PauseMenuOnLoad;

    private void Start()
    {
        playerRespawn = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRespawn>();

        Time.timeScale = (Time.timeScale == 0) ? 1 : Time.timeScale;

        if (PauseMenuOnLoad)
        {
            pauseUI.SetActive(true);
            PauseMenuOnLoad = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPause();
        }
    }
    
    public void SwitchPause()
    {
       
        pauseUI.SetActive(!pauseUI.activeSelf);
        Time.timeScale = Time.timeScale == 1 ? 0 : 1; //short if ( .. == 1) then 0 else 1
        
    }
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadCheckpoint()
    {
        if (CheckpointActivated)
        {
            PauseMenuOnLoad = true;
            //PlayerPrefs.SetString("PauseMenuOnLoad", PauseMenuOnLoad.ToString());

            playerRespawn.Respawn();
            MenuAfterDeath.SetActive(false);
        }
        
    }
    public void Restart() //add restart save
    {
        RestartDefault = true;
        playerRespawn.load = true;
        if (SceneManager.GetActiveScene().name.ToString() == "Level1")
        {
            SceneManager.LoadScene("Level1");
        }

        if (SceneManager.GetActiveScene().name.ToString() == "Level2")
        {
            SceneManager.LoadScene("Level2");
        }

        if (SceneManager.GetActiveScene().name.ToString() == "Level3")
        {
            SceneManager.LoadScene("Level3");
        }


    }
    public void Save()
    {
        
        PlayerPrefs.SetString("SavedLevel",SceneManager.GetActiveScene().name.ToString());

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            PlayerPrefs.SetFloat("Level1PosX", playerRespawn.transform.position.x);
            PlayerPrefs.SetFloat("Level1PosY", playerRespawn.transform.position.y);
            PlayerPrefs.SetFloat("Level1PosZ", playerRespawn.transform.position.z);
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            PlayerPrefs.SetFloat("Level2PosX", playerRespawn.transform.position.x);
            PlayerPrefs.SetFloat("Level2PosY", playerRespawn.transform.position.y);
            PlayerPrefs.SetFloat("Level2PosZ", playerRespawn.transform.position.z);
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            PlayerPrefs.SetFloat("Level3PosX", playerRespawn.transform.position.x);
            PlayerPrefs.SetFloat("Level3PosY", playerRespawn.transform.position.y);
            PlayerPrefs.SetFloat("Level3PosZ", playerRespawn.transform.position.z);
        }


        PlayerPrefs.SetInt("Score",ScoreManager.instance.Score);
        PlayerPrefs.SetInt("Cash", ScoreManager.instance.Cash);
        PlayerPrefs.Save();

        Debug.Log("Saved");
    }
    public void Options()
    {
        SceneManager.LoadScene("Options");  //dat ma main menu a zapnut options
    }

    public void Exit()
    {
        Application.Quit();
    }
}
