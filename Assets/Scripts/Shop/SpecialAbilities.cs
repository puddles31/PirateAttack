using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialAbilities : MonoBehaviour {

    private List<SpecialAbility> specialAbilityList;

    private SpecialAbility heavyArmour;
    [SerializeField]
    private Sprite heavyArmourSprite;
    private float heavyArmourDuration = 10;

    private System.Random rnd;
    private PlayerController player;
    private UIManager uiManager;


    private void Start() {
        rnd = new System.Random();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        heavyArmour = new(0, "Heavy Armour", 30, heavyArmourSprite, () => { StartCoroutine(HeavyArmourAction()); });

        specialAbilityList = new List<SpecialAbility>() { heavyArmour };
    }

    public List<SpecialAbility> RandomSelection(int n) {
        return specialAbilityList.OrderBy(x => rnd.Next()).Take(n).ToList();
    }

    private IEnumerator HeavyArmourAction() {
        player.heavyArmourEnabled = true;
        uiManager.SetSpecialAbilityOutlineActive(true);
        yield return new WaitForSeconds(heavyArmourDuration);
        player.heavyArmourEnabled = false;
        uiManager.SetSpecialAbilityOutlineActive(false);
        StartCoroutine(player.SpecialAbilityCooldownTimer());
    }
}


public class SpecialAbility : Enumeration {

    public SpecialAbility(int id, string specialAbilityName, int cooldownTimer, Sprite sprite, Action specialAbilityAction) : base(id, specialAbilityName) {
        this.cooldownTimer = cooldownTimer;
        this.specialAbilityAction = specialAbilityAction;
        this.sprite = sprite;
    }

    private const string TYPENAME = "Special Ability";

    private PlayerController player;
    private UIManager uiManager;

    private int cooldownTimer;
    private Sprite sprite;
    private Action specialAbilityAction;

    public void SetupSpecialAbility() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        if (player != null) {
            player.SetSpecialAbility(cooldownTimer, specialAbilityAction);
            uiManager.SetSpecialAbilitySprite(sprite);
        }
    }

    public ShopItem ToShopItem() {
        return new ShopItem(TYPENAME, Name, 150, SetupSpecialAbility);
    }
}

