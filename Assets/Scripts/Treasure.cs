using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Pickup {

    [SerializeField]
    private int treasureScore;

    private GameManager gameManager;

    protected override void Start() {
        base.Start();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Increase the player's score on pickup
    protected override void OnPickup() {
        gameManager.IncreaseScore(treasureScore);
    }
}
