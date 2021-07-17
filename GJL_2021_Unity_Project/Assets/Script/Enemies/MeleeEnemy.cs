using System.Collections;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public float attackSpeed;

    bool attacking = false;

    IEnumerator AttackLoop()
    {
        attacking = true;
        while(attacking)
        {
            yield return new WaitForSeconds(1f/attackSpeed);
        }
    }

    protected override void Attack()
    {
        StartCoroutine(AttackLoop());
    }
    protected override void StopAttack()
    {
        attacking = false;
    }
}