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

    private bool isShopOpen = false;
    public bool IsShopOpen { get { return isShopOpen; } }


    private void Start() {
        // Get references to spawn and game managers
        spawnManager = transform.parent.GetComponentInChildren<SpawnManager>();
        gameManager = transform.parent.GetComponentInChildren<GameManager>();

        // Set the cursor to the crosshair
        SetCrosshairCursor();
    }

    private void Update() {
        // If the player is in the shop and presses "Esc" then exit the shop
        if (isShopOpen && Input.GetKeyDown(KeyCode.Escape)) {
            SetShopScreenActive(false);
        }    
    }


    // Set the cursor sprite to the crosshair
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
        isShopOpen = setActive;

        gameScreen.SetActive(!setActive);
        shopScreen.SetActive(setActive);
        gameManager.SetPaused(setActive);

        // Set the cursor depending on if the shop is open or closed
        if (setActive) {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            // Uncomment to enable randomised shop each time merchant is opened
            // GameObject.FindGameObjectWithTag("Merchant").GetComponent<Merchant>().CreateRandomShop();
            RefreshShopButtons();

        }
        else {
            SetCrosshairCursor();
        }
    }

    public void RefreshShopButtons() {
        foreach (ShopButton shopButton in shopScreen.GetComponentsInChildren<ShopButton>()) {
            shopButton.RefreshButton();
        }
    }


    // Change the cursor to a pointer (call this on mouse over a button)
    public void OnMouseOverButton(Button button) {
        if (button.IsInteractable()) {
            Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // Change the cursor to default (call this on mouse exit of a button)
    public void OnMouseExitButton() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
