using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    static CameraFollow instance;
    public static CameraFollow Instance
    {
        get
        {
            return instance;
        }
    }

    public Vector3 CameraGroundForward
    {
        get
        {
            Vector3 orientation = transform.rotation.eulerAngles;
            return new Vector3(0, orientation.y, 0).normalized;
        }
    }

    void Awake()
    {
        instance = this;
    }

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