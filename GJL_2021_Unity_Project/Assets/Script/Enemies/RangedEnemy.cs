using System.Collections;
using UnityEngine;

public class RangedEnemy : Enemy {   
    //const float range = 10f;
    float distanceToPlayer = -1;

    const float fireRate = 1f; // rate of fire
    const int clipSize = 5; // how many shots before reload
    int shotsRemaining = clipSize;
    const float reloadTime = 3f;

    private float nextFire = 0f;

    private void Update() {
        // distanceToPlayer = Vector3.Distance(playerGO.transform.position, transform.position);

        // if(distanceToPlayer <= range) // within range, shoot
        // {
        //     ShootWeapon();
        // }
    }
    

    private float ShootWeapon()
    {
        // if (Time.time > nextFire)
        // {


            
        // }

        if(shotsRemaining <= 0)
        {
            //nextFire = Time.time + reloadTime;
            shotsRemaining = clipSize;
            Debug.LogFormat("Reloading for {0}s", reloadTime);
            return reloadTime;
        } else
        {
            //nextFire = Time.time + fireRate;
            shotsRemaining--;
            Fire();
            // TODO handle actual firing/damage to player
            Debug.LogFormat("BANG ! Fired Weapon. {0}/{1} Shots remaining", shotsRemaining, clipSize);
            return fireRate;
        }
    }

    void Fire()
    {
        Debug.Log("Fire()");
        Vector3 dir = (PlayerPosition - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, attackRange))
        {
            Debug.Log("Ray cast hit");
            if (hit.collider.tag == TAG.Player.ToString())
            {
                Player.Instance.TakeDamage();
            }
        }
        else
        {
            Debug.Log("Ray cast miss :'(");
        }
    }

    bool shooting = false;

    IEnumerator Shooting()
    {
        shooting = true;
        while(shooting)
        {
            yield return new WaitForSeconds(ShootWeapon());
        }
    }

    protected override void Attack()
    {
        StartCoroutine(Shooting());
        Agent.SetDestination(transform.position);
        //Agent.isStopped = true;
    }

    protected override void StopAttack()
    {
        shooting = false;
        //Agent.isStopped = false;
    }
}