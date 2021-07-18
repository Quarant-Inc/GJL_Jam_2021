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
            Swipe();
            yield return new WaitForSeconds(1f/attackSpeed);
        }
    }

    void Swipe()
    {
        Animator.SetTrigger(ENEMY_ANIMATION.ATTACK.ToString());
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach(Collider col in nearbyColliders)
        {
            if (col.tag == TAG.Player.ToString())
            {
                Player.Instance.TakeDamage();
                break;
            }
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