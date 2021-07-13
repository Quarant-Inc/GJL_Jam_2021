using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerDied();
public class Player : MonoBehaviour
{
    #region SingletonStuff
    static Player instance;
    public static Player Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    #region PlayerEvents
    public PlayerDied PlayerDied;
    #endregion

    #region Stats
    const int maxHealth = 10;
    const int defaultSpeed = 10;
    int health;
    public int Health
    {
        get
        {
            return health;
        }
        private set
        {
            //Restrict health value to be between 0 and maxHealth.
            health = value <= maxHealth && value >= 0 ? value :
            (
                value >= maxHealth ? maxHealth : 0
            );

            //Activate death event if health runs out.
            if (value <= 0)
            {
                PlayerDied();
                Debug.Log("Player done dieded.");
            }
        }
    }
    float speed;
    #endregion

    #region Components
    Rigidbody body;
    Rigidbody RigidBody
    {
        get
        {
            if (body == null)
            {
                return body = GetComponent<Rigidbody>();
            }
            return body;
        }
    }
    #endregion

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetStats();
    }

    Dictionary<DIRECTION, Vector3> directionVectors = new Dictionary<DIRECTION, Vector3>()
    {
        {DIRECTION.FORWARD, Vector3.forward},
        {DIRECTION.BACKWARD, Vector3.back},
        {DIRECTION.LEFT, Vector3.left},
        {DIRECTION.RIGHT, Vector3.right}
    };

    Vector3 GetDirectionVector(DIRECTION dir)
    {
        switch(dir)
        {
            case DIRECTION.FORWARD:
            {
                return transform.forward;
            }
            case DIRECTION.LEFT:
            {
                return -transform.right;
            }
            case DIRECTION.RIGHT:
            {
                return transform.right;
            }
            case DIRECTION.BACKWARD:
            {
                return -transform.forward;
            }
            default:
            {
                return new Vector3();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(DIRECTION.FORWARD);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Move(DIRECTION.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(DIRECTION.BACKWARD);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(DIRECTION.RIGHT);
        }
    }

    DIRECTION prevDirection = DIRECTION.NONE;

    void Move(DIRECTION dir)
    {
        Vector3 direction = directionVectors[dir];
        //Vector3 direction = GetDirectionVector(dir);
        if (prevDirection != dir)
        {
            transform.LookAt(transform.position + direction, transform.up);
        }
        //
        //Vector3 direction = directionVectors[dir];
        transform.LookAt(transform.position + direction, transform.up);
        RigidBody.velocity = direction * speed * Time.deltaTime * 40;

        prevDirection = dir;
        
    }

    void ResetStats()
    {
        health = maxHealth;
        speed  = defaultSpeed;
    }
}