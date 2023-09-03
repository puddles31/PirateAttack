using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    [SerializeField]
    private GameObject treasurePrefab, healthkitPrefab;
    private GameObject player;

    [SerializeField]
    private float healthkitSpawnChance;

    private bool bulletTimeActive;
    private float bulletTimeStrength = 1;


    protected override void Start() {
        base.Start();
        isPlayer = false;

        // Get reference to Player
        player = GameObject.FindGameObjectWithTag("Player");

        // Set vertical force to 1 (initially) - always moving forwards
        verticalForce = bulletTimeStrength;

        // Start shooting cannonballs at player
        StartCoroutine(ShootAtPlayer());
    }

    private void Update() {
        // If the player exists, try to turn towards them
        if (player != null) {
            // Calculate the angle between this ship's forward direction and the player
            Vector3 towardsPlayer = (player.transform.position - transform.position).normalized;
            float angleToPlayer = Vector3.SignedAngle(transform.forward, towardsPlayer, Vector3.up);

            // Set the horizontal force based on the angle to the player
            horizontalForce = angleToPlayer * bulletTimeStrength / 180;
        }
        else {
            horizontalForce = 0;
        }
    }


    // While the player is alive, shoot at them on a cooldown timer
    IEnumerator ShootAtPlayer() {
        while (player != null) {
            yield return new WaitForSeconds(shootCooldown);

            if (player != null) {
                ShootCannonball(player.transform.position);
            }
        }
    }

    protected override Cannonball ShootCannonball(Vector3 targetPosition) {
        var cannonball = base.ShootCannonball(targetPosition);

        if (bulletTimeActive) {
            cannonball.TimeModifier = bulletTimeStrength;
        }

        return cannonball;
    }

    public void SetBulletTimeActive(bool isActive, float bulletTimeStrength) {
        bulletTimeActive = isActive;
        this.bulletTimeStrength = bulletTimeStrength;

        if (isActive) {
            verticalForce = bulletTimeStrength;
            shootCooldown /= bulletTimeStrength;

            if (shipRb != null && shipRb.velocity != null && shipRb.angularVelocity != null) {
                shipRb.velocity *= bulletTimeStrength;
                shipRb.angularVelocity *= bulletTimeStrength;
            }
            
        }
        else {
            verticalForce = 1;
            shootCooldown *= bulletTimeStrength;
            shipRb.velocity /= bulletTimeStrength;
            shipRb.angularVelocity /= bulletTimeStrength;
        }
        
    }

    // When the ship is destroyed, spawn a treasure pickup and a chance for a healthkit pickup
    protected override void DestroyShip() {
        base.DestroyShip();

        Instantiate(treasurePrefab, transform.position, transform.rotation);

        if (Random.value < healthkitSpawnChance) {
            Instantiate(healthkitPrefab, transform.position, transform.rotation);
        }
    }
}
