using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public Transform top;
    public GameObject Player;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Character>().isDead == false)
            {
                StartCoroutine(Teleports());
            }
            
        }
    }

    IEnumerator Teleports()
    {
        yield return new WaitForSeconds(0.6f);
        Debug.Log("Test");
        Player.transform.position = new Vector2(Player.transform.position.x, top.transform.position.y);
    }
}
