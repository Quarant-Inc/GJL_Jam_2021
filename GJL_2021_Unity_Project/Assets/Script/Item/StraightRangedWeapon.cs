using UnityEngine;

[System.Serializable]
public class StraightRangedWeapon : Weapon
{
    public int penetrationLevel;
    public float chargeDelay;
    public float rayWidth;

    public override void Use()
    {
        if (itemPrefab != null)
        {
            GameObject weapon = GameObject.Instantiate(itemPrefab);
            //Do stuff with weapon object
        }

        Vector3 forward;
        if (Util.GetCursorFromPlayerDir(out forward))
        {
            //Vector3 forward = Player.Instance.transform.forward;
            Vector3 pos = Player.Instance.transform.position;

            Debug.Log("Is this being used?");

            //TODO: Fix bug which makes penetration only work once


            /*for (int i = 0; i < penetrationLevel; i++)
            {
                //RaycastHit hit;
                RaycastHit[] hits = Physics.SphereCastAll(pos, rayWidth, forward, 100,0);
                foreach(RaycastHit hit in hits)
                {
                    if (hit.collider.tag == TAG.Enemy.ToString())
                    {
                        Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                        if (enemyScript != null)
                        {
                            enemyScript.DamageEnemy();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }*/

            RaycastHit hit;
            if (Physics.SphereCast(pos, rayWidth,forward, out hit))
            {
                if (hit.collider.tag == TAG.Enemy.ToString())
                {
                    Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.DamageEnemy();
                    }
                }

            }
        }
    }
}