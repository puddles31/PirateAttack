using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AltFires : MonoBehaviour {

    private List<AltFire> altFireList;

    private AltFire homingMissiles, bombShot;

    [SerializeField]
    private GameObject bombShotPrefab;

    private int missilesAmmo = 5, missilesCooldown = 2;

    private int bombShotAmmo = 3, bombShotCooldown = 2, bombShotImpactDamage = 2, bombShotBlastDamage = 3;
    private float bombShotSpeed = 30, bombShotKnockbackForce = 10, bombShotBlastRadius = 15;

    private System.Random rnd;
    private PlayerController player;
    private UIManager uiManager;


    private void Awake() {
        rnd = new System.Random();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        homingMissiles = new(0, "Homing Missiles", missilesAmmo, missilesCooldown, null, (pos) => { HomingMissilesAction(pos); });
        bombShot = new(1, "Bomb Shot", bombShotAmmo, bombShotCooldown, null, (pos) => { BombShotAction(pos); });

        altFireList = new List<AltFire>() { bombShot };
    }

    public List<AltFire> RandomSelection(int n) {
        return altFireList.OrderBy(x => rnd.Next()).Take(n).ToList();
    }


    private void HomingMissilesAction(Vector3 mousePos) {
        Debug.Log("Boom");
    }

    private void BombShotAction(Vector3 mousePos) {
        // Get the vector from the player to the mouse position
        Vector3 towardsTarget = (mousePos - player.transform.position).normalized;
        towardsTarget.y = 0;

        // Create the bomb shot and set its speed and faction
        var bombShotTemp = Instantiate(bombShotPrefab, player.transform.position, Quaternion.LookRotation(towardsTarget)).GetComponent<BombShot>();
        bombShotTemp.Speed = bombShotSpeed;
        bombShotTemp.ImpactDamage = bombShotImpactDamage;
        bombShotTemp.BlastDamage = bombShotBlastDamage;
        bombShotTemp.KnockbackForce = bombShotKnockbackForce;
        bombShotTemp.BlastRadius = bombShotBlastRadius;

        player.bombShot = bombShotTemp;

        uiManager.SetAltFireOutlineActive(true);
    }

}


public class AltFire : Enumeration {

    public AltFire(int id, string altFireName, int maxAmmo, int cooldownTimer, Sprite sprite, Action<Vector3> altFireAction) : base(id, altFireName) {
        this.maxAmmo = maxAmmo;
        this.cooldownTimer = cooldownTimer;
        this.sprite = sprite;
        this.altFireAction = altFireAction;
    }

    private const string TYPENAME = "Alt-Fire";

    private PlayerController player;
    private UIManager uiManager;

    private Sprite sprite;
    private Action<Vector3> altFireAction;

    private int maxAmmo;
    private int cooldownTimer;


    public void SetupAltFire() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        if (player != null) {
            player.SetAltFire(maxAmmo, cooldownTimer, altFireAction);
            uiManager.SetAltFireSprite(sprite);
        }
    }

    public ShopItem ToShopItem() {
        return new ShopItem(TYPENAME, Name, 100, sprite, SetupAltFire);
    }
}

