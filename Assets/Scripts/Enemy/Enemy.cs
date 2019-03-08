using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Enemy_Base enemyBase;

    public bool isFacingRight;

    Rigidbody2D rb;


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
        if (enemyBase.EnemyType.ToString() == "OneHit")
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBlocker"))
        {
            flip();
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

}
