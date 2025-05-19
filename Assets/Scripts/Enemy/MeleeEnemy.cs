using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private double damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private EnemyPatrol PatrolParentObject;

    private float DistanceLeft;
    private float DistanceRight;

    //References
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;
    Vector3 DistLeft;
    Vector3 DistRight;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        PatrolParentObject = GetComponent<EnemyPatrol>();

    }

    private void Update()
    {
        DistanceLeft = Vector2.Distance(transform.position, leftEdge.position);
        DistanceRight = Vector2.Distance(transform.position, rightEdge.position);

        DistLeft = new Vector3(DistanceLeft / 2, 0, 0);
        DistRight = new Vector3(DistanceRight / 2, 0, 0);

        //Debug.Log(DistanceLeft);

        cooldownTimer += Time.deltaTime;


        //Attack only when player in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if (PlayerInLongSightLeft() || PLayerInLongSightRight())  // when player is far from enemy he speeds up
        {
            enemyPatrol.speedForScripts = 5;
        }
        else enemyPatrol.speedForScripts = 2;




        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();

        
    }

    private bool PlayerInSight()
    {
        //Debug.Log("Player");
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private bool PLayerInLongSightRight()
    {
        RaycastHit2D ISeePlayerButCantAttack;

        if (!enemyPatrol.movingLeft) //moving right tak chcem druhy raycast do druheho bodu (rightpoint)
        {
            
            ISeePlayerButCantAttack = Physics2D.BoxCast(boxCollider.bounds.center + rightEdge.position - transform.position - DistRight,
            new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.right, DistanceRight, playerLayer);

            return ISeePlayerButCantAttack.collider != null;
        }


        return false; // ISeePlayerButCantAttack.collider == null;

    }
    private bool PlayerInLongSightLeft()
    {
        RaycastHit2D ISeePlayerButCantAttack;

        if (enemyPatrol.movingLeft)
        {
            ISeePlayerButCantAttack = Physics2D.BoxCast(boxCollider.bounds.center + leftEdge.position - transform.position + DistLeft,
            new Vector3(DistanceLeft, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left,DistanceLeft, playerLayer);

            return ISeePlayerButCantAttack.collider != null;
        }
 
        return false; // ISeePlayerButCantAttack.collider == null;
    }
    public bool CheckIfPlayerBehind()
    {
        RaycastHit2D ISeePlayerButCantAttack;

        if (enemyPatrol.movingLeft)
        {
            ISeePlayerButCantAttack = Physics2D.BoxCast(boxCollider.bounds.center + rightEdge.position - transform.position - DistRight,
            new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.right, DistanceRight, playerLayer);

            return ISeePlayerButCantAttack.collider != null;
        }
        else if (!enemyPatrol.movingLeft)
        {
            ISeePlayerButCantAttack = Physics2D.BoxCast(boxCollider.bounds.center + leftEdge.position - transform.position + DistLeft,
            new Vector3(DistanceLeft, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, DistanceLeft, playerLayer);

            return ISeePlayerButCantAttack.collider != null;
        }

        return false; // ISeePlayerButCantAttack.collider == null
    }
  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.color = Color.blue; //right
        Gizmos.DrawWireCube(boxCollider.bounds.center + rightEdge.position - transform.position - DistRight,
            new Vector3(DistanceRight, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.color = Color.cyan; //left
        Gizmos.DrawWireCube(boxCollider.bounds.center + leftEdge.position - transform.position + DistLeft,
            new Vector3(DistanceLeft, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

    }
    private void DamagePlayer() //for animation use
    {
        if (PlayerInSight())
            playerHealth.TakeDamage((float)damage);
    }
}