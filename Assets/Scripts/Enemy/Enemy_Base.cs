using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Enemy_Base {

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
}
