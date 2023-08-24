using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopItem
{

    protected string itemType;
    public string Type { get { return itemType; } }

    protected string itemName;
    public string Name { get { return itemName; } }

    protected int itemPrice;
    public int Price { get { return itemPrice; } }

}
