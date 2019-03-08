using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ZoneAttack : MonoBehaviour
{
    public Enemy_Base enemyBase;
    float timeSinceLastFire;
    public bool isFacingPlayer;
    public bool isFacingRight;

    public float fireRate;
    bool status = true;
    private bool isMoving = true;

    Rigidbody2D rb;

    public Rigidbody2D projectile;
    public Transform projectileSpawnPoint;
    public float projectileForce;

    public Rigidbody2D changeBubble;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Change variables of Rigidbody2D after saving a reference
        rb.mass = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        enemyBase.anim.GetComponent<Animator>();

        // Check if 'anim' variable was set in the inspector
        if (!enemyBase.anim)
        {
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogError("Animator not found on " + name);
        }
    }
    void Update()
    {
        if (isMoving)
        {
            
            if (rb)
                if (!isFacingRight)
                    // Make player move left 
                    rb.velocity = new Vector2(-enemyBase.speed, rb.velocity.y);
                else
                    // Make player move right 
                    rb.velocity = new Vector2(enemyBase.speed, rb.velocity.y);
        }

            
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyBase.anim.SetBool("isStop", false);
            rb.bodyType = RigidbodyType2D.Dynamic;
            isMoving = true;

            status = true;

            if (!isFacingRight)
            {
                flip();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBlocker"))
        {    
            flip();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyBase.anim.SetBool("isStop", true);
            rb.bodyType = RigidbodyType2D.Static;
            isMoving = false;

            // coolDown += Time.deltaTime;
            // if(coolDown > fireRate)
            //bool status = true;

            if (status)
            {
                isFacingPlayer = collision.GetComponent<Character>().m_FacingRight;

                if (isFacingPlayer != isFacingRight)
                {
                    //Debug.Log("Khac");
                    //Vector3 scaleFactor = transform.localScale;

                    //// Change sign of scale in 'x'
                    //scaleFactor.x *= -1; // or - -scaleFactor.x

                    //// Assign updated value back to 'localScale'
                    //transform.localScale = scaleFactor;
                    flip();

                }
                
                status = false;
            }

            //if (isFacingPlayer != collision.GetComponent<Character>().m_FacingRight)
            //{
            //    flip();
            //}
            //if (isFacingPlayer)
            //{
            //    Vector3 theScale = transform.localScale;

            //    theScale.x *= -1;

            //    transform.localScale = theScale;
            //}
            //else
            //{
            //    Vector3 theScale = transform.localScale;

            //    theScale.x *= 1;

            //    transform.localScale = theScale;
            //}

            if (Time.time > timeSinceLastFire + fireRate)
            {
                enemyBase.anim.SetTrigger("isAttack");
                //fire();

                timeSinceLastFire = Time.time;
            }
            Debug.Log("it moving");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (!collision.gameObject.CompareTag("Ground"))
        //   flip();

        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);

            enemyBase.EnemyHealth--;
            Debug.Log("Enemyhealth" + enemyBase.EnemyHealth);
            if (enemyBase.EnemyHealth-- <= 0)
            {
                // No Animation Event
                //Destroy(gameObject);
                // Animation Event
                // -Start Animation
                //enemyBase.anim.SetTrigger("Death");
                Vector3 pos = gameObject.transform.position + new Vector3(0.0f, 2.0f, 0.0f);

                Rigidbody2D temp = Instantiate(changeBubble, pos, gameObject.transform.rotation);

                temp.AddForce(gameObject.transform.right * 0.2f, ForceMode2D.Impulse);

                //enemy = collision.gameObject.GetComponent("Enemy") as Enemy;

                temp.GetComponent<Enemy_InBubble>().fruitItem = enemyBase.fruitItem;
                temp.GetComponent<Enemy_InBubble>().enemiesName = enemyBase.EnemyName;

                Destroy(gameObject);
            }
        }
    }
    void flip()
    {
        // Toggle variable
        isFacingRight = !isFacingRight;

        // Keep a copy of 'localScale' because scale cannot be changed directly
        Vector3 scaleFactor = transform.localScale;

        // Change sign of scale in 'x'
        scaleFactor.x *= -1; // or - -scaleFactor.x

        // Assign updated value back to 'localScale'
        transform.localScale = scaleFactor;
    }

    void fire()
    {
        //Check if 'projectileSpawnPoint' and 'projectile' exist
        if (projectileSpawnPoint && projectile)
        {
            //Create the 'Projectile' and add to Scene
            Rigidbody2D temp = Instantiate(projectile, projectileSpawnPoint.position,
                projectileSpawnPoint.rotation);

            //Stop 'Character' from hitting 'Projectile'
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(),
                temp.GetComponent<Collider2D>(), true);

            //Check what direction 'Character' is facing before firing
            if (isFacingRight)
            {
                temp.transform.Rotate(0, 180, 0);
                temp.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
            }
            else
            {
                //temp.transform.Rotate(0, -180, 0);
                temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);

            }
        }

    }


}
