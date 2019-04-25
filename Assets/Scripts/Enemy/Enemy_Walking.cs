using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Walking : MonoBehaviour
{
    public Enemy enemy;

    Rigidbody2D rb;

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
        if (enemy.EnemyType.ToString() == "OneHit")
        {
            if (rb)
                if (!enemy.isFacingRight)
                    // Make player move left 
                    rb.velocity = new Vector2(-enemy.speed, rb.velocity.y);
                else
                    // Make player move right 
                    rb.velocity = new Vector2(enemy.speed, rb.velocity.y);
        }

        if (Time.time > timeSinceLastFire + enemy.fireRate)
        {
            enemy.Attack();
            timeSinceLastFire = Time.time;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBlocker"))
        {
            enemy.flip(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);

            enemy.EnemyHealth--;

            if (enemy.EnemyHealth-- <= 0)
            {
                enemy.Dead(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
