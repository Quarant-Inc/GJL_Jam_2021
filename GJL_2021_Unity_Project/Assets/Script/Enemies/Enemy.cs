 using UnityEngine;

public class Enemy : MonoBehaviour {   
    
    public GameObject enemy;
    
    // some of these might be better public idk
    const int  maxHealth = 1;
    const int defaultSpeed = 10;
    const int maxArmour = 1;
    int health = 1;
    int armour = 0;

    void Update()    {
        // this just to see if it works
        if (Input.GetMouseButtonDown(1))
        {
            DamageEnemy();
        }
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
        Debug.Log(""+ armour + health);
    }

    private void KillEnemy(){
        // do other killing stuff here
        Destroy(enemy);
    }
}