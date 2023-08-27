using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Merchant : MonoBehaviour
{

    private UIManager uiManager;
    private GameObject player;
    private PlayerStats playerStats;
    private Consumables consumables;
    private AltFires altFires;
    private SpecialAbilities specialAbilities;

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
        playerStats = player.GetComponent<PlayerStats>();
        consumables = gameObject.GetComponent<Consumables>();
        altFires = gameObject.GetComponent<AltFires>();
        specialAbilities = gameObject.GetComponent<SpecialAbilities>();
        shopButtons = shopScreen.GetComponentsInChildren<ShopButton>();

        CreateRandomShop();
    }

    [ContextMenu("Generate Random Shop")]
    public void CreateRandomShop() {
        shopButtons[0].shopItem = consumables.RandomSelection(1).First().ToShopItem();
        shopButtons[1].shopItem = altFires.RandomSelection(1).First().ToShopItem();
        shopButtons[2].shopItem = specialAbilities.RandomSelection(1).First().ToShopItem();

        foreach (var tuple in shopButtons.Skip(3).Zip(playerStats.RandomSelection(3), (b, s) => new { Button = b, Stat = s })) {
            tuple.Button.shopItem = tuple.Stat.ToShopItem();
        }

        foreach (var shopButton in shopButtons) {
            shopButton.isSoldOut = false;
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
