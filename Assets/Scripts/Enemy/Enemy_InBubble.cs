using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_InBubble : MonoBehaviour
{
    public Rigidbody2D fruitItem;
    public string enemiesName;
   
    void Start()
    {
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Vector3 pos = gameObject.transform.position + Vector3.up;

    //        Projectile enemyType = collision.GetComponent("Projectile") as Projectile;

    //        enemyType.test.fruitItem = fruitItem;
    //        enemyType.test.EnemyType = enemiesType;

    //        Rigidbody2D temp = Instantiate(fruitItem, pos, gameObject.transform.rotation);


    //        Debug.Log(enemyType.test.EnemyType);

    //        temp.AddForce(gameObject.transform.right * 0.2f, ForceMode2D.Impulse);


    //        Destroy(gameObject);
    //    }
    //}
    // Update is called once per frame
    void Update()
    {
        
    }
}
