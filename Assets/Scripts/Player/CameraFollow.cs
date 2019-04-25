using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject newcPos;

    void Update()
    {
        if (GameManager.instance.gm  == GameState.Starting )
        {
            newcPos  = GameManager.instance.camraPosition;
        }
     
        transform.position = Vector3.Lerp(transform.position, newcPos.transform.position, 0.02f);
    }
}
