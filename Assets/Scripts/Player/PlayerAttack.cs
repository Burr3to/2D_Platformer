using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerAttack : MonoBehaviour
{
    [Header("After Death")]
    [SerializeField] GameObject MenuAfterDeath;
    [SerializeField] InGameMenu inGameMenu;

    [Header("Fireballs aka basic aa")]
    [SerializeField] public float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;
    private float startingAttackCooldown = 0.3f;
    public bool StartTimer { get; private set; }

    [Header("Blueballs aka skills")]
    [SerializeField] public float SkillReleaseTime;
    [SerializeField] public float ChargeSpeed;
    [SerializeField] public float ChargeTime;
    [SerializeField] private Transform BlueSkillPoint;
    [SerializeField] private GameObject[] blueSkillballs;
    //[SerializeField] private Animator charging;


    [Header("Explosion aka r skill xD")]
    [SerializeField] private GameObject Explosion;
    [SerializeField] private GameObject ExplosionUI;
    [SerializeField] public float InstantiateExplosionTime;
    public bool ExplosionPurchased { get; set; }
    public float chargeTimeExplosion { get; private set; }
    public bool Explodebool { get; private set; }


    private Health health;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity; //ked to bude velka hodnota tak to bude akoky nekoneèno

    private void Awake()
    {

        MenuAfterDeath.SetActive(false);
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        StartTimer = false;
        PlayerPrefs.Save();

    }

    private void Start()
    {

        if (InGameMenu.RestartDefault)
        {
            attackCooldown = 0.3f;
            //InGameMenu.RestartDefault = false;
            ScoreManager.instance.ChangeCash(0);
            ScoreManager.instance.ChangeScore(0);
            
        }
        else
        {
            if (PlayerPrefs.HasKey("attackCooldown")) attackCooldown = PlayerPrefs.GetFloat("attackCooldown");
            else
                attackCooldown = 0.3f;

            
            
            if (PlayerPrefs.HasKey("Cash") && PlayerPrefs.HasKey("Score"))
            {
               ScoreManager.instance.LoadData();
            }


            if (PlayerPrefs.GetInt("ExplosionPurchased") == 1)
            {
                ExplosionPurchased = true;
                ExplosionUI.SetActive(true);
            }
            else PlayerPrefs.SetInt("ExplosionPurchased", 0);

            InGameMenu.RestartDefault = false;
        }
    }


    private void Update()
    {
        //Debug.Log(PlayerPrefs.GetInt("ExplosionPurchased"));

        //after death
        if (health.ReadytoSpawnLoot && health.canOpenDeathMenu)
        {
            MenuAfterDeath.SetActive(true);
            //inGameMenu.SwitchPause();
            health.canOpenDeathMenu = false;
        }
        //2
        {
            if (Input.GetKeyDown(KeyCode.PageUp)) ScoreManager.instance.ChangeCash(500);
            if (Input.GetKeyDown(KeyCode.PageDown)) ScoreManager.instance.ChangeScore(500);
            if (Input.GetKeyDown(KeyCode.End)) GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().AddExtraJumps();
            if (Input.GetKeyDown(KeyCode.Home))
            {
                ExplosionPurchased = true;
                ExplosionUI.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                ScoreManager.instance.LoadData();
                ScoreManager.instance.ChangeScore(ScoreManager.instance.Skore);
            }
        }

        //basic aa
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack()) //upravit ked budem chciet bezat a strilat
            Attack();

        //skill
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChargeTime = 0;
            StartTimer = true;
        }
        else if (Input.GetKeyUp(KeyCode.E) && (ChargeTime <= SkillReleaseTime || ChargeTime >= SkillReleaseTime)) //aj ked je cas splneny aj ked nie
        {
            if (ChargeTime >= SkillReleaseTime)
            {
                Skill1();
            }
            ChargeTime = 0;
            StartTimer = false;
        }

        //Explosion skill
        if (Input.GetKeyDown(KeyCode.R) && ExplosionPurchased)
        {
            chargeTimeExplosion = 0;
            Explodebool = true;
        }

        if (chargeTimeExplosion >= InstantiateExplosionTime && Explodebool)
        {
            GameObject clone = Instantiate(Explosion, playerMovement.transform.position, Quaternion.identity);
            health.TakeDamage(0.5f);
            chargeTimeExplosion = 0;
            Explodebool = false;
        }


        //r skill
        chargeTimeExplosion += Time.deltaTime;

        //e skill
        cooldownTimer += Time.deltaTime;
        ChargeTime += Time.deltaTime * ChargeSpeed;
    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private void Skill1()
    {
        StartTimer = false;
        anim.SetTrigger("attack");

        blueSkillballs[FindBlueball()].transform.position = BlueSkillPoint.position;
        blueSkillballs[FindBlueball()].GetComponent<BlueSkill>().SetDirection(Mathf.Sign(transform.localScale.x));


    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private int FindBlueball()
    {
        for (int i = 0; i < blueSkillballs.Length; i++)
        {
            if (!blueSkillballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    public void RaiseAttackSpeed()
    {
        if (attackCooldown > 0.19f)
        {
            attackCooldown -= 0.04f;
            
            PlayerPrefs.Save();
        }
        PlayerPrefs.SetFloat("attackCooldown", attackCooldown);
    }
}