using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Pickup {

    [SerializeField]
    private int goldAmount;

    private GameManager gameManager;

    protected override void Start() {
        base.Start();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Increase the player's gold on pickup
    protected override void OnPickup() {
        gameManager.IncreaseGold(goldAmount);
    }
}
