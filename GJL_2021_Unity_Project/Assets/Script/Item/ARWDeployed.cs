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
}