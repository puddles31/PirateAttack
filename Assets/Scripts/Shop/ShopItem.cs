using System;
using System.Collections;
using UnityEngine;

public class ShopItem
{

    public string Type { get; private set; }
    public string Name { get; private set; }

    public int Price { get; private set; }
    public Sprite ItemSprite { get; private set; }

    private Action purchaseAction;

    public ShopItem(string type, string name, int price, Sprite sprite, Action purchaseAction) {
        Type = type;
        Name = name;
        Price = price;
        ItemSprite = sprite;
        this.purchaseAction = purchaseAction;
    }

    public void OnPurchase() {
        purchaseAction();
    }
}
