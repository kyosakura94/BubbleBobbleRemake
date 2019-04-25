using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float lifeTime;
    public Rigidbody2D changeBubble;
    private Enemy enemy;
    public ParticleSystem deathExpotion;


    // Use this for initialization
    void Start () {

        if (lifeTime <= 0)
        {
            lifeTime = 2.0f;
            Debug.LogWarning("LifeTime of projectile is not set. Default to"+ lifeTime);
        }

        Destroy(gameObject, lifeTime);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Playerdeath");
            GameManager.instance.removeEnemy(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {

    }
}
