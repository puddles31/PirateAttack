using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private SpawnManager spawnManager;
    private GameManager gameManager;

    [SerializeField]
    private Texture2D crosshairCursor, pointerCursor;

    [SerializeField]
    private GameObject gameScreen;
    [SerializeField]
    private TextMeshProUGUI healthText, goldText, waveText, enemiesText;

    [SerializeField]
    private GameObject shopScreen;
    [SerializeField]
    private TextMeshProUGUI shopGoldText;


    private void Start() {
        spawnManager = transform.parent.GetComponentInChildren<SpawnManager>();
        gameManager = transform.parent.GetComponentInChildren<GameManager>();

        SetCrosshairCursor();
    }

    // Set the crosshair cursor sprite
    private void SetCrosshairCursor() {
        Vector2 midpoint = new Vector2(crosshairCursor.width / 2, crosshairCursor.height / 2);
        Cursor.SetCursor(crosshairCursor, midpoint, CursorMode.Auto);
    }

    // Update the health text in the UI
    public void UpdateHealthText(int health) {
        healthText.text = "Health: " + health;
    }

    // Update the gold text in the UI
    public void UpdateGoldText(int gold) {
        goldText.text = "Gold: " + gold;
        shopGoldText.text = "Gold: " + gold;
    }

    // Update the wave text in the UI
    public void UpdateWaveText(int wave) {
        waveText.text = "Wave " + wave;
    }

    // Update the enemies text in the UI
    public void UpdateEnemiesText(int enemies) {
        if (enemies > 0) {
            enemiesText.text = enemies + " Enemies Left";
        }
        else {
            enemiesText.text = "Wave Complete. Next wave starting in " + spawnManager.GetWaveDelay() + " seconds.";
        }
    }


    // Set the shop screen active
    public void SetShopScreenActive(bool setActive) {
        gameScreen.SetActive(!setActive);
        shopScreen.SetActive(setActive);
        gameManager.SetPaused(setActive);

        if (setActive) {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        else {
            SetCrosshairCursor();
        }
    }

    public void OnMouseOverButton() {
        Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExitButton() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
