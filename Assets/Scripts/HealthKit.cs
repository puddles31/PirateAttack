using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : Pickup {

    [SerializeField]
    private int healthValue;

    private PlayerController playerController;

    protected override void Start() {
        base.Start();
        playerController = player.GetComponent<PlayerController>();
    }

    // Increase the player's health on pickup
    protected override void OnPickup() {
        playerController.IncreaseHealth(healthValue);
    }
}
