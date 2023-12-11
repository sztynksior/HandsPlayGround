using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyWhenOutOfScene : MonoBehaviour
{
    void Update()
    {
        if(gameObject.transform.position.y < -10.0)
        {
            Destroy(gameObject);
        }
    }
}
