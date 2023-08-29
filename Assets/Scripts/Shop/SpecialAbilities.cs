using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialAbilities : MonoBehaviour {

    private List<SpecialAbility> specialAbilityList;

    private SpecialAbility heavyArmour;

    private System.Random rnd;


    private void Start() {
        heavyArmour = new(0, "Heavy Armour", 30, () => { Debug.Log("ARMOUR"); });

        specialAbilityList = new List<SpecialAbility>() { heavyArmour };

        rnd = new System.Random();
    }

    public List<SpecialAbility> RandomSelection(int n) {
        return specialAbilityList.OrderBy(x => rnd.Next()).Take(n).ToList();
    }

}


public class SpecialAbility : Enumeration {

    public SpecialAbility(int id, string specialAbilityName, int cooldownTimer, Action specialAbilityAction) : base(id, specialAbilityName) {
        this.cooldownTimer = cooldownTimer;
        this.specialAbilityAction = specialAbilityAction;
    }

    private const string TYPENAME = "Special Ability";

    private PlayerController player;
    private Action specialAbilityAction;

    private int cooldownTimer;


    public void SetupSpecialAbility() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (player != null) {
            player.SetSpecialAbility(cooldownTimer, specialAbilityAction);
        }
    }

    public ShopItem ToShopItem() {
        return new ShopItem(TYPENAME, Name, 150, SetupSpecialAbility);
    }
}

