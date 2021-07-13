using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 intercept = new Vector3(-3.4f, 6.82f, -3.24f);
    
    Vector3 PlayerPosition
    {
        get
        {
            return Player.Instance.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerPosition + intercept;
    }
}
