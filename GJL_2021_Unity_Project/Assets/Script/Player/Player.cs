using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void HealthChanged(int health);
public delegate void MaxHeathChanged(int maxHealth);
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
    public MaxHeathChanged MaxHeathChanged;
    public SpeedChanged SpeedChanged;
    public ArmourChanged ArmourChanged;
    #endregion

    #region Stats
    int maxHealth = 10;
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
        private set
        {
            maxHealth = value;
            MaxHeathChanged(maxHealth);
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

    float defaultMaxPickup = 1.1f;
    float MaxPickupDistance
    {
        get
        {
            return SphereTrigger.radius;
        }
        set
        {
            SphereTrigger.radius = value;
        }
    }

    Vector3 CameraGroundForward
    {
        get
        {
            return CameraFollow.Instance.CameraGroundForward;
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

    SphereCollider sphereTrigger;
    SphereCollider SphereTrigger
    {
        get
        {
            if (sphereTrigger == null)
            {
                return sphereTrigger = GetComponent<SphereCollider>();
            }
            return sphereTrigger;
        }
    }
    #endregion

    //Queue<ItemTemplate> items = new Queue<ItemTemplate>();
    Queue<Item> items = new Queue<Item>();
    //Item localItem;
    //GameObject localItemObject;
    //List<ItemGameObjectPair> localItems = new List<ItemGameObjectPair>();
    Dictionary<int, ItemGameObjectPair> localItems = new Dictionary<int, ItemGameObjectPair>();



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
            if (Input.GetKey(KeyCode.A))
            {
                Move(DIRECTION.FORWARD_LEFT);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Move(DIRECTION.FORWARD_RIGHT);
            }
            else
            {
                Move(DIRECTION.FORWARD);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.A))
            {
                Move(DIRECTION.BACKWARD_LEFT);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Move(DIRECTION.BACKWARD_RIGHT);
            }
            else
            {
                Move(DIRECTION.BACKWARD);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move(DIRECTION.LEFT);
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
            if (localItems.Count > 0)
            {
                ItemGameObjectPair closestPair = new ItemGameObjectPair(null, null);
                float closestDist = float.MaxValue;
                foreach(ItemGameObjectPair pair in localItems.Values)
                {
                    float distance = Vector3.Distance(transform.position, pair.gameObject.transform.position);
                    if (distance < closestDist)
                    {
                        closestPair = pair;
                        closestDist = distance;
                    }
                }
                PickupItem(closestPair);
            }
            /*if (localItem != null)
            {
                PickupItem(localItem);
                localItem = null;
            }
            else
            {
                Debug.Log("localItem is null, silly.");
            }*/
        }
    }

    DIRECTION prevDirection = DIRECTION.NONE;

    Vector3 GetDirectionVector(DIRECTION dir)
    {
        Vector3 forward = CameraGroundForward;
        Vector3 backward = -forward;
        Vector3 right = Quaternion.AngleAxis(90f, transform.up) * forward;
        Vector3 left = Quaternion.AngleAxis(90f, transform.up) * backward;
        
        Vector3 forward_right = Quaternion.AngleAxis(45f, transform.up) * forward;
        Vector3 forward_left = Quaternion.AngleAxis(-45f, transform.up) * forward;
        Vector3 backward_right = Quaternion.AngleAxis(-45f, transform.up) * backward;
        Vector3 backward_left = Quaternion.AngleAxis(45f, transform.up) * backward;

        switch(dir)
        {
            case DIRECTION.FORWARD:
            {
                return forward;
            }
            case DIRECTION.BACKWARD:
            {
                return backward;
            }
            case DIRECTION.LEFT:
            {
                return left;
            }
            case DIRECTION.RIGHT:
            {
                return right;
            }
            case DIRECTION.FORWARD_RIGHT:
            {
                return forward_right;
            }
            case DIRECTION.FORWARD_LEFT:
            {
                return forward_left;
            }
            case DIRECTION.BACKWARD_RIGHT:
            {
                return backward_right;
            }
            case DIRECTION.BACKWARD_LEFT:
            {
                return backward_left;
            }
            default:
            {
                return new Vector3();
            }
        }
    }

    void Move(DIRECTION dir)
    {
        Animator.SetTrigger(PLAYER_ANIM_PARAMS.MOVE.ToString());

        //Vector3 direction = directionVectors[dir];
        Vector3 direction = GetDirectionVector(dir);
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



    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Triggered by "+col.name);
        if (col.tag == TAG.Item.ToString())
        {
            Debug.Log("item found");
            Item item = GetItemFromPickup(col.gameObject);
            if (item != null)
            {
                ItemGameObjectPair pair = new ItemGameObjectPair(item,col.gameObject);
                localItems.Add(pair.ID, pair);
                // localItem = item;
                // localItemObject = col.gameObject;
            }
        }
    }

    Item GetItemFromPickup(GameObject obj)
    {
        PickupItem<Weapon> weapon = obj.GetComponent<PickupItem<Weapon>>();
        if (weapon != null)
        {
            return weapon.itemSpec;
        }

        PickupItem<Tool> tool = obj.GetComponent<PickupItem<Tool>>();
        if (tool != null)
        {
            return tool.itemSpec;
        }

        PickupItem<Potion> potition = obj.GetComponent<PickupItem<Potion>>();
        if (potition != null)
        {
            return potition.itemSpec;
        }

        return null;
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == TAG.Item.ToString())
        {
            Item item = GetItemFromPickup(col.gameObject);
            if (item != null)
            {
                ItemGameObjectPair pair = localItems.Values.Where(i => i.gameObject == col.gameObject).ElementAt(0);
    
                //localItem = null;
            }
        }
    }

    void PickupItem(ItemGameObjectPair pair)
    {
        Debug.Log("Pickup attempted");
        
        /*ItemTemplate temp = new ItemTemplate();
        temp.name = item.name;
        temp.type = item.itemSpec.type;
        items.Enqueue(temp);*/

        items.Enqueue(pair.item);

        //UIManager.Instance.AddItem(temp);
        UIManager.Instance.AddItem(pair.item);

        localItems.Remove(pair.ID);
        Destroy(pair.gameObject);
    }

    void UseItem()
    {
        if (items.Count > 0)
        {
            //ItemTemplate item = items.Dequeue();
            Item item = items.Dequeue();
            Debug.LogFormat("Used {0}",item.name);

            item.Use();

            UIManager.Instance.UsedItem();
        }
    }

    // simple just to test hearts for now
    public void TakeDamage()
    {
        // take damage from armour value first. like temp hp
        if (Armour > 0)
        {
            Armour--;
        }
        else
        {
            Health--;
        }
    }

    public void AddMaxHealth()
    {        
        MaxHealth ++; 
        Debug.LogFormat("Increase max hp: {0}", MaxHealth);
    }

    public void FullHeal()
    {
        Health = MaxHealth;
    }

    public void AddArmour()
    {
        Armour++;
    }

    public float pickIncreaseTime = 3f;

    public void IncreasePickupRadius()
    {
        StartCoroutine(PickupWiden());
    }

    IEnumerator PickupWiden()
    {
        MaxPickupDistance = 3;
        Debug.LogFormat("MaxPickupDistance: {0};", MaxPickupDistance);
        yield return new WaitForSeconds(pickIncreaseTime);
        MaxPickupDistance = 1.1f;
        Debug.LogFormat("MaxPickupDistance: {0};",MaxPickupDistance);
    }
}

struct ItemGameObjectPair
{
    static int count = 0;
    readonly int id;
    public int ID
    {
        get
        {
            return id;
        }
    }
    public Item item;
    public GameObject gameObject;
    public ItemGameObjectPair(Item _item, GameObject _gameObject)
    {
        id = count++;
        item = _item;
        gameObject = _gameObject;
    }
}