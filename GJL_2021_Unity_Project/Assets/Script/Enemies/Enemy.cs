using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
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

    Vector3 PlayerPosition
    {
        get
        {
            return Player.Instance.transform.position;
        }
    }
    
    // some of these might be better public idk
    const int  maxHealth = 1;
    const int defaultSpeed = 10;
    const int maxArmour = 1;
    int health = maxHealth;
    public int armour = 0;

    public float sightRadius = 10f;
    public float attackRange;

    enum ENEMY_STATE
    {
        IDLE,
        CHASING,
        SEARCHING,
        ATTACKING
    }

    ENEMY_STATE currentState = ENEMY_STATE.IDLE;

    private void Awake()
    {

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
    }

    private void KillEnemy(){
        // do other killing stuff here
        Destroy(gameObject);
    }

    protected abstract void Attack();
    protected abstract void StopAttack();

    Vector3 lastSeenPos;

    void StateCheck()
    {
        switch(currentState)
        {
            case ENEMY_STATE.IDLE:
            {
                if (PlayerVisible())
                {
                    lastSeenPos = PlayerPosition;
                    currentState = ENEMY_STATE.CHASING;
                }
                break;
            }
            case ENEMY_STATE.CHASING:
            {
                if (!PlayerVisible())
                {
                    currentState = ENEMY_STATE.SEARCHING;
                }
                else
                {
                    lastSeenPos = PlayerPosition;
                    Agent.SetDestination(lastSeenPos);
                    if (PlayerWithinAttackRange())
                    {
                        currentState = ENEMY_STATE.ATTACKING;
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

    bool checkingForPlayer = true;

    IEnumerator CheckForPlayer()
    {
        //This desperately needs to be optimised.
        while(checkingForPlayer)
        {

            yield return 0;
        }
    }
}