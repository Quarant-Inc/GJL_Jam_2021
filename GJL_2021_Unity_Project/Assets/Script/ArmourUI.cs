using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmourUI : MonoBehaviour
{

    public GameObject armour;
    public List<Image> armours;

    // Start is called before the first frame update
    void Start()
    {
        AddArmour(Player.Instance.Armour);
        Player.Instance.ArmourChanged += AddArmour;
    }


    void AddArmour(int armourCount)
    {
        // reset list
        foreach(Image i in armours)
        {
            if (i !=  null && i.gameObject != null)
            {
                Destroy(i.gameObject);
            }
        }
        armours.Clear();

        // (re)draw all hearts for max health
        for (var i = 0; i < armourCount; i++)
        {
            GameObject a = Instantiate(armour, this.transform);
            armours.Add(a.GetComponent<Image>());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
