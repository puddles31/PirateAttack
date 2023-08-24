using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Merchant : MonoBehaviour
{

    private UIManager uiManager;
    private GameObject player;
    private float interactDistance = 16;

    [SerializeField]
    private GameObject interactText;

    [SerializeField]
    private GameObject shopScreen;
    private ShopButton[] shopButtons;


    void Start()
    {
        // Get references to UI manager and player
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        shopButtons = shopScreen.GetComponentsInChildren<ShopButton>();

        CreateRandomShop();
    }


    private void CreateRandomShop() {
        foreach (var shopButton in shopButtons) {
            shopButton.shopItem = new Consumable("Ammo Box", 25);
        }
    }

    
    void Update()
    {
        // Check the player exists
        if (player != null) {
            // Get the distance between the merchant and the player
            float distance = (player.transform.position - transform.position).magnitude;

            // If the player is close enough to the merchant and the shop isn't open
            if (distance < interactDistance && !uiManager.IsShopOpen) {

                // Show a UI text element above the merchant
                interactText.SetActive(true);

                // If the player presses "E", open the shop
                if (Input.GetKeyDown(KeyCode.E)) {
                    uiManager.SetShopScreenActive(true);
                }
            }
            // Otherwise, hide the UI text element above the merchant
            else {
                interactText.SetActive(false);
            }
        }
    }
}
