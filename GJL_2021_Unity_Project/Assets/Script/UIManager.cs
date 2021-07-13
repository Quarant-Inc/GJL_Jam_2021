using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text lblHealth;
    public Text lblSpeed;
    public Text lblArmour;

    public Text lblItemQueue;

    static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetHealth(Player.Instance.Health);
        SetSpeed(Player.Instance.Speed);
        SetArmour(0);

        Player.Instance.HealthChanged += SetHealth;
        Player.Instance.SpeedChanged += SetSpeed;
        Player.Instance.ArmourChanged += SetArmour;
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

    void SetArmour(int armour)
    {
        if (lblArmour != null)
        {
            lblArmour.text = "Armour: "+armour;
        }
        else
        {
            Debug.Log("lblArmour is non-existent :/");
        }
    }

    List<string> items= new List<string>();

    public void AddItem(Item item)
    {
        items.Add(item.gameObject.name);
        if (lblItemQueue != null)
        {
            string text = "Item Queue: ";
            foreach(string itemStr in items)
            {
                text += itemStr + ",";
            }
            lblItemQueue.text = text;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}