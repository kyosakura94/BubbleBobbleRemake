using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Enemy : Enemy_Base
{
    

    public override void flip(GameObject player)
    {
        // Toggle variable
        isFacingRight = !isFacingRight;

        // Keep a copy of 'localScale' because scale cannot be changed directly
        Vector3 scaleFactor = player.transform.localScale;

        // Change sign of scale in 'x'
        scaleFactor.x *= -1; // or - -scaleFactor.x

        // Assign updated value back to 'localScale'
        player.transform.localScale = scaleFactor;
    }

    public override void Attack() {

        AudioManager go = GameObject.FindObjectOfType<AudioManager>();
        go.Play("Enemyhit");
        if (projectileSpawnPoint && projectile)
        {
            // Create the 'Projectile' and add to Scene
            Rigidbody2D temp = Rigidbody2D.Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            if (isFacingRight)
            {
                temp.transform.Rotate(0, 180, 0);
                temp.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
            }
            else
                temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
        }
    }

    public override void Dead(GameObject gameObject)
    {
        AudioManager go = GameObject.FindObjectOfType<AudioManager>();
        go.Play("Enemydeath");

        Vector3 pos = gameObject.transform.position + new Vector3(0.0f, 2.0f, 0.0f);

        Rigidbody2D temp = Rigidbody2D.Instantiate(changeBubble, pos, gameObject.transform.rotation);

        temp.AddForce(gameObject.transform.right * 0.2f, ForceMode2D.Impulse);

        temp.GetComponent<Enemy_InBubble>().fruitItem = fruitItem;
        temp.GetComponent<Enemy_InBubble>().enemiesName = EnemyName;
    }

}
