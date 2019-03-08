using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float lifeTime;
    public Rigidbody2D changeBubble;
    private Enemy enemy;

    // Use this for initialization
    void Start () {

        if (lifeTime <= 0)
        {
            lifeTime = 2.0f;
            Debug.LogWarning("LifeTime of projectile is not set. Default to"+ lifeTime);
        }

        Destroy(gameObject, lifeTime);
	}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Vector3 pos = gameObject.transform.position + new Vector3(0.0f, 2.0f, 0.0f);

    //        Rigidbody2D temp = Instantiate(changeBubble, pos, gameObject.transform.rotation);

    //        temp.AddForce(gameObject.transform.right * 0.2f, ForceMode2D.Impulse);

    //        enemy = collision.gameObject.GetComponent("Enemy") as Enemy;

    //        temp.GetComponent<Enemy_InBubble>().fruitItem = enemy.enemyBase.fruitItem;
    //        temp.GetComponent<Enemy_InBubble>().enemiesName = enemy.enemyBase.EnemyName;

    //        Destroy(gameObject);
    //        Destroy(collision.gameObject);
    //    }
    //}


    // Update is called once per frame
    void Update () {

    }
}
