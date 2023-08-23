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
    private string itemName, itemType;

    [SerializeField]
    private int itemPrice;

    private GameManager gameManager;
    private UIManager uiManager;

    void Awake() {
        button = gameObject.GetComponent<Button>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
    }

    public void RefreshButton() {
        nameText.text = itemName;
        typeText.text = itemType;
        priceText.text = itemPrice + " Gold";

        if (gameManager.Gold < itemPrice) {
            button.interactable = false;
        }
        else {
            button.interactable = true;
        }
    }


    public void PurchaseItem() {
        // Check if the 
        if (gameManager.Gold >= itemPrice) {
            gameManager.DecreaseGold(itemPrice);
            uiManager.RefreshShopButtons();
        }
    }
}
