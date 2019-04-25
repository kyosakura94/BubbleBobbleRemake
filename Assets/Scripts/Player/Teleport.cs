using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public Transform top;
    GameObject Player;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Character>().isDead == false)
            {
                StartCoroutine(Teleports(other.gameObject));
            }
        }
    }
    //private void Update()
    //{
    //    if (GameManager.instance.gm == GameState.Preparing)
    //    {
    //        Player = GameObject.FindGameObjectWithTag("Player");
    //    }
    //}

    IEnumerator Teleports(GameObject other)
    {
        yield return new WaitForSeconds(0.1f);
        other.transform.position = new Vector2(other.transform.position.x, top.transform.position.y);
    }
}
