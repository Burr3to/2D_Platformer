using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    [Header("Player")]
    Transform Player;
    [SerializeField] private LayerMask playerLayer;

    [Header("Health")]
    [SerializeField] private float MaxHealth;
    public float Maxhealth { get { return MaxHealth; } set { MaxHealth = value; } } 
   

    [SerializeField] private int EnemyScoreValue;
    public float currentHealth { get; private set; }


    [Header("Attack Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    private float cooldownTimer = Mathf.Infinity;


    [Header("Slider")]
    [SerializeField] private Slider slider;

    [Header("Object to disable on Boss Enrage")]
    [SerializeField] GameObject objectToDisableOnBossEnrage;

    [Header("Object to enable on Boss Enrage")]
    [SerializeField] GameObject ObjectToEnableOnBossEnrage; 

    Collider2D Collider;
    public bool isFlipped { get; private set; }
    private bool ReadytoSpawnLoot = false;

    //references
    private Health playerHealth;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    //on death text
    [SerializeField] private TextMeshProUGUI WinnerWinnerChickedDinner;

    //bordel

    public bool PlayerCanTakeDamage = false;
    private bool WalkAnimActivated;
    public bool IntroFinished { get; private set; }
    public bool DefendActive { get; set; }
    public bool EnragedState { get; private set; }

    private void Start()
    {

        Collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        slider.maxValue = MaxHealth;
        slider.value = MaxHealth;
        currentHealth = MaxHealth;

        WinnerWinnerChickedDinner.enabled = false;

    }

    private void Update()
    {

        PlayerCanTakeDamage = false;
        slider.value = currentHealth;

        if (PlayerInSight()) //
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
            }
        }
        Enrage();

    }

    private void Enrage()
    {
        if (currentHealth <= MaxHealth / 3 && !EnragedState)
        {
            float timeElapsed = 0;
            EnragedState = true;
            anim.SetTrigger("Defend");
            
            StartCoroutine(RegenerateHealth(3f));
            spriteRenderer.color = new Color(0.8f, 0.3811321f, 0.3811321f, 1);


            timeElapsed += Time.deltaTime;

            objectToDisableOnBossEnrage.SetActive(false);
            ObjectToEnableOnBossEnrage.SetActive(true);
        }
    }

    IEnumerator RegenerateHealth(float duration)
    {
        spriteRenderer.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 1.5f, 1.0f));

        float targetHealth = currentHealth + Maxhealth / 2;
        float timer = 0f;

        // until the timer reaches duration or the current health reaches the target health
        while (timer < duration && currentHealth < targetHealth)
        {
            
            currentHealth += 0.5f;

            // Clamp the current health so it does not exceed the target
            currentHealth = Mathf.Clamp(currentHealth, 0f, targetHealth);
            timer += Time.deltaTime;

            // Wait for one frame before continuing the loop
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "FireBall" || collision.gameObject.tag == "Skill" || collision.gameObject.tag == "Explosion") && DefendActive)
        {
            return;
        }
        else
        {
            if (collision.gameObject.tag == "FireBall")
            {
                TakeDamage(collision.gameObject.GetComponent<Projectile>().FireBallDamage);
            }
            else if (collision.gameObject.tag == "Skill")
            {
                TakeDamage(collision.gameObject.GetComponent<BlueSkill>().SkillDamage);
            }
            else if (collision.gameObject.tag == "Explosion") 
            {
                TakeDamage(collision.gameObject.GetComponent<Explosion_skill>().DamageBoss);
            }
        }


    }

    public void SetHealth(float value)
    {
        currentHealth += value;
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, MaxHealth);

        if (currentHealth > 0)
        {

        }
        else if (currentHealth == 0)
        {
            anim.SetTrigger("Death");
            ScoreManager.instance.ChangeScore(EnemyScoreValue);
            ReadytoSpawnLoot = true; 

            WinnerWinnerChickedDinner.enabled = true; //textmeshprougui
        }
  
    }
    public void PlayerCanBeDamaged()
    {
        PlayerCanTakeDamage = true;
    }
    public void IntroFinish()
    {
        IntroFinished = true;
    }
    public void WalkSwitch()
    {
        if (WalkAnimActivated) WalkAnimActivated = !WalkAnimActivated;
        else WalkAnimActivated = true;
    }
    public bool GetWalkAnim()
    {
        return WalkAnimActivated;
    }
    public void SetBossWalk()
    {
        anim.SetTrigger("Walk");
    }
    public void SetBossRun()
    {
        anim.SetTrigger("Run");
    }

    public bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(Collider.bounds.center + transform.right * transform.localScale.x * colliderDistance,
            new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y, Collider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Collider = GetComponent<Collider2D>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Collider.bounds.center + transform.right * transform.localScale.x * colliderDistance,
            new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y, Collider.bounds.size.z));
    }

    public void LookAtPlayer()
    {

        // If the player is to the left of the object, flip the objects scale
        if (Player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        // If the player is to the right of the object, flip the objects scale
        else if (Player.position.x > transform.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void DeactivateAfterDeath()
    {
        gameObject.SetActive(false);
    }
}
