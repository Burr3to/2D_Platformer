using UnityEngine;
using System.Collections;


public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth = 5;
    public float MaxHealthTotal { get; set; }
    public float currentHealth { get; private set; }

    [Header("Score value of enemy")]
    [SerializeField] public int EnemyScoreValue;  //toto treba presunut inam lebo je to aj na hracovi

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private Animator anim;
    public bool dead { get; set; }
    public bool ReadytoSpawnLoot = false;
    public bool canOpenDeathMenu { get; set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {

        if (InGameMenu.RestartDefault)  //default values
        {
            currentHealth = startingHealth;
            MaxHealthTotal = startingHealth;
            PlayerPrefs.DeleteKey("RestartOnPosZero");
            
        }
        else  //load
        {
            currentHealth = PlayerPrefs.GetFloat("currentHealth");
            MaxHealthTotal = PlayerPrefs.GetFloat("MaxHealthTotal");
            //InGameMenu.RestartDefault = false;
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("currentHealth", currentHealth);
        PlayerPrefs.SetFloat("MaxHealthTotal", MaxHealthTotal);
    }

    public void LoadData()
    {
        currentHealth =  PlayerPrefs.GetFloat("currentHealth");
        MaxHealthTotal = PlayerPrefs.GetFloat("MaxHealthTotal");
    }

    
    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, MaxHealthTotal);
        //currentHealth -= 1;
        Debug.Log("enemy health is : " + currentHealth);

        SaveData();

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead) // ak mrtvy
            {

                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;

                canOpenDeathMenu = true;
                ScoreManager.instance.ChangeScore(EnemyScoreValue);
                ReadytoSpawnLoot = true;
                anim.SetBool("grounded", true);
                anim.SetTrigger("die");

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }
    public void AddMaxHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth + 1, 0, 10); // 10 is max health total
        MaxHealthTotal += 1;
        SaveData();
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, MaxHealthTotal);
        SaveData();
    }
    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true); //player and enemy layer
        for (int i = 0; i < numberOfFlashes; i++) // Flash the sprite a certain number of times
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    //Respawn
    public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth); // dat MaxHealth ak restart budem riesit
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Invunerability());

        //Activate all attached component classes
        foreach (Behaviour component in components)
            component.enabled = true;
    }
}