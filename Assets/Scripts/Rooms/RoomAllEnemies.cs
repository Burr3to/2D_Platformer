using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomAllEnemies : MonoBehaviour
{

    private float EnemiesCount;
    public float DeadEnemies { get; private set; }
    public bool AllDead { get; private set; }
    public bool ShowText { get; set; }
    public bool LevelFailed { get; private set; }
    public bool StartTimer { get; set; }

    public List<GameObject> enemies { get; private set; }

    [Header("Collider of Exit")]
    [SerializeField] private GameObject EnterDoorSprite;
    [SerializeField] private GameObject ExitDoorSprite;

    [Header("UI text")]
    [SerializeField] TextMeshProUGUI ExitText;
    [SerializeField] TextMeshProUGUI Timer;
    [SerializeField] public GameObject Congratulations;

    [Header("Time for this room")]
    [SerializeField] private float TimeForRoom;

    [Header("UI Objects")]
    [SerializeField] GameObject TimerObject;
    [SerializeField] GameObject ExitTextObject;


    private Health health;
    private void Awake()
    {
        enemies = new List<GameObject>();
        ShowText = false;
        Congratulations.SetActive(false);
    }

    private void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        FindEnemies(transform);
       

        EnemiesCount = enemies.Count;
    }
    // UI you need to kill all enemies UI in room
    //
    private void FindEnemies(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.tag == "Enemy")
            {
                enemies.Add(child.gameObject);
            }
            FindEnemies(child);
        }
    }
    private void Update()
    {

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (!enemies[i].activeSelf)
            {
                enemies.RemoveAt(i);
                DeadEnemies++;
            }
        }


        if (enemies.Count == 0)
        {
            AllDead = true;
            EnterDoorSprite.SetActive(false);
            ExitDoorSprite.SetActive(false);
        }


        if (ShowText == true)
        {
            TimerObject.SetActive(true);
            ExitTextObject.SetActive(true);
        }
        else
        {
            TimerObject.SetActive(false);
            ExitTextObject.SetActive(false);
        }

        //timer

        if (StartTimer && !AllDead)
        {
            Timer.text = Mathf.Clamp(
                Mathf.RoundToInt(TimeForRoom -= Time.deltaTime),
                0,
                float.MaxValue).ToString();
        }


        
        if (Timer.text.ToString() != "0" && AllDead)
        {
           Congratulations.SetActive(true);
        }

        if (Timer.text.ToString() == "0" && LevelFailed == false && enemies.Count > 0)
        {
            LevelFailed = true;
            health.TakeDamage(1);
        }


    }
}
