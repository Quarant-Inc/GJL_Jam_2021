using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 PlayerPosition
    {
        get
        {
            return Player.Instance.transform.position;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerPosition + new Vector3(-3.4f, 6.82f, -3.24f);
    }
}
