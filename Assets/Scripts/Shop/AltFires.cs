using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AltFires : MonoBehaviour {

    private List<AltFire> altFireList;

    private AltFire homingMissiles;

    private System.Random rnd;


    private void Start() {
        homingMissiles = new(0, "Homing Missiles", 5, 2, (pos) => { Debug.Log("BOOM"); });

        altFireList = new List<AltFire>() { homingMissiles };

        rnd = new System.Random();
    }

    public List<AltFire> RandomSelection(int n) {
        return altFireList.OrderBy(x => rnd.Next()).Take(n).ToList();
    }

}


public class AltFire : Enumeration {

    public AltFire(int id, string altFireName, int maxAmmo, float cooldownTimer, Action<Vector3> altFireAction) : base(id, altFireName) {
        this.maxAmmo = maxAmmo;
        this.cooldownTimer = cooldownTimer;
        this.altFireAction = altFireAction;
    }

    private const string TYPENAME = "Alt-Fire";

    private PlayerController player;
    private Action<Vector3> altFireAction;

    private int maxAmmo;
    private float cooldownTimer;


    public void SetupAltFire() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (player != null) {
            player.SetAltFire(maxAmmo, cooldownTimer, altFireAction);
        }
    }

    public ShopItem ToShopItem() {
        return new ShopItem(TYPENAME, Name, 100, SetupAltFire);
    }
}

