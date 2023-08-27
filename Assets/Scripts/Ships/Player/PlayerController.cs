using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Ship {
    
    private Plane groundPlane;
    private bool onShootCooldown, onAltFireCooldown, onSpecialAbilityCooldown;
    private float altFireCooldown, specialAbilityCooldown;
    private int altFireAmmo, maxAltFireAmmo;
    private Action<Vector3> AltFireAction;
    private Action SpecialAbilityAction;
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

        AltFireAction = (pos) => { };
        SpecialAbilityAction = () => { };
    }


    void Update() {
        // Get vertical / horizontal inputs - use Math.Max to keep vertical positive (no backwards movement)
        horizontalForce = Input.GetAxis("Horizontal");
        verticalForce = Mathf.Max(0f, Input.GetAxis("Vertical"));

        // Shoot cannonball on left mouse click, starting a cooldown timer
        if (Input.GetMouseButtonDown(0) && !onShootCooldown && !gameManager.IsPaused) {
            onShootCooldown = true;
            StartCoroutine(ShootCooldownTimer());
            ShootCannonball(CalculateMousePos());
        }

        // Shoot alternate fire on right mouse click, starting a cooldown timer and decreasing ammo
        if (Input.GetMouseButtonDown(1) && !onAltFireCooldown && altFireAmmo > 0 && !gameManager.IsPaused) {
            onAltFireCooldown = true;
            altFireAmmo--;
            StartCoroutine(AltFireCooldownTimer());
            AltFireAction(CalculateMousePos());
        }

        // Use special ability on spacebar, starting a cooldown timer
        if (Input.GetKeyDown(KeyCode.Space) && !onSpecialAbilityCooldown && !gameManager.IsPaused) {
            onSpecialAbilityCooldown = true;
            StartCoroutine(SpecialAbilityCooldownTimer());
            SpecialAbilityAction();
        }
    }


    // Cooldown timer for shooting cannonballs
    IEnumerator ShootCooldownTimer() {
        yield return new WaitForSeconds(shootCooldown);
        onShootCooldown = false;
    }

    // Cooldown timer for alt fire
    IEnumerator AltFireCooldownTimer() {
        yield return new WaitForSeconds(altFireCooldown);
        onAltFireCooldown = false;
    }

    // Cooldown timer for special ability
    IEnumerator SpecialAbilityCooldownTimer() {
        yield return new WaitForSeconds(specialAbilityCooldown);
        onSpecialAbilityCooldown = false;
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


    public void SetAltFire(int maxAltFireAmmo, float altFireCooldown, Action<Vector3> AltFireAction) {
        this.maxAltFireAmmo = maxAltFireAmmo;
        this.altFireCooldown = altFireCooldown;
        this.AltFireAction = AltFireAction;

        altFireAmmo = maxAltFireAmmo;
    }

    public void SetSpecialAbility(float specialAbilityCooldown, Action SpecialAbilityAction) {
        this.specialAbilityCooldown = specialAbilityCooldown;
        this.SpecialAbilityAction = SpecialAbilityAction;
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

    // Increase the player's maximum health, and healing for the same amount
    public void IncreaseMaxHealth(int maxHealthIncrease) {
        maxHealth += maxHealthIncrease;
        IncreaseHealth(maxHealthIncrease);
    }

    // Increase the player's acceleration
    public void IncreaseAcceleration(float accelerationIncrease) {
        acceleration += accelerationIncrease;
    }

    // Increase the player's angular acceleration
    public void IncreaseAngularAcceleration(float angularAccelerationIncrease) {
        angularAcceleration += angularAccelerationIncrease;
    }

    // Decrease the player's shoot cooldown, without going below 0.1
    public void DecreaseShootCooldown(float shootCooldownDecrease) {
        shootCooldown = Mathf.Max(0.1f, shootCooldown - shootCooldownDecrease);
    }

    // Increase the player's cannonball speed
    public void IncreaseCannonballSpeed(float cannonballSpeedIncrease) {
        cannonballSpeed += cannonballSpeedIncrease;
    }

    // Increase the player's cannonball damage
    public void IncreaseCannonballDamage(int cannonballDamageIncrease) {
        cannonballDamage += cannonballDamageIncrease;
    }
}
