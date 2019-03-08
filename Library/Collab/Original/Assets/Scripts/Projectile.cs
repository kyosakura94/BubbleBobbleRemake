using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float lifeTime;
    public Rigidbody2D changeBubble;
    public Rigidbody2D fruitItems;
    public string enemiesName;
    public List<Enemy_Type> Enemy_type = new List<Enemy_Type>();

    // Use this for initialization
    void Start () {

        if (lifeTime <= 0)
        {
            lifeTime = 2.0f;
            Debug.LogWarning("LifeTime of projectile is not set. Default to"+ lifeTime);
        }
        if (fruitItems)
        {

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
            
            //Enemy_Type c = collision.GetComponent<Enemy_Type>();
            

            Enemy_Type c = collision.GetComponent("Enemy_Type") as Enemy_Type;
            Enemy_type.Add(c);

            //Debug.Log(c.EnemyName);
            //Debug.Log(Enemy_type[0].EnemyName);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {

    }
}
