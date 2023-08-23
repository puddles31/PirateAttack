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

    [SerializeField]
    private string itemName, itemType;

    [SerializeField]
    private int itemPrice;

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
        nameText.text = itemName;
        typeText.text = itemType;
        priceText.text = itemPrice + " Gold";

        if (isSoldOut) {
            button.interactable = false;
            soldOutText.SetActive(true);

            // Update the disabled color for the button
            var colorBlock = button.colors;
            colorBlock.disabledColor = soldOutColor;
            button.colors = colorBlock;
            priceText.color = defaultColor;
        }
        else if (gameManager.Gold < itemPrice) {
            button.interactable = false;
            priceText.color = tooExpensiveColor;
        }
        else {
            button.interactable = true;
            priceText.color = defaultColor;
        }
    }


    public void PurchaseItem() {
        // Check if the 
        if (gameManager.Gold >= itemPrice) {
            gameManager.DecreaseGold(itemPrice);
            isSoldOut = true;
            // APPLY THE PURCHASE HERE
            uiManager.RefreshShopButtons();
            uiManager.OnMouseExitButton();
        }
    }
}
