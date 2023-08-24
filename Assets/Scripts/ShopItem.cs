using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    private enum ItemType {
        PlayerStat,
        AltFire,
        SpecialAbility,
        Consumable
    }

    [SerializeField]
    private ItemType type;
    public string Type { get { return TypeToString(); } }

    private string itemName;
    public string Name { get { return itemName; } }

    [SerializeField]
    private int price;
    public int Price { get { return price; } }




    private string TypeToString() {
        switch (type) {
            case ItemType.PlayerStat:
                return "Player Stat";

            case ItemType.AltFire:
                return "Alt-Fire";

            case ItemType.SpecialAbility:
                return "Special Ability";

            case ItemType.Consumable:
                return "Consumable";

            default:
                return "ERROR: Invalid item type";
        }
    }
}
