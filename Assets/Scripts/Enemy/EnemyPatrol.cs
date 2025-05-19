using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;
    [SerializeField] private MeleeEnemy meleeEnemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    public float speedForScripts { get { return speed; } set { speed = value; } }  //na pouzitie v inom scripte
    private Vector3 enemyScale;
    public bool movingLeft { get; private set; }

    private void Awake()
    {
        enemyScale = enemy.localScale;
    }
    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (meleeEnemy.CheckIfPlayerBehind())
        {
            movingLeft = !movingLeft; // Change direction
        }

        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x) // Check if the enemy has not reached the left edge
                MoveInDirection(-1);
            else
                DirectionChange(); // If the enemy has reached the left edge, change direction
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void DirectionChange() // This function changes the direction of the enemy and resets the idle timer
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    public void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);

        //Make Transform face direction
        enemy.localScale = new Vector3(Mathf.Abs(enemyScale.x) * _direction,
            enemyScale.y, enemyScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}