using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public delegate void HealthChanged(int health);
public delegate void MaxHeathChanged(int maxHealth);
public delegate void SpeedChanged(int speed);
public delegate void ArmourChanged(int armour);
public delegate void PlayerDied();
public delegate void ItemPickedUp();
public delegate void ItemUsed();
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
    public ItemPickedUp ItemPickedUp;
    public ItemUsed ItemUsed;
    #endregion

    #region Stats
    int maxHealth = 10;
    const int defaultSpeed = 10;
    const int maxArmour = 5;
    bool spedUp = false;
    bool magnetEnabled = false;
    public GameObject temporaryBuffText;
    bool footstepsPlaying = false;
    public GameObject pauseScreen;
    public Button playButton;
    public Button quitButton;
    bool gamePaused = false;
    public AK.Wwise.Event clickSound = null;
    public static bool enemySpeaking = false;

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

            if (HealthChanged != null)
            {
                HealthChanged(health);
            }

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

    Queue<Item> items = new Queue<Item>();
    public List<Item> Items{
        get
        {
            return items.ToList();
        }
    }

    Dictionary<int, ItemGameObjectPair> localItems = new Dictionary<int, ItemGameObjectPair>();

    DIRECTION prevDirection = DIRECTION.NONE;

    void Awake()
    {
        instance = this;
        ResetStats();
    }

    void Start()
    {
        PlayerDied += Util.Quit;
    }

    public void SetAnimation(PLAYER_ANIM_PARAMS anim)
    {
        Animator.SetTrigger(anim.ToString());
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
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused) {

                gamePaused = false;
                Time.timeScale = 1;
                pauseScreen.SetActive(false);
            
            }
            else
            {
                gamePaused = true;
                Time.timeScale = 0;
                pauseScreen.SetActive(true);
            }
            
        }
        if (!gamePaused)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (!footstepsPlaying)
                {
                    footstepsPlaying = true;
                    AkSoundEngine.PostEvent("Player_Footstep_Start", gameObject);
                }

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
                if (!footstepsPlaying)
                {
                    footstepsPlaying = true;
                    AkSoundEngine.PostEvent("Player_Footstep_Start", gameObject);
                }

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
                if (!footstepsPlaying)
                {
                    footstepsPlaying = true;
                    AkSoundEngine.PostEvent("Player_Footstep_Start", gameObject);
                }

                Move(DIRECTION.LEFT);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (!footstepsPlaying)
                {
                    footstepsPlaying = true;
                    AkSoundEngine.PostEvent("Player_Footstep_Start", gameObject);
                }

                Move(DIRECTION.RIGHT);
            }
            else
            {
                Animator.SetTrigger(PLAYER_ANIM_PARAMS.STOP_MOVING.ToString());
                RigidBody.velocity = new Vector3(0, RigidBody.velocity.y, 0);

                if (footstepsPlaying)
                {
                    footstepsPlaying = false;
                    AkSoundEngine.PostEvent("Player_Footstep_Stop", gameObject);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                UseItem();
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (localItems.Count > 0)
                {
                    Debug.Log("gjrigjr");
                    ItemGameObjectPair closestPair;// = GetClosestPair();
                    if (GetClosestPair(out closestPair))
                    {
                        PickupItem(closestPair);
                    }
                }
            }
        }
    }
    bool looking = false;

    public void LookAt(Vector3 pos, float time)
    {
        StartCoroutine(LookAtAsync(pos, time));
    }

    IEnumerator LookAtAsync(Vector3 pos, float time)
    {
        looking = true;
        transform.LookAt(pos);
        yield return new WaitForSeconds(time);
        looking = false;
    }

    bool GetClosestPair(out ItemGameObjectPair igoPair)
    {
        bool foundOne = false;
        ItemGameObjectPair closestPair = new ItemGameObjectPair(null, null);
        float closestDist = float.MaxValue;
        foreach(ItemGameObjectPair pair in localItems.Values)
        {
            if (pair.gameObject == null)
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, pair.gameObject.transform.position);
            if (distance < closestDist)
            {
                closestPair = pair;
                closestDist = distance;
                foundOne = true;
            }
        }
        igoPair = closestPair;
        return foundOne;
    }

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

    public float forceMultiplier = 1f;
    public float maxSpeed = 10f;

    void Move(DIRECTION dir)
    {
        Animator.SetTrigger(PLAYER_ANIM_PARAMS.MOVE.ToString());

        //Vector3 direction = directionVectors[dir];
        Vector3 direction = GetDirectionVector(dir);
        //Vector3 direction = GetDirectionVector(dir);
        //if (prevDirection != dir)
        //{
        //    transform.LookAt(transform.position + direction, transform.up);
        //}
        //
        //Vector3 direction = directionVectors[dir];
        if (!looking)
        {
            transform.LookAt(transform.position + direction, transform.up);
        }

        RigidBody.AddForce(direction*speed*forceMultiplier);

        if(RigidBody.velocity.magnitude > maxSpeed)
        {
            float yVel = RigidBody.velocity.y;
            Vector3 clampedVelocity = RigidBody.velocity.normalized * maxSpeed;

            RigidBody.velocity = new Vector3(clampedVelocity.x, yVel, clampedVelocity.z);
            //RigidBody.velocity = RigidBody.velocity.normalized * maxSpeed;
        }

        //Debug.Log(RigidBody.velocity.magnitude);

        //RigidBody.velocity = direction * speed;

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
        PickupItem<MeleeWeapon> meleeWeapon = obj.GetComponent<PickupItem<MeleeWeapon>>();
        if (meleeWeapon != null)
        {
            return meleeWeapon.itemSpec;
        }

        PickupItem<StraightRangedWeapon> straightRangedWeapon = obj.GetComponent<PickupItem<StraightRangedWeapon>>();
        if (straightRangedWeapon != null)
        {
            return straightRangedWeapon.itemSpec;
        }

        PickupItem<ArcRangedWeapon> arcRangedWeapon = obj.GetComponent<PickupItem<ArcRangedWeapon>>();
        if (arcRangedWeapon != null)
        {
            return arcRangedWeapon.itemSpec;
        }
        // PickupItem<Weapon> weapon = obj.GetComponent<PickupItem<Weapon>>();
        // if (weapon != null)
        // {
        //     return weapon.itemSpec;
        // }

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
        if (pair.gameObject != null)
        {
            Debug.Log("Pickup attempted");
            
            /*ItemTemplate temp = new ItemTemplate();
            temp.name = item.name;
            temp.type = item.itemSpec.type;
            items.Enqueue(temp);*/

            items.Enqueue(pair.item);

            //UIManager.Instance.AddItem(temp);
            UIManager uIManager;
            if (UIManager.InstanceExists(out uIManager))
            {
                uIManager.AddItem(pair.item);
            }

            localItems.Remove(pair.ID);
            Debug.Log(pair.gameObject);
            Debug.Log("Parent: " + pair.gameObject.transform.parent.gameObject);
            Destroy(pair.gameObject.transform.parent.gameObject);

            if(ItemPickedUp != null)
            {
                ItemPickedUp();
            }
        }
    }

    void UseItem()
    {
        if (items.Count > 0)
        {
            //ItemTemplate item = items.Dequeue();
            Item item = items.Dequeue();
            Debug.LogFormat("Used {0}",item.name);

            item.Use();
            item.useSound.Post(gameObject);

            ItemUsed();

            UIManager uIManager;
            if (UIManager.InstanceExists(out uIManager))
            {
                uIManager.UsedItem();
            }
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

    public void AddHealth()
    {
        Health++;
        Mathf.Clamp(Health, 0, MaxHealth);
        //MaxHealth ++; 
        //Debug.LogFormat("Increase max hp: {0}", MaxHealth);
    }

    //public void FullHeal()
    //{
    //    Health = MaxHealth;
    //}

    public void AddArmour()
    {
        Armour++;
        Mathf.Clamp(Armour, 0, MaxArmour);
    }

    public float pickIncreaseTime = 3f;
    public float speedIncreaseTime = 3f;

    public void IncreasePickupRadius()
    {
        if (!magnetEnabled)
        {
            StartCoroutine(PickupWiden());
        }
    }

    IEnumerator PickupWiden()
    {
        magnetEnabled = true;
        temporaryBuffText.SetActive(true);
        temporaryBuffText.GetComponent<Text>().text = "ITEM MAGNET ENABLED";
        float prevPickupDistance = MaxPickupDistance;
        MaxPickupDistance *= 2;
        Debug.LogFormat("MaxPickupDistance: {0};", MaxPickupDistance);
        yield return new WaitForSeconds(pickIncreaseTime);
        MaxPickupDistance = prevPickupDistance;
        Debug.LogFormat("MaxPickupDistance: {0};",MaxPickupDistance);
        temporaryBuffText.SetActive(false);
        magnetEnabled = false;
    }

    public void IncreaseSpeed()
    {
        if (!spedUp)
        {
            StartCoroutine(SpeedUp());
        }
    }

    IEnumerator SpeedUp()
    {
        spedUp = true;
        temporaryBuffText.SetActive(true);
        temporaryBuffText.GetComponent<Text>().text = "SPEED BOOST ENABLED";
        int prevSpeed = Speed;
        Speed *= 2;
        yield return new WaitForSeconds(speedIncreaseTime);
        Speed = prevSpeed;
        temporaryBuffText.SetActive(false);
        spedUp = false;
    }

    public void PlayGame()
    {
        playButton.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
        Time.timeScale = 1;
        clickSound.Post(gameObject);
        StartCoroutine(DelayPlayGame());
    }

    IEnumerator DelayPlayGame()
    {
        yield return new WaitForSeconds(0.04f);
        playButton.GetComponentInChildren<Text>().alignment = TextAnchor.UpperCenter;
        //SceneManager.LoadScene("MainGame");

        gamePaused = false;
        pauseScreen.SetActive(false);

    }

    public void QuitGame()
    {
        quitButton.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
        Time.timeScale = 1;
        clickSound.Post(gameObject);
        StartCoroutine(DelayQuitGame());
    }

    IEnumerator DelayQuitGame()
    {
        yield return new WaitForSeconds(0.04f);
        quitButton.GetComponentInChildren<Text>().alignment = TextAnchor.UpperCenter;
        //SceneManager.LoadScene("Options");
        Util.LoadScene(SCENE.MENU);
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