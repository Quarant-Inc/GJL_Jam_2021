using UnityEngine;

[System.Serializable]
public class ArcRangedWeapon : Weapon
{
    public float maxDistance;
    public float arcAxis;
    public float detonationDelay;
    public override void Use()
    {
        if (itemPrefab != null)
        {
            GameObject weapon = GameObject.Instantiate(itemPrefab);
            weapon.SetActive(true);
            Vector3 start = Player.Instance.transform.position;
            start.y++;
            Vector3 forward = Player.Instance.transform.forward;
            Vector3 right = Player.Instance.transform.right;
            Vector3 trajectory = (Quaternion.AngleAxis(-arcAxis, right) * forward).normalized;
            ARWDeployed deployedScript = weapon.GetComponent<ARWDeployed>();
            if (deployedScript != null)
            {
                deployedScript.Fire(start, trajectory, detonationDelay, maxDistance);
            }
        }
    }
}