using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float lifeTime;
    public Rigidbody2D changeBubble;

   
    // Use this for initialization
    void Start () {

        if (lifeTime <= 0)
        {
            lifeTime = 2.0f;
            Debug.LogWarning("LifeTime of projectile is not set. Default to"+ lifeTime);
        }
        Destroy(gameObject, lifeTime);
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Vector3 pos = gameObject.transform.position + Vector3.up;
            
            Rigidbody2D temp = Instantiate(changeBubble, pos, gameObject.transform.rotation);

            temp.AddForce(gameObject.transform.right * 0.2f, ForceMode2D.Impulse);
   
            Enemy_Type c = collision.GetComponent("Enemy_Type") as Enemy_Type;

            temp.GetComponent<Enemy_InBubble>().fruitItem = c.fruitItem;
            temp.GetComponent<Enemy_InBubble>().enemiesName = c.EnemyName;


            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {

    }
}
