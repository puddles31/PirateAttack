using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Merchant : MonoBehaviour
{

    private UIManager uiManager;
    private GameObject player;
    private float interactDistance = 16;


    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            // Get the distance between the merchant and the player
            float distance = (player.transform.position - transform.position).magnitude;

            // If the player is close enough to the merchant, open shop menu when key is pressed
            if (distance < interactDistance && Input.GetKeyDown(KeyCode.E)) {
                uiManager.SetShopScreenActive(true);
            }

        }
    }
}
