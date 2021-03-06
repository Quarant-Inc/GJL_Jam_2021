using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public delegate void EnemyKilled();
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{
    public static EnemyKilled EnemyKilled;
    enum ENEMY_STATE
    {
        IDLE,
        CHASING,
        SEARCHING,
        ATTACKING
    }

    protected enum ENEMY_ANIMATION
    {
        IDLE,
        ATTACK,
        DAMAGED,
        DEATH,
        RUN
    }

    #region Components
    Animator animator;
    protected Animator Animator
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
    NavMeshAgent agent;
    protected NavMeshAgent Agent
    {
        get
        {
            if (agent == null)
            {
                return agent = GetComponent<NavMeshAgent>();
            }
            return agent;
        }
    }
    #endregion

    protected Vector3 PlayerPosition
    {
        get
        {
            return Player.Instance.transform.position;
        }
    }

    #region Stats
    
    // some of these might be better public idk
    const int  maxHealth = 1;
    const int defaultSpeed = 10;
    const int maxArmour = 1;
    int health = maxHealth;
    public int armour = 0;

    public float sightRadius = 10f;
    public float attackRange;

    #endregion



    ENEMY_STATE currentState = ENEMY_STATE.IDLE;

    public bool moveable = true;

    private void Awake()
    {

    }

    void Start()
    {
        if (moveable)
        {
            StartCoroutine(PerformStateCheck());
        }
    }

    void Update()    {
        // this just to see if it works
        /*if (Input.GetMouseButtonDown(1))
        {
            DamageEnemy();
        }*/
    }

    public void DamageEnemy(){
        if(armour > 0){
            armour --;
        } else {
            health --;
        }

        if(health <= 0){
            KillEnemy();
        }
        Debug.LogFormat("Armour: {0}. Health: {1}.", armour, health);
        SetAnimation(ENEMY_ANIMATION.DAMAGED);
        //Animator.SetTrigger(ENEMY_ANIMATION.DAMAGED.ToString());
    }

    private void KillEnemy()
    {
        AkSoundEngine.PostEvent("Goblin_Death_Sound", gameObject);
        SetAnimation(ENEMY_ANIMATION.DEATH);
        //Animator.SetTrigger(ENEMY_ANIMATION.DEATH.ToString());
        // do other killing stuff here
        Destroy(gameObject);
        if (EnemyKilled != null)
        {
            EnemyKilled();
        }
    }

    void SetAnimation(ENEMY_ANIMATION anim)
    {
        Animator.SetTrigger(anim.ToString());
    }

    protected abstract void Attack();
    protected abstract void StopAttack();

    Vector3 lastSeenPos;

    const float stateCheckInterval = 0.125f;

    IEnumerator EnemyFinishSpeaking()
    {
        yield return new WaitForSeconds(2f);
        Player.enemySpeaking = false;
    }


    void StateCheck()
    {
        //Debug.LogFormat("{0} state is {1}",name,currentState);
        switch(currentState)
        {
            case ENEMY_STATE.IDLE:
            {
                if (PlayerVisible())
                {
                    lastSeenPos = PlayerPosition;
                    currentState = ENEMY_STATE.CHASING;
                        if (Player.enemySpeaking == false)
                        {
                            if (Random.Range(0, 1) > 0)
                            {
                                AkSoundEngine.PostEvent("Human_Detected", gameObject);
                            }
                            else
                            {
                                AkSoundEngine.PostEvent("Goblin_Detect", gameObject);
                            }
                            Player.enemySpeaking = true;
                            StartCoroutine(EnemyFinishSpeaking());
                        }
                    SetAnimation(ENEMY_ANIMATION.RUN);
                }
                break;
            }
            case ENEMY_STATE.CHASING:
            {
                if (!PlayerVisible())
                {
                    currentState = ENEMY_STATE.SEARCHING;
                        if (Player.enemySpeaking == false)
                        {
                            if (Random.Range(0, 1) > 0)
                            {
                                AkSoundEngine.PostEvent("Human_Lost", gameObject);
                            }
                            else
                            {
                                AkSoundEngine.PostEvent("Goblin_Lost_Sight", gameObject);
                            }
                            Player.enemySpeaking = true;
                            StartCoroutine(EnemyFinishSpeaking());
                        }
                    }
                else
                {
                    lastSeenPos = PlayerPosition;
                    Agent.SetDestination(lastSeenPos);
                    if (PlayerWithinAttackRange())
                    {
                        currentState = ENEMY_STATE.ATTACKING;
                            if (Player.enemySpeaking == false)
                            {

                                AkSoundEngine.PostEvent("Robot_Kill_or_Hit_Player", gameObject);
                                Player.enemySpeaking = true;
                                StartCoroutine(EnemyFinishSpeaking());
                            }
                        Attack();
                    }
                }
                break;
            }
            case ENEMY_STATE.SEARCHING:
            {
                if (PlayerVisible())
                {
                    currentState = ENEMY_STATE.CHASING;
                    lastSeenPos = PlayerPosition;
                }
                else if (Vector3.Distance(lastSeenPos, transform.position) < 1.5f)
                {
                    currentState = ENEMY_STATE.IDLE;
                    SetAnimation(ENEMY_ANIMATION.IDLE);
                }
                break;
            }
            case ENEMY_STATE.ATTACKING:
            {
                if (!PlayerWithinAttackRange())
                {
                    currentState = ENEMY_STATE.CHASING;
                    StopAttack();
                }
                break;
            }
        }
    }

    bool PlayerVisible()
    {
        return Vector3.Distance(transform.position, PlayerPosition) <= sightRadius;
    }

    bool PlayerWithinAttackRange()
    {
        return Vector3.Distance(transform.position, PlayerPosition) <= attackRange;
    }

    bool runningStateCheck = true;

    IEnumerator PerformStateCheck()
    {
        while(runningStateCheck)
        {
            StateCheck();
            yield return new WaitForSeconds(stateCheckInterval);
        }
    }
}