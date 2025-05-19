using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_shop : MonoBehaviour
{

    public GameObject player;

    [Header("Shop stuff")]
    [SerializeField] private AudioClip pickupSound;
    public Transform shopMenu;
    float score, cash;



    [SerializeField]
    TextMeshProUGUI MaxHealth_price,
        RegenHealth_price,
        AttackSpeed_price,
        Multijump_price,
        Explosion_price;

    [SerializeField] GameObject ExplosionSkillUI;

    Health health;
    PlayerAttack attack;
    PlayerMovement playerMovement;

    private bool ExplosionSpellAlreadyActive;

    private void Awake()
    {
        shopMenu.gameObject.SetActive(false);  //start with shopmenu hidden
        ExplosionSkillUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        health = player.GetComponent<Health>();
        attack = player.GetComponent<PlayerAttack>();
        playerMovement = player.GetComponent<PlayerMovement>();
        score = ScoreManager.instance.GetScore();
        cash = ScoreManager.instance.GetCash();
    }

    public void AddMaxHealth() 
    {
        
        if (cash >= float.Parse(MaxHealth_price.text) && health.currentHealth < 10)  //opravit donekonecna
        {
            health.AddMaxHealth();
            SoundManager.instance.PlaySound(pickupSound);
            ScoreManager.instance.ChangeCash(-int.Parse(MaxHealth_price.text));
        }
        
    }

    public void AddHealth()
    {
       
        if (cash >= float.Parse(RegenHealth_price.text) && health.currentHealth < health.MaxHealthTotal)
        {
            
            SoundManager.instance.PlaySound(pickupSound);
            health.AddHealth(1);
            ScoreManager.instance.ChangeCash(-int.Parse(RegenHealth_price.text));
        }
        

    }

    public void RaiseAttackSpeed() //pridat zvuk?
    {
       
        if (cash >= float.Parse(AttackSpeed_price.text))
        {
           
            attack.RaiseAttackSpeed();
            SoundManager.instance.PlaySound(pickupSound);
            ScoreManager.instance.ChangeCash(-int.Parse(AttackSpeed_price.text));
        }
        
    }

    public void AddExtraJumps()
    {
        
        if (cash >= float.Parse(Multijump_price.text) && playerMovement.extraJumpstoOtherSc < 3)
        {
            playerMovement.AddExtraJumps();
            SoundManager.instance.PlaySound(pickupSound);
            ScoreManager.instance.ChangeCash(-int.Parse(Multijump_price.text));
        }
    }

    public void ActivateExplosionSkill()
    {
        if (cash >= float.Parse(Explosion_price.text) && !ExplosionSpellAlreadyActive)
        {
            SoundManager.instance.PlaySound(pickupSound);
            ExplosionSpellAlreadyActive = true;
            ExplosionSkillUI.SetActive(true);
            attack.ExplosionPurchased = true;
            PlayerPrefs.SetInt("ExplosionPurchased", 1);
            ScoreManager.instance.ChangeCash(-int.Parse(Explosion_price.text));
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shopMenu.gameObject.SetActive(true);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shopMenu.gameObject.SetActive(false);
        }
    }


}
