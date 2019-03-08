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

    public bool m_FacingRight = true;

    public Rigidbody2D projectile;
    public float projectileForce;
    public Transform projectileSpawPoint;

    public float life;
    public bool isDead = false;

    private float gravity = -0.5f;
    public float flyTime;

    float chargeTime = 0.0f;


    public Transform topTeleport;
    private bool status = true;

    public bool isGodMode;
    public float jumpBoost;
    public float powerUpTime;

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

        animator.SetBool("isPowerUp", false);   
    }

    // Update is called once per frame
    void Update() {

        float moveValue = Input.GetAxisRaw("Horizontal") * speed;
        Debug.Log("Presing " + moveValue);

        animator.SetFloat("isRun", Mathf.Abs(moveValue));

        if (groundCheck)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        }

        if (isGrounded)
        {
            Debug.Log(isGrounded);
            animator.SetBool("isJump", false);
            //animator.SetBool("isFlyIdle", false);
        }

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb2.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetBool("isJump", true);
            }

            if (Input.GetButton("Fire2"))
            {
                chargeTime += Time.deltaTime;
                Debug.Log(chargeTime);
                animator.SetBool("isFlyIdle", false);
                animator.SetBool("isFly", true);
                //animator.SetBool("isFlyIdle", true);
                //animator.SetTrigger("isTrickIdle");
                //StartCoroutine(stopFlyMode());
                //if (chargeTime >= 2.0f)
                //{
                //    animator.SetTrigger("isTrickIdle");
                //}

            }

            if (Input.GetButtonUp("Fire2"))
            {             
                animator.SetBool("isFly", false);
                if (chargeTime >= 0.4f)
                {
                    animator.SetBool("isFlyIdle", true);
                    rb2.gravityScale = -0.1f;

                }
                Debug.Log("gravityScale" + rb2.gravityScale);

                m_AirControl = true;

                Move(moveValue);
                chargeTime = 0.0f;
                //rb2.gravityScale = 3.0f;
            }

            //if (Input.GetButtonUp("Fire2"))
            //{
            //    animator.SetBool("isFlyIdle", true);
            //    chargeTime = 0.0f;
            //    //rb2.gravityScale = 3.0f;

            //}
            //else
            //{
            //    animator.SetBool("isFly", false);
            //    //animator.SetBool("isFlyIdle", false);
            //    //isFlyMode = false;
            //}
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");

        }

        if (m_AirControl)
        {
            if (Input.GetButtonDown("Fire3"))
            {
                animator.SetBool("isFlyIdle", false);
                rb2.gravityScale = 2.5f;
                m_AirControl = false;
            }
        }
        
        if (isDead && status)
        {
            Debug.Log("Player is Dead");
            animator.SetBool("isDie", true);
            status = false;
            rb2.AddForce(Vector2.up * 12.0f, ForceMode2D.Impulse);
            rb2.GetComponent<CircleCollider2D>().isTrigger = true;
            StartCoroutine(Death());    
        }

        Move(moveValue);
    }
    IEnumerator Death() {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
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
        rb2.gravityScale = 3.0f;
    }

    public void BubbleFlyIdle()
    {
        //animator.SetBool("isFlyIdle", true);
        //rb2.gravityScale = -0.1f;
    }

    public void BubbleFly()
    {
        animator.SetBool("isFly", false);
        rb2.gravityScale = gravity;
        animator.SetTrigger("isTrickIdle");
        
        //if (!isGrounded)
        //{
        //    animator.SetBool("isFly", false);
        //    animator.SetBool("isIdle", true);

        //}     
    }

    public void Move(float move)
    {
        //only control the player if grounded or airControl is turned on
        if (isGrounded || m_AirControl)
        {
            Debug.Log("He is moving");

            Vector3 targetVelocity = new Vector2(move, rb2.velocity.y);

            rb2.velocity = Vector3.SmoothDamp(rb2.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight || move < 0 && m_FacingRight)
            {
                Flip();
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
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
        if (collision.gameObject.tag == "Enemies_Light" || collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);

            Debug.Log("Test");
            if (isDead)
            {
                Destroy(gameObject);
            }
            else
            {
                animator.SetTrigger("gethit");
                //GethitbyEnemies();
            }

        }

        if (collision.gameObject.tag == "Enemies_InBubble")
        {
            Vector3 pos = gameObject.transform.position + new Vector3(0.0f, 2.0f, 0.0f);

            Enemy_InBubble test = collision.gameObject.GetComponent("Enemy_InBubble") as Enemy_InBubble;

            Rigidbody2D temp = Instantiate(test.fruitItem, pos, collision.transform.rotation);

            if (m_FacingRight)
            {
                Vector3 force = collision.transform.up + collision.transform.right;
                temp.gravityScale = 0;

                temp.AddForce(force * 4.0f, ForceMode2D.Impulse);
                StartCoroutine(setGravityScale());

                temp.gravityScale = 1;
            }
            else
            {
                Vector3 force = collision.transform.up + -collision.transform.right;
                temp.gravityScale = 0;

                temp.AddForce(force * 0.5f, ForceMode2D.Impulse);
                StartCoroutine(setGravityScale());

                temp.gravityScale = 1;
                temp.AddForce(gameObject.transform.up * 0.2f, ForceMode2D.Impulse);
            }

            Debug.Log(test.enemiesName);

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Collectable")
        {
            // Create a reference to the Script on 'Collectible'
            Collectible c = collision.gameObject.GetComponent<Collectible>();

            // Check if 'Collectible' Script exists on GameObject being collided with
            if (c)
            {
                // Increase points using Scripts variable
                points += c.points;
            }

            // Delete gameObject that collided with 'Character'
            Destroy(collision.gameObject);
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Teleport_Btm")
        {
            StartCoroutine(Teleports());
        }
        if (collision.gameObject.tag == "Teleport_Btm")
        {
            StartCoroutine(Teleports());
        }

        if (collision.gameObject.tag == "Powerup_JumpMode")
        {
            animator.SetBool("isPowerUp", true);
            // Add the powerup
            jumpForce += jumpBoost;

            // Begin countdown to losing power
            StartCoroutine(stopJumpMode());

            // Delete gameObject that collided with 'Character'
            Destroy(collision.gameObject);
        }

    }
    IEnumerator stopJumpMode()
    {
        yield return new WaitForSeconds(powerUpTime);

        // Turn off Powerup after specified time
        jumpForce -= jumpBoost;
        animator.SetBool("isPowerUp", false);

    }

    IEnumerator Teleports()
    {
        yield return new WaitForSeconds(0.6f);
        Debug.Log("Test");

        gameObject.transform.position = new Vector2(gameObject.transform.position.x, topTeleport.transform.position.y);
    }

    IEnumerator setGravityScale()
    {
        yield return new WaitForSeconds(10.0f);
    }

    public int points { get; set; }
}
