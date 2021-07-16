using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    Collider thisCollider;
    Collider Collider
    {
        get
        {
            if (thisCollider == null)
            {
                return thisCollider =GetComponent<Collider>();
            }
            return thisCollider;
        }
    }

    void Start()
    {
        //Ignore collisions with player
        Collider[] playerColliders = Player.Instance.GetComponents<Collider>();
        foreach(Collider playerCol in playerColliders)
        {
            Physics.IgnoreCollision(playerCol, Collider);
        }
    }

    bool collided = false;
    bool fired = false;
    Vector3 prevPosition;

    public void Fire(Vector3 start, Vector3 trajectory, float detonationDelay, float maxDistance)
    {
        transform.position = start;
        //Body.useGravity = false;
        Body.velocity = trajectory*10;
        fired = true;
        StartCoroutine(Detonation(detonationDelay,maxDistance, start));
        Debug.DrawRay(start, trajectory, Color.blue, 10);
    }

    IEnumerator Detonation(float delay, float dist, Vector3 startPos)
    {
        prevPosition = transform.position;
        float t = 0;
        while (t < delay && !collided)
        {
            t += Time.deltaTime;
            Debug.DrawLine(prevPosition,transform.position, Color.blue, 10);
            prevPosition = transform.position;
            if (Vector3.Distance(transform.position, startPos) > dist)
            {
                Debug.Log("Distance achieved.");
                break;
            }
            yield return 0;
        }
        Explode();
    }

    Collider ogCollider;

    void OnCollisionEnter(Collision col)
    {
        if (fired)
        {
            collided = true;
            Debug.LogFormat("Object hit {0}",col.gameObject.name);
            ogCollider = col.collider;
        }
    }

    void Explode()
    {
        Debug.Log("Explode()");

        //Body.useGravity = false;
        //Body.velocity = new Vector3();


        //Physics.OverlapSphere()
        
        List<Collider> colliders = Physics.OverlapSphere(transform.position, 5f, 0).ToList();
        if (ogCollider != null)
        {
            colliders.Add(ogCollider);
        }
        Debug.LogFormat("Collider count: {0};", colliders.Count);
        foreach(Collider col in colliders)
        {
            if (col.tag == TAG.Enemy.ToString())
            {
                Debug.Log("Hit enemy");
                Enemy enemyScript = col.gameObject.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    Debug.Log("Damaging enemy");
                    enemyScript.DamageEnemy();
                }
                else
                {
                    Debug.Log("Enemy didn't have a script :/");
                }
            }
        }
        //Implement explosion
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}