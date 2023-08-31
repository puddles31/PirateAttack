using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour {

    private UIManager uiManager;

    private bool isPaused;
    public bool IsPaused { get { return isPaused; } }

    private int gold = 5000;
    public int Gold { get { return gold; } }


    void Start() {
        // Get reference to UI Manager
        uiManager = transform.parent.GetComponentInChildren<UIManager>();
        uiManager.UpdateGoldText(gold);

        // Ignore collisions between enemies (layer 3) and walls (layer 6)
        Physics.IgnoreLayerCollision(3, 6);
    }

    // Increase the player's gold
    public void IncreaseGold(int goldIncrease) {
        gold += goldIncrease;
        uiManager.UpdateGoldText(gold);
    }

    // Decrease the player's gold
    public void DecreaseGold(int goldDecrease) {
        gold -= goldDecrease;
        if (gold < 0) {
            gold = 0;
        }

        uiManager.UpdateGoldText(gold);
    }

    // Pause/unpause the game by adjusting the time scale
    public void SetPaused(bool setPaused) {
        isPaused = setPaused;

        if (setPaused) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }

}
