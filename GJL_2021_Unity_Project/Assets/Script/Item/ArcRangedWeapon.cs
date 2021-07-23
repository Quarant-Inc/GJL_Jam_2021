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
            
            RaycastHit hit;
            Ray ray = CameraFollow.Instance.Camera.ScreenPointToRay(Input.mousePosition);

            Vector3 forward;
            if (Util.GetCursorFromPlayerDir(out forward))
            {
                Vector3 right = Quaternion.AngleAxis(90f, Vector3.up) * forward;
                Debug.DrawRay(Player.Instance.transform.position,right, Color.red, 10);
                //Vector3 right = Player.Instance.transform.right;
                Vector3 trajectory = (Quaternion.AngleAxis(-arcAxis, right) * forward).normalized;
                Debug.LogFormat("Trajectory: {0};", trajectory);
                ARWDeployed deployedScript = weapon.GetComponent<ARWDeployed>();
                if (deployedScript != null)
                {
                    deployedScript.Fire(start, trajectory, detonationDelay, maxDistance);
                }
                Player.Instance.LookAt(start + forward,0.7f);
                Player.Instance.SetAnimation(PLAYER_ANIM_PARAMS.THROW);
                
            }

            /*if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(CameraFollow.Instance.transform.position, hit.point,Color.blue, 10);
                //GameObject.CreatePrimitive(PrimitiveType.Capsule).transform.position = hit.point;
                Debug.LogFormat("ARW hit collider: {0};",hit.collider.name);
                Vector3 forward = (hit.point - Player.Instance.transform.position).normalized;
                Vector3 right = Quaternion.AngleAxis(90f, Vector3.up) * forward;
                Debug.DrawRay(Player.Instance.transform.position,right, Color.red, 10);
                //Vector3 right = Player.Instance.transform.right;
                Vector3 trajectory = (Quaternion.AngleAxis(-arcAxis, right) * forward).normalized;
                Debug.LogFormat("Trajectory: {0};", trajectory);
                ARWDeployed deployedScript = weapon.GetComponent<ARWDeployed>();
                if (deployedScript != null)
                {
                    deployedScript.Fire(start, trajectory, detonationDelay, maxDistance);
                }
            }*/
            

        }
    }
}