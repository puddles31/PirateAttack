using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour
{
    private Button button;

    [SerializeField]
    private TextMeshProUGUI nameText, typeText, priceText;

    [SerializeField]
    private GameObject soldOutText;

    private bool isSoldOut;
    private Color soldOutColor, tooExpensiveColor, defaultColor;

    public ShopItem shopItem;

    private GameManager gameManager;
    private UIManager uiManager;


    void Awake() {
        button = gameObject.GetComponent<Button>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        soldOutColor = new Color32(100, 100, 100, 255);
        tooExpensiveColor = new Color32(225, 0, 0, 255);
        defaultColor = new Color32(50, 50, 50, 255);
    }


    // Refresh the button's information
    public void RefreshButton() {
        // Update the text fields
        nameText.text = shopItem.Name;
        typeText.text = shopItem.Type;
        priceText.text = shopItem.Price + " Gold";

        if (isSoldOut) {
            button.interactable = false;
            soldOutText.SetActive(true);

            // Update the disabled color for the button
            var colorBlock = button.colors;
            colorBlock.disabledColor = soldOutColor;
            button.colors = colorBlock;
            priceText.color = defaultColor;
        }
        else if (gameManager.Gold < shopItem.Price) {
            button.interactable = false;
            priceText.color = tooExpensiveColor;
        }
        else {
            button.interactable = true;
            priceText.color = defaultColor;
        }
    }


    public void PurchaseItem() {
        // Check if the player has enough gold for the item
        if (gameManager.Gold >= shopItem.Price) {
            gameManager.DecreaseGold(shopItem.Price);
            isSoldOut = true;

            shopItem.OnPurchase();

            uiManager.RefreshShopButtons();
            uiManager.OnMouseExitButton();
        }
    }
}
