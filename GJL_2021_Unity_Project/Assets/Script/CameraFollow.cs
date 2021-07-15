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
            Vector3 facingUp =  new Vector3(0, orientation.y, 0);
            Vector3 camGroundForward = Quaternion.Euler(facingUp) * Vector3.forward;
            return camGroundForward;
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