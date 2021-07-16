using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ARWDeployed : MonoBehaviour
{
    Rigidbody body;
    Rigidbody Body
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

    bool collided = false;

    public void Fire(Vector3 trajectory, float detonationDelay, float maxDistance)
    {
        Vector3 start = transform.position;
        Body.AddForce(trajectory*5);
        StartCoroutine(Detonation(detonationDelay,maxDistance, start));
    }

    IEnumerator Detonation(float delay, float dist, Vector3 startPos)
    {
        float t = 0;
        while (t < delay && !collided)
        {
            t += Time.deltaTime;
            if (Vector3.Distance(transform.position, startPos) > dist)
            {
                break;
            }
            yield return 0;
        }
        Explode();
    }

    void OnCollisionEnter(Collision col)
    {
        collided = true;
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, 0, QueryTriggerInteraction.Ignore);
        foreach(Collider col in colliders)
        {
            if (col.tag == TAG.Enemy.ToString())
            {
                Enemy enemyScript = col.gameObject.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.DamageEnemy();
                }
            }
        }
        //Implement explosion
        Destroy(gameObject);
    }
}