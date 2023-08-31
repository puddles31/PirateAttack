using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    private List<PlayerStat> statsList;

    [SerializeField]
    private PlayerStat maxHealth, cannonballDamage, cannonballSpeed, fireRate, moveSpeed;

    private System.Random rnd;
    private PlayerController playerController;


    private void Awake() {
        playerController = gameObject.GetComponent<PlayerController>();

        maxHealth = new(0, "Max Health", null, () => { playerController.IncreaseMaxHealth(1); });
        cannonballDamage = new(1, "Cannonball Damage", null, () => { playerController.IncreaseCannonballDamage(1); });
        cannonballSpeed = new(2, "Cannonball Speed", null, () => { playerController.IncreaseCannonballSpeed(5); });
        fireRate = new(3, "Fire Rate", null, () => { playerController.DecreaseShootCooldown(0.12f); });
        moveSpeed = new(4, "Move Speed", null, () => { playerController.IncreaseAcceleration(2); playerController.IncreaseAngularAcceleration(0.3f); });

        statsList = new List<PlayerStat>() { maxHealth, cannonballDamage, cannonballSpeed, fireRate, moveSpeed };

        rnd = new System.Random();
    }

    public List<PlayerStat> RandomSelection(int n) {
        return statsList.OrderBy(x => rnd.Next()).Take(n).ToList();
    }

}

[Serializable]
public class PlayerStat : Enumeration {

    public PlayerStat(int id, string statName, Sprite sprite, Action levelIncreaseAction) : base(id, statName) {
        this.levelIncreaseAction = levelIncreaseAction;
        this.sprite = sprite;
    }

    private const string TYPENAME = "Player Stat";

    private Sprite sprite;
    private Action levelIncreaseAction;

    [SerializeField]
    private int statLevel = 0;
    public int StatLevel { get { return statLevel; } }

    public void IncreaseLevel() {
        if (statLevel < 5) {
            statLevel++;
            levelIncreaseAction();
        }
    }

    public ShopItem ToShopItem() {
        return new ShopItem(TYPENAME, Name, 50, sprite, IncreaseLevel);
    }
}

