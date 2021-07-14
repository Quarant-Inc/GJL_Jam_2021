using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{

    public GameObject heart;
    public List<Image> hearts;

    int maxHealth = -1;

    // Start is called before the first frame update
    void Start()
    {
        // draw all hearts
        maxHealth = Player.Instance.MaxHealth;
        for (var i = 0; i < maxHealth; i++)
        {
            GameObject h = Instantiate(heart, this.transform);
            hearts.Add(h.GetComponent<Image>());
        }

        Player.Instance.HealthChanged += UpdateHearts;
    }

    void UpdateHearts(int health)
    {
        int heartFill = health;

        foreach (Image i in hearts)
        {
            i.fillAmount = heartFill;
            heartFill --;
        }
        
    }

    // TODO ?? add hearts function for increasing the max hearts if required

    // Update is called once per frame
    void Update()
    {
        
    }
}
