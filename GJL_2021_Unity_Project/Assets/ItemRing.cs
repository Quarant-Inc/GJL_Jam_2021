using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRing : MonoBehaviour
{
    public int RingIndex = 0; // what position in the player inventory to look at
    public Image Icon = null;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(name + "item ring");
        Icon.enabled = false;
        Player.Instance.ItemPickedUp += RedrawRing;
        Player.Instance.ItemUsed += RedrawRing;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RedrawRing()
    {
        var items = Player.Instance.Items;
        if (items.Count > RingIndex){
            Debug.LogFormat("Item at index {0}: {1}", RingIndex, Player.Instance.Items[RingIndex].inventorySprite);
            Icon.sprite = Player.Instance.Items[RingIndex].inventorySprite;
            Icon.enabled = true;
        } else
        {
            Icon.enabled = false;
            Icon.sprite = null;
        }
        
    }
}
