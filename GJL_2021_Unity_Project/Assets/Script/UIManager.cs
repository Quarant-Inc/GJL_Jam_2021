using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text lblHealth;
    public Text lblSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        SetHealth(Player.Instance.Health);
        SetSpeed(Player.Instance.Speed);
        Player.Instance.HealthChanged += SetHealth;
        Player.Instance.SpeedChanged += SetSpeed;
    }

    void SetHealth(int health)
    {
        if (lblHealth != null)
        {
            lblHealth.text = "Health: "+health;
        }
        else
        {
            Debug.Log("lblHealth is null you silly goose.");
        }
    }

    void SetSpeed(int speed)
    {
        if (lblSpeed != null)
        {
            lblSpeed.text = "Speed: "+speed;
        }
        else
        {
            Debug.Log("lblSpeed is null you absolute nob head.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}