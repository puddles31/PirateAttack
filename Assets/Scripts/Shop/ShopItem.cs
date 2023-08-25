using System;
using System.Collections;
using UnityEngine;

public class ShopItem
{

    public string Type { get; private set; }
    public string Name { get; private set; }

    public int Price { get; private set; }

    private Action purchaseAction;

    public ShopItem(string type, string name, int price, Action purchaseAction) {
        Type = type;
        Name = name;
        Price = price;
        this.purchaseAction = purchaseAction;
    }

    public void OnPurchase() {
        purchaseAction();
    }
}
