using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private UIManager uiManager;

    private bool isPaused;
    public bool IsPaused { get { return isPaused; } }

    private int score;
    public int Score { get { return score; } }


    void Start() {
        // Get reference to UI Manager
        uiManager = transform.parent.GetComponentInChildren<UIManager>();
        uiManager.UpdateGoldText(score);

        // Ignore collisions between enemies (layer 3) and walls (layer 6)
        Physics.IgnoreLayerCollision(3, 6);
    }

    // Increase the player's score
    public void IncreaseScore(int scoreIncrease) {
        score += scoreIncrease;
        uiManager.UpdateGoldText(score);
    }

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
