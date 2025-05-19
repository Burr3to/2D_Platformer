using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    Rigidbody2D rigidbody2D;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player ran off the edge

    [Header("Extra Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    public int extraJumpstoOtherSc { get; private set; }

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask AirObjects;
    [SerializeField] private LayerMask wallLayer;


    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;


    [Header("Dash things")]
    [SerializeField] private float DashSpeed = 15f;
    [SerializeField] private float DashLength = .3f;
    [SerializeField] private float DashBufferLength = .1f;
    [SerializeField] private float DashCooldown;
    private float DashTimer = Mathf.Infinity;
    private float DashBufferCounter;
    private bool isDashing;
    private bool hasDashed;
    private bool canDash => DashBufferCounter > 0f && !hasDashed;
    private Vector2 dir;


    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }
    
    private void Start()
    {

        if (InGameMenu.RestartDefault) //if restart button
        {

            if (SceneManager.GetActiveScene().name.ToString() == "Level1")
            {
                transform.position = new Vector3(-1.04f, -2.97f, 0);
            }

            if (SceneManager.GetActiveScene().name.ToString() == "Level2")
            {             
                transform.position = new Vector3(-9.5f, -2.1f, 0);
            }

            if (SceneManager.GetActiveScene().name.ToString() == "Level3")
            {
                transform.position = new Vector3(-9.27f, -3.79f, 0);
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                Vector3 level1Pos = new Vector3(PlayerPrefs.GetFloat("Level1PosX"), PlayerPrefs.GetFloat("Level1PosY"),
                    PlayerPrefs.GetFloat("Level1PosZ"));
                transform.position = level1Pos;
                ScoreManager.instance.LoadData();
                Camera.main.GetComponent<CameraController>().MoveToSavedRoom();
            }
            else if (SceneManager.GetActiveScene().name == "Level2")
            {
                Vector3 level2Pos = new Vector3(PlayerPrefs.GetFloat("Level2PosX"), PlayerPrefs.GetFloat("Level2PosY"),
                    PlayerPrefs.GetFloat("Level2PosZ"));
                extraJumps = PlayerPrefs.GetInt("extraJumps");

                transform.position = level2Pos;
                ScoreManager.instance.LoadData();

                
                Camera.main.GetComponent<CameraController>().MoveToSavedRoom();
            }
            else if (SceneManager.GetActiveScene().name == "Level3")
            {
                Vector3 level3Pos = new Vector3(PlayerPrefs.GetFloat("Level3PosX"), PlayerPrefs.GetFloat("Level3PosY"),
                    PlayerPrefs.GetFloat("Level3PosZ"));
                extraJumps = PlayerPrefs.GetInt("extraJumps");
                    ScoreManager.instance.LoadData();
                transform.position = level3Pos;
            }

            

            extraJumps = PlayerPrefs.GetInt("extraJumps");
            //InGameMenu.RestartDefault = false;
        }

        
       

    }


    private void Update()
    {
        extraJumpstoOtherSc = extraJumps;
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);


        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerPrefs.DeleteAll();
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
            Jump();

        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter >= 1)
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            jumpCounter--;
        }

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);



        if (onWall())
        {
            if (onWall() && Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();  
            }
            body.gravityScale = 6f;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
                jumpCounter = extraJumps; //Reset jump counter to extra jump value
            }
            else
                coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
        }


        //DASH
        if (Input.GetKeyDown(KeyCode.Q) && !isDashing && DashTimer > DashCooldown)
        {
            //anim.SetBool("isDashing",true);    dorobit dnes 6.1
            DashBufferCounter = DashBufferLength;
            DashTimer = 0;
            StartCoroutine(Dash());

        }
        else
        {
            DashBufferCounter -= Time.deltaTime;
        }

        DashTimer += Time.deltaTime;
    }
    //coyote sa nemoze od ground pocitat
    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;
        //If coyote counter is 0 or less and not on the wall and don't have any extra jumps don't do anything

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
        {
            //WallJump();
        }
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                //If not on the ground and coyote counter bigger than 0 do a normal jump
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0) //If we have extra jumps then jump and decrease the jump counter
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            //Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }

    IEnumerator Dash()
    {
        float dashStartTime = Time.time;
        hasDashed = true;
        isDashing = true;
        anim.SetBool("isDashing", true);
        anim.Play("Dash");
        


        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.gravityScale = 0f;
        rigidbody2D.drag = 0f;

        //direction of the dash based on the local scale of the object
        if (transform.localScale.x == 1)
        {
            dir = new Vector2(1f, 0f);
        }
        else if (transform.localScale.x == -1)
        {
            dir = new Vector2(-1f, 0f);
        }

        // Perform the dash for the specified duration
        while (Time.time < dashStartTime + DashLength)
        {
            rigidbody2D.velocity = dir.normalized * DashSpeed;
            yield return null; // Wait for the next frame

        }

        anim.SetBool("isDashing", false);
        isDashing = false;
    }

    private void WallJump()
    {
        //body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        isWallJumping = true;
        wallJumpingCounter = 0f;
        wallJumpingDirection = Mathf.Sign(transform.localScale.x);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        //Debug.Log("Ground");
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && !onWall();
    }

    public bool AddExtraJumps()
    {
        extraJumps++;
        PlayerPrefs.SetInt("extraJumps", extraJumps);
        return true;
    }



}