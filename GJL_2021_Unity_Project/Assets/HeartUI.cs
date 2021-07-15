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
        AddHearts(Player.Instance.MaxHealth);
        Player.Instance.MaxHeathChanged += AddHearts;
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
    void AddHearts(int maxHealth)
    {
        // reset list
        foreach(Image i in hearts)
        {
            Destroy(i.gameObject);
        }
        hearts.Clear();

        // (re)draw all hearts for max health
        //maxHealth = Player.Instance.MaxHealth;
        for (var i = 0; i < maxHealth; i++)
        {
            GameObject h = Instantiate(heart, this.transform);
            hearts.Add(h.GetComponent<Image>());
        }

        UpdateHearts(Player.Instance.Health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
