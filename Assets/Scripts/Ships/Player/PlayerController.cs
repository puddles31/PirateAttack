using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Ship {
    
    private Plane groundPlane;

    private bool onShootCooldown, onAltFireCooldown, onSpecialAbilityCooldown;
    private int altFireCooldown;
    private int altFireAmmo, maxAltFireAmmo, specialAbilityCooldown;

    public bool heavyArmourEnabled, bombShotActive;
    public BombShot bombShot;

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
        uiManager.UpdateHealthText(health, maxHealth);

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

        // Shoot alternate fire on right mouse click
        if (Input.GetMouseButtonDown(1) && !gameManager.IsPaused) {

            if (bombShotActive) {
                Debug.Log("Bomb shot active: exploding");
                bombShot.Explode();
            }
            else if (!onAltFireCooldown && altFireAmmo > 0 && maxAltFireAmmo > 0) {
                Debug.Log("Bomb shot not active: spawning bomb");
                onAltFireCooldown = true;
                DecreaseAltFireAmmo(1);
                AltFireAction(CalculateMousePos());
            }
            else {
                Debug.Log("On cooldown");
            }
            
        }

        // Use special ability on spacebar, starting a cooldown timer - only if player has special ability (when cooldown not 0)
        if (Input.GetKeyDown(KeyCode.Space) && !onSpecialAbilityCooldown && specialAbilityCooldown != 0 && !gameManager.IsPaused) {
            onSpecialAbilityCooldown = true;
            SpecialAbilityAction();
        }
    }


    // Cooldown timer for shooting cannonballs
    private IEnumerator ShootCooldownTimer() {
        yield return new WaitForSeconds(shootCooldown);
        onShootCooldown = false;
    }

    public void StartAltFireCooldown() {
        StartCoroutine(AltFireCooldownTimer());
    }

    // Cooldown timer for alt fire
    private IEnumerator AltFireCooldownTimer() {
        uiManager.SetAltFireCooldownActive(true);

        for (int i = altFireCooldown; i > 0; i--) {
            uiManager.UpdateAltFireCooldownText(i);
            yield return new WaitForSeconds(1);
        }

        uiManager.SetAltFireCooldownActive(false);
        onAltFireCooldown = false;
    }

    // Cooldown timer for special ability
    public IEnumerator SpecialAbilityCooldownTimer() {
        uiManager.SetSpecialAbilityCooldownActive(true);
        
        for (int i = specialAbilityCooldown; i > 0; i--) {
            uiManager.UpdateSpecialAbilityCooldownText(i);
            yield return new WaitForSeconds(1);
        }

        uiManager.SetSpecialAbilityCooldownActive(false);
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


    public void SetAltFire(int maxAltFireAmmo, int altFireCooldown, Action<Vector3> AltFireAction) {
        this.maxAltFireAmmo = maxAltFireAmmo;
        altFireAmmo = maxAltFireAmmo;
        this.altFireCooldown = altFireCooldown;
        this.AltFireAction = AltFireAction;
        
        uiManager.SetAltFireActive(true);
        uiManager.UpdateAltFireAmmoText(maxAltFireAmmo, maxAltFireAmmo);
    }

    public void SetSpecialAbility(int specialAbilityCooldown, Action SpecialAbilityAction) {
        this.specialAbilityCooldown = specialAbilityCooldown;
        this.SpecialAbilityAction = SpecialAbilityAction;

        uiManager.SetSpecialAbilityActive(true);
        uiManager.SetSpecialAbilityCooldownActive(false);
    }


    // Update the health UI text when increasing health
    public override void IncreaseHealth(int healthIncrease) {
        base.IncreaseHealth(healthIncrease);
        uiManager.UpdateHealthText(health, maxHealth);
    }

    // Update the health UI text when decreasing health
    public override void DecreaseHealth(int healthDecrease) {
        base.DecreaseHealth(healthDecrease);
        uiManager.UpdateHealthText(health, maxHealth);
    }


    // Increase the player's alt-fire ammo, without going over the maximum ammo value
    public void IncreaseAltFireAmmo(int ammoIncrease) {
        altFireAmmo = Mathf.Min(maxAltFireAmmo, altFireAmmo + ammoIncrease);
        uiManager.UpdateAltFireAmmoText(altFireAmmo, maxAltFireAmmo);
    }

    // Decrease the player's alt-fire ammo, without going below 0
    public void DecreaseAltFireAmmo(int ammoDecrease) {
        altFireAmmo = Mathf.Max(0, altFireAmmo +- ammoDecrease);
        uiManager.UpdateAltFireAmmoText(altFireAmmo, maxAltFireAmmo);
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


    // Increase the player's shoot cooldown
    public void IncreaseShootCooldown(float shootCooldownIncrease) {
        shootCooldown += shootCooldownIncrease;
    }

    // Decrease the player's shoot cooldown, without going below 0.1
    public void DecreaseShootCooldown(float shootCooldownDecrease) {
        shootCooldown = Mathf.Max(0.1f, shootCooldown - shootCooldownDecrease);
    }


    // Increase the player's cannonball speed
    public void IncreaseCannonballSpeed(float cannonballSpeedIncrease) {
        cannonballSpeed += cannonballSpeedIncrease;
    }

    // Decrease the player's cannonball speed, without going below 10
    public void DecreaseCannonballSpeed(float cannonballSpeedDecrease) {
        cannonballSpeed = Mathf.Max(10, cannonballSpeed - cannonballSpeedDecrease);
    }


    // Increase the player's cannonball damage
    public void IncreaseCannonballDamage(int cannonballDamageIncrease) {
        cannonballDamage += cannonballDamageIncrease;
    }

    // Decrease the player's cannonball damage, without going below 1
    public void DecreaseCannonballDamage(int cannonballDamageDecrease) {
        cannonballDamage = Mathf.Max(1, cannonballDamage - cannonballDamageDecrease);
    }
}
