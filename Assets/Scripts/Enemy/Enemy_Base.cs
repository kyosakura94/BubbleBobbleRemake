using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy_Base {

    public enum Type{
        Range,
        OneHit,
        Moving
    }

    public Animator anim;
    public float speed;

    public Type EnemyType;
    public int EnemyHealth;

    public string EnemyName;

    public Rigidbody2D fruitItem;
    public Rigidbody2D projectile;

    public float projectileForce;

    public Transform projectileSpawnPoint;

    public Rigidbody2D changeBubble;
    public float fireRate;
    public bool isFacingRight;

    public virtual void Attack() {
    }
    public virtual void flip(GameObject player) {
    }
    public virtual void Dead(GameObject player) { }

}
