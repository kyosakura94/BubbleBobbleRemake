using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    //Rigidbody2D rb;
    public Rigidbody2D rb2;

    public float speed;
    public float jumpForce;

    public bool isGrounded;
    private bool m_AirControl = false;
    private float m_MovementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask isGroundLayer;

    Animator animator;

    private bool m_FacingRight = true;

    public Rigidbody2D projectile;
    public float projectileForce;
    public Transform projectileSpawPoint;

    public float life;
    public bool isDead = false;

    public bool isFlyMode = false;
    private float gravity = -0.5f;
    public float flyTime;


    public Transform topTeleport;

    // Use this for initialization
    void Start() {

        animator = GetComponent<Animator>();

        rb2 = GetComponent<Rigidbody2D>();
        if (!rb2)
        {
            Debug.LogWarning("Rigidbody2D not found on " + name + ". Adding one by default");
            rb2 = gameObject.AddComponent<Rigidbody2D>();
        }

        if (speed <= 0 || speed > 5.0f)
        {
            speed = 5.0f;
            Debug.LogWarning("Speed not set on " + name + ". Defaulting to " + speed);
        }

        if (jumpForce <= 0 || jumpForce > 10.0f)
        {
            jumpForce = 10.0f;
            Debug.LogWarning("JumpForce not set on " + name + ". Defaulting to " + jumpForce);
        }

        if (!groundCheck)
        {
            groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();

        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.1f;

        }

        if (!projectile)
        {
            Debug.LogError("Projectile is not found on");
        }

        if (!projectileSpawPoint)
        {
            Debug.LogError("Projectile Spawn Point is not found on");
        }
        if (projectileForce <= 0)
        {
            projectileForce = 5.0f;
            Debug.LogWarning("LifeTime of projectile is not set. Default to" + projectileForce);
        }

        if (life <= 0)
        {
            life = 3.0f;
            Debug.LogWarning("LifeTime of life is not set. Default to" + life);
        }

        if (flyTime <= 0)
        {
            flyTime = 1.0f;
            Debug.LogWarning("LifeTime of flyTime is not set. Default to" + flyTime);
        }


    }

    // Update is called once per frame
    void Update() {

        float moveValue = Input.GetAxisRaw("Horizontal") * speed;

        animator.SetFloat("isRun", Mathf.Abs(moveValue));

        if (groundCheck)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        }

        if (isGrounded)
        {
            animator.SetBool("isJump", false);
        }

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb2.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetBool("isJump", true);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");

        }
        if (Input.GetButtonDown("Fire2"))
        {
           
            Debug.Log("Test");

            animator.SetBool("isFly", true);

            isFlyMode = true;
            StartCoroutine(stopFlyMode());

        }
        if (isDead)
        {
            Debug.Log("Player is Dead");
            Destroy(gameObject);
        }
        Move(moveValue);
    }

    public void BlowBubble() {

        Debug.Log("Blow bubble");
        if (projectile && projectileSpawPoint)
        {
            Rigidbody2D temp = Instantiate(projectile, projectileSpawPoint.position, projectileSpawPoint.rotation);
            //most do it in GameManager
            //Physics2D.IgnoreCollision(GetComponent<Collider2D>());
            //Physics2D.GetIgnoreLayerCollision
            if (m_FacingRight)
            {
                temp.AddForce(projectileSpawPoint.right * projectileForce, ForceMode2D.Impulse);
            }
            else
                temp.AddForce(-projectileSpawPoint.right * projectileForce, ForceMode2D.Impulse);

        }
    }

    IEnumerator stopFlyMode()
    {
        yield return new WaitForSeconds(flyTime);
        rb2.gravityScale = 2.5f;
        isFlyMode = false;
    }

    public void BubbleFly()
    {
        rb2.gravityScale = gravity;
        animator.SetBool("isFly", false);

        animator.SetTrigger("isIdle");
    }

    public void Move(float move)
    {
        //only control the player if grounded or airControl is turned on
        if (isGrounded || m_AirControl)
        {
            Vector3 targetVelocity = new Vector2(move, rb2.velocity.y);
            rb2.velocity = Vector3.SmoothDamp(rb2.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight || move < 0 && m_FacingRight)
            {
                Flip();

            }
        }

    }
    public void GethitbyEnemies() {

        if (!isDead)
        {
            life -= 1;
            Debug.Log("Current life:" + life);
            if (life <= 0)
            {
                isDead = true;
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Collectible")
        //{
        //    Destroy(collision.gameObject);
        //}
        if (collision.gameObject.tag == "Enemies_Light")
        {
            Debug.Log("Test");

            if (isDead)
            {
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
            else
            {

                GethitbyEnemies();
                Destroy(collision.gameObject);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Teleport_Btm")
        {
            StartCoroutine(Teleports());
        }
    }

    IEnumerator Teleports()
    {
        yield return new WaitForSeconds(0.6f);
        Debug.Log("Test");

        gameObject.transform.position = new Vector2(gameObject.transform.position.x, topTeleport.transform.position.y);
    }
    public int points { get; set; }
}
