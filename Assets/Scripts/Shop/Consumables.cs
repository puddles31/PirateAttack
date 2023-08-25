using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Consumables : MonoBehaviour {

    private List<Consumable> consumablesList;

    public Consumable healthKit;
    public Consumable ammoBox;

    private System.Random rnd;

    private void Start() {
        healthKit = new(0, "Health Kit", () => { Debug.Log("HEALTH KIT!"); });
        ammoBox = new(1, "Ammo Box", () => { Debug.Log("AMMO BOX!"); });

        consumablesList = new List<Consumable>() { healthKit, ammoBox };

        rnd = new System.Random();
    }

    public List<Consumable> RandomSelection(int n) {
        return consumablesList.OrderBy(x => rnd.Next()).Take(n).ToList();
    }

}


public class Consumable : Enumeration {

    public Consumable(int id, string consumableName, Action consumableAction) : base(id, consumableName) {
        this.consumableAction = consumableAction;
    }

    private const string TYPENAME = "Consumable";

    private Action consumableAction;

    public ShopItem ToShopItem() {
        return new ShopItem(TYPENAME, Name, 10, consumableAction);
    }
}

