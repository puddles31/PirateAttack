using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : ShopItem
{
    public Consumable(string itemName, int itemPrice) {
        this.itemName = itemName;
        this.itemPrice = itemPrice;
        itemType = "Consumable";
    }
}