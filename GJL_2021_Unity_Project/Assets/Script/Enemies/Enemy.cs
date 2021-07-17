using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
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
    public GameObject playerGO;
    
    // some of these might be better public idk
    const int  maxHealth = 1;
    const int defaultSpeed = 10;
    const int maxArmour = 1;
    int health = maxHealth;
    public int armour = 0;

    private void Awake()  {
        playerGO = GameObject.Find("Player");
        if(playerGO == null){
            Debug.Log("Player not found by name");
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
    }

    private void KillEnemy(){
        // do other killing stuff here
        Destroy(gameObject);
    }
}