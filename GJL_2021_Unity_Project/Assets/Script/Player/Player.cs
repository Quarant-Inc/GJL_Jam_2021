using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(int health);
public delegate void SpeedChanged(int speed);
public delegate void ArmourChanged(int armour);
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
    public HealthChanged HealthChanged;
    public SpeedChanged SpeedChanged;
    public ArmourChanged ArmourChanged;
    #endregion

    #region Stats
    const int maxHealth = 10;
    const int defaultSpeed = 10;
    const int maxArmour = 5;

    public int MaxArmour
    {
        get
        {
            return maxArmour;
        }
    }
    int armour = 0;
    public int Armour
    {
        get
        {
            return armour;
        }
        private set
        {
            armour = value >= 0 && value <= maxArmour ? value :
            (
                value > maxArmour ? maxArmour : 0
            );

            if (ArmourChanged != null)
            {
                ArmourChanged(armour);
            }
        }
    }
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

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

            HealthChanged(health);

            //Activate death event if health runs out.
            if (value <= 0)
            {
                if (PlayerDied != null)
                {
                    PlayerDied();
                }
                Debug.Log("Player done dieded.");
            }
        }
    }
    int speed;
    public int Speed
    {
        get
        {
            return speed;
        }
        private set
        {
            speed = value < 0 ? 0 : value;

            if (SpeedChanged != null)
            {
                SpeedChanged(speed);
            }
        }
    }
    #endregion

    #region Components

    Animator animator;
    Animator Animator
    {
        get
        {
            if (animator == null)
            {
                return animator = GetComponent<Animator>();
            }
            return animator;
        }
    }

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

    Queue<ItemTemplate> items = new Queue<ItemTemplate>();

    void Awake()
    {
        instance = this;
        ResetStats();
    }

    void Start()
    {
        
    }

    Dictionary<DIRECTION, Vector3> directionVectors = new Dictionary<DIRECTION, Vector3>()
    {
        {DIRECTION.FORWARD, Vector3.forward},
        {DIRECTION.BACKWARD, Vector3.back},
        {DIRECTION.LEFT, Vector3.left},
        {DIRECTION.RIGHT, Vector3.right}
    };

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(DIRECTION.FORWARD);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move(DIRECTION.LEFT);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Move(DIRECTION.BACKWARD);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(DIRECTION.RIGHT);
        }
        else
        {
            Animator.SetTrigger(PLAYER_ANIM_PARAMS.STOP_MOVING.ToString());
            RigidBody.velocity = new Vector3(0, RigidBody.velocity.y, 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            UseItem();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (localItem != null)
            {
                PickupItem(localItem);
                localItem = null;
            }
        }
    }

    DIRECTION prevDirection = DIRECTION.NONE;

    void Move(DIRECTION dir)
    {
        Animator.SetTrigger(PLAYER_ANIM_PARAMS.MOVE.ToString());

        Vector3 direction = directionVectors[dir];
        //Vector3 direction = GetDirectionVector(dir);
        if (prevDirection != dir)
        {
            transform.LookAt(transform.position + direction, transform.up);
        }
        //
        //Vector3 direction = directionVectors[dir];
        transform.LookAt(transform.position + direction, transform.up);
        RigidBody.velocity = direction * speed;

        prevDirection = dir;
        
    }

    void ResetStats()
    {
        health = maxHealth;
        speed  = defaultSpeed;
    }

    Item localItem;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == TAG.Item.ToString())
        {
            Item item = col.gameObject.GetComponent<Item>();
            //PickupItem(item);
            localItem = item;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == TAG.Item.ToString())
        {
            if (col.gameObject.GetComponent<Item>() == localItem)
            {
                localItem = null;
            }
        }
    }

    void PickupItem(Item item)
    {
        ItemTemplate temp = new ItemTemplate();
        temp.name = item.name;
        temp.type = item.type;
        items.Enqueue(temp);

        UIManager.Instance.AddItem(temp);

        Destroy(item.gameObject);
    }

    void UseItem()
    {
        if (items.Count > 0)
        {
            ItemTemplate item = items.Dequeue();
            Debug.LogFormat("Used {0}",item.name);

            UIManager.Instance.UsedItem();
        }
    }

    // simple just to test hearts for now
    public void TakeDamage()
    {
        Health --;
    }
}