using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialAbilities : MonoBehaviour {

    private List<SpecialAbility> specialAbilityList;

    private SpecialAbility heavyArmour, dash, cannonBoost, bulletTime;

    [SerializeField]
    private Sprite heavyArmourSprite, dashSprite, cannonBoostSprite, bulletTimeSprite;

    private int heavyArmourDuration = 10, heavyArmourCooldown = 30;
    private int dashForce = 15, dashCooldown = 6;
    private int cannonBoostDuration = 10, cannonBoostCooldown = 30;
    private float bulletTimeStrength = 0.25f;
    private int bulletTimeDuration = 10, bulletTimeCooldown = 30;

    private System.Random rnd;
    private PlayerController player;
    private UIManager uiManager;
    private SpawnManager spawnManager;


    private void Awake() {
        rnd = new System.Random();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();

        heavyArmour = new(0, "Heavy Armour", heavyArmourCooldown, heavyArmourSprite, () => { StartCoroutine(HeavyArmourAction()); });
        dash = new(1, "Dash", dashCooldown, dashSprite, () => { DashAction(); });
        cannonBoost = new(2, "Cannon Boost", cannonBoostCooldown, cannonBoostSprite, () => { StartCoroutine(CannonBoostAction()); });
        bulletTime = new(3, "Bullet Time", bulletTimeCooldown, bulletTimeSprite, () => { StartCoroutine(BulletTimeAction()); });

        specialAbilityList = new List<SpecialAbility>() { heavyArmour, dash, cannonBoost, bulletTime };
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

    private void DashAction() {
        int horizontalForce = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        int verticalForce = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);

        if (verticalForce == 0 && horizontalForce == 0) {
            verticalForce = 1;
        }

        Rigidbody playerRb = player.gameObject.GetComponent<Rigidbody>();

        playerRb.velocity = Vector3.zero;

        Vector3 forceDir = (verticalForce * Vector3.forward) + (horizontalForce * Vector3.right);
        playerRb.AddRelativeForce(forceDir.normalized * dashForce, ForceMode.VelocityChange);

        StartCoroutine(player.SpecialAbilityCooldownTimer());
    }

    private IEnumerator CannonBoostAction() {
        player.IncreaseCannonballDamage(3);
        player.IncreaseCannonballSpeed(15);
        player.DecreaseShootCooldown(0.25f);
        uiManager.SetSpecialAbilityOutlineActive(true);

        yield return new WaitForSeconds(cannonBoostDuration);

        player.DecreaseCannonballDamage(3);
        player.DecreaseCannonballSpeed(15);
        player.IncreaseShootCooldown(0.25f);
        uiManager.SetSpecialAbilityOutlineActive(false);

        StartCoroutine(player.SpecialAbilityCooldownTimer());
    }

    private IEnumerator BulletTimeAction() {

        foreach (var cannonballObj in GameObject.FindGameObjectsWithTag("Cannonball")) {
            var cannonball = cannonballObj.GetComponent<Cannonball>();

            if (!cannonball.IsFriendly) {
                cannonball.TimeModifier = bulletTimeStrength;
            }
        }

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            enemy.GetComponent<Enemy>().SetBulletTimeActive(true, bulletTimeStrength);
        }

        spawnManager.bulletTimeActive = true;
        spawnManager.bulletTimeStrength = bulletTimeStrength;

        uiManager.SetSpecialAbilityOutlineActive(true);

        yield return new WaitForSeconds(bulletTimeDuration);

        foreach (var cannonballObj in GameObject.FindGameObjectsWithTag("Cannonball")) {
            var cannonball = cannonballObj.GetComponent<Cannonball>();

            if (!cannonball.IsFriendly) {
                cannonball.TimeModifier = 1;
            }
        }

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            enemy.GetComponent<Enemy>().SetBulletTimeActive(false, bulletTimeStrength);
        }

        spawnManager.bulletTimeActive = false;

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
        return new ShopItem(TYPENAME, Name, 150, sprite, SetupSpecialAbility);
    }
}

