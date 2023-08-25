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


    protected override void Start() {
        base.Start();
        isPlayer = false;

        // Get reference to Player
        player = GameObject.FindGameObjectWithTag("Player");

        // Set vertical force to 1 - always moving forwards
        verticalForce = 1;

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
            horizontalForce = angleToPlayer / 180;
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


    // When the ship is destroyed, spawn a treasure pickup and a chance for a healthkit pickup
    protected override void DestroyShip() {
        base.DestroyShip();

        Instantiate(treasurePrefab, transform.position, transform.rotation);

        if (Random.value < healthkitSpawnChance) {
            Instantiate(healthkitPrefab, transform.position, transform.rotation);
        }
    }
}
