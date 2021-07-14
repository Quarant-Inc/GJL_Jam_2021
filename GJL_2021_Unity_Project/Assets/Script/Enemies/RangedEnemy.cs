 using UnityEngine;

public class RangedEnemy : Enemy {   
    const float range = 10f;
    float distanceToPlayer = -1;

    const float fireRate = 1f; // rate of fire
    const int clipSize = 5; // how many shots before reload
    int shotsRemaining = clipSize;
    const float reloadTime = 3f;

    private float nextFire = 0f;

    private void Update() {
        distanceToPlayer = Vector3.Distance(playerGO.transform.position, transform.position);

        if(distanceToPlayer <= range) // within range, shoot
        {
            ShootWeapon();
        }
    }
    

    private void ShootWeapon()
    {
        if (Time.time > nextFire)
        {
            if(shotsRemaining <= 0)
            {
                nextFire = Time.time + reloadTime;
                shotsRemaining = clipSize;
                Debug.LogFormat("Reloading for {0}s", reloadTime);
            } else
            {
                nextFire = Time.time + fireRate;
                shotsRemaining--;
                // TODO handle actual firing/damage to player
                Debug.LogFormat("BANG ! Fired Weapon. {0}/{1} Shots remaining", shotsRemaining, clipSize);
            }

            
        }
    }

}