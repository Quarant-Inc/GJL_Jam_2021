using UnityEngine;

[System.Serializable]
public class MeleeWeapon : Weapon
{
    public float range;
    public float coneAngle;
    public bool penetrative;
    public override void Use()
    {
        Debug.Log("MeleeWeapon.Use();");
        /*Collider[] nearbyColliders = Physics.OverlapSphere(Player.Instance.transform.position,range);
        foreach(Collider col in nearbyColliders)
        {
            Debug.LogFormat("Melee hit {0}",col.name);
            if (col.tag == TAG.Enemy.ToString())
            {
                Enemy enemy = col.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy();
                }
            }
        }
        */
        Vector3 origin = Player.Instance.transform.position;
        origin.y += 0.5f;
        Vector3 forward = Player.Instance.transform.forward;
        Vector3 leftConeEdge = Quaternion.AngleAxis(-(coneAngle/2f), Vector3.up) * forward;
        Vector3 rightConeEdge = Quaternion.AngleAxis((coneAngle / 2f), Vector3.up) * forward;

        CheckHit(forward, origin);
        CheckHit(leftConeEdge, origin);
        CheckHit(rightConeEdge, origin);
    }

    void CheckHit(Vector3 dir, Vector3 origin)
    {
        Debug.DrawRay(origin, dir, Color.blue,10);
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit))
        {
            Debug.LogFormat("Melee hit {0}", hit.collider.name);
            if (hit.collider.tag == TAG.Enemy.ToString())
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy();
                }
            }
        }
    }
}