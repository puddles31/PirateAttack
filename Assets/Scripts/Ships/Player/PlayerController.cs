using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Ship {
    
    private Plane groundPlane;
    private bool onShootCooldown;
    private UIManager uiManager;
    private GameManager gameManager;

    protected override void Start() {
        base.Start();
        isPlayer = true;

        // Create a new invisible plane with the normal equal to Up vector, and distance from origin of 0
        // This is equivalent to the ground plane gameobject in the world
        groundPlane = new Plane(Vector3.up, 0);

        // Get reference to UI Manager and set initial health text
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        uiManager.UpdateHealthText(health);

        // Get reference to Game Manager
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    void Update() {
        // Get vertical / horizontal inputs - use Math.Max to keep vertical positive (no backwards movement)
        horizontalForce = Input.GetAxis("Horizontal");
        verticalForce = Mathf.Max(0f, Input.GetAxis("Vertical"));

        // Shoot cannonball on mouse click, starting a cooldown timer
        if (Input.GetMouseButtonDown(0) && !onShootCooldown && !gameManager.IsPaused) {
            onShootCooldown = true;
            StartCoroutine(ShootCooldownTimer());
            ShootCannonball(CalculateMousePos());
        }
    }

    // Cooldown timer for shooting cannonballs
    IEnumerator ShootCooldownTimer() {
        yield return new WaitForSeconds(shootCooldown);
        onShootCooldown = false;
    }


    // Calculate the mouse's position in world space
    private Vector3 CalculateMousePos() {
        float rayDistance;
        Vector3 mouseWorldPos = Vector3.up;

        // Create a ray going through the screen to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // If the ray intersects with the ground plane (it always will), get the world position of the intersection
        if (groundPlane.Raycast(ray, out rayDistance)) {
            mouseWorldPos = ray.GetPoint(rayDistance);
        }

        return mouseWorldPos;
    }

    // Update the health UI text when increasing health
    public override void IncreaseHealth(int healthIncrease) {
        base.IncreaseHealth(healthIncrease);
        uiManager.UpdateHealthText(health);
    }

    // Update the health UI text when decreasing health
    public override void DecreaseHealth(int healthDecrease) {
        base.DecreaseHealth(healthDecrease);
        uiManager.UpdateHealthText(health);
    }

}
