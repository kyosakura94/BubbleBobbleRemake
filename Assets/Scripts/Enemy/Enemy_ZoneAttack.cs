using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ZoneAttack : MonoBehaviour
{

    public bool isFacingPlayer;
    bool status = true;
    private bool isMoving = true;

    Rigidbody2D rb;

    public Enemy enemy;
    float timeSinceLastFire;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Change variables of Rigidbody2D after saving a reference
        rb.mass = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        enemy.anim.GetComponent<Animator>();

        // Check if 'anim' variable was set in the inspector
        if (!enemy.anim)
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
                if (!enemy.isFacingRight)
                    // Make player move left 
                    rb.velocity = new Vector2(-enemy.speed, rb.velocity.y);
                else
                    // Make player move right 
                    rb.velocity = new Vector2(enemy.speed, rb.velocity.y);
        }

            
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.anim.SetBool("isStop", false);
            rb.bodyType = RigidbodyType2D.Dynamic;
            isMoving = true;

            status = true;

            if (!enemy.isFacingRight)
            {
                enemy.flip(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBlocker"))
        {
            enemy.flip(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.anim.SetBool("isStop", true);
            rb.bodyType = RigidbodyType2D.Static;
            isMoving = false;

            if (status)
            {
                isFacingPlayer = collision.GetComponent<Character>().m_FacingRight;

                if (isFacingPlayer != enemy.isFacingRight)
                {
                    enemy.flip(gameObject);

                }      
                status = false;
            }

            if (Time.time > timeSinceLastFire + enemy.fireRate)
            {
                enemy.anim.SetTrigger("isAttack");
                timeSinceLastFire = Time.time;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);

            enemy.EnemyHealth--;
            Debug.Log("Enemyhealth" + enemy.EnemyHealth);
            if (enemy.EnemyHealth-- <= 0)
            {
                Vector3 pos = gameObject.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
                Rigidbody2D temp = Instantiate(enemy.changeBubble, pos, gameObject.transform.rotation);

                temp.AddForce(gameObject.transform.right * 0.2f, ForceMode2D.Impulse);
                temp.GetComponent<Enemy_InBubble>().fruitItem = enemy.fruitItem;
                temp.GetComponent<Enemy_InBubble>().enemiesName = enemy.EnemyName;

                Destroy(gameObject);
            }
        }
    }

    void fire()
    {
        enemy.Attack();
    }

}
