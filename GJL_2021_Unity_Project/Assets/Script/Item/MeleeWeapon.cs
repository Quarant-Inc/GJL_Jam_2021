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
        Collider[] nearbyColliders = Physics.OverlapSphere(Player.Instance.transform.position,range);
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

    }
}