using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : ShopItem {
    public PlayerStat(string itemName, int itemPrice) {
        this.itemName = itemName;
        this.itemPrice = itemPrice;
        itemType = "Player Stat";
    }
}