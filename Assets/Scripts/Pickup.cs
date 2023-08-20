using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour {

    private float initialTime, initialY, waveHeight = 0.5f, waveSpeed = 2, spawnForce = 8;
    private float lifeDuration = 15, timeSinceSpawn, blinkDuration = 0.4f;
    private float attractForce = 100, attractDistance = 10, distance;

    private Rigidbody pickupRb;
    private MeshRenderer[] meshRenderers;
    protected GameObject player;


    protected virtual void Start() {
        // Capture the initial y position and time
        initialY = transform.position.y;
        initialTime = Time.time;

        // Get reference to rigidbody, renderer and player
        pickupRb = GetComponent<Rigidbody>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Add a relative impulse force to the pickup in a random direction
        Vector3 randomDir = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y).normalized;
        pickupRb.AddRelativeForce(randomDir * spawnForce, ForceMode.Impulse);

        // Start the blinking coroutine
        StartCoroutine(BlinkingControl());
    }

    protected void Update() {
        // Calculate current time since spawn
        timeSinceSpawn = Time.time - initialTime;

        // Calculate the new y position for the pickup using the Sin function
        Vector3 pos = transform.position;
        float newY = initialY + waveHeight * Mathf.Sin(timeSinceSpawn * waveSpeed);
        transform.position = new Vector3(pos.x, newY, pos.z);
    }

    protected void FixedUpdate() {
        if (player != null) {
            // Get the vector from the pickup towards the player
            Vector3 towardsPlayer = player.transform.position - transform.position;
            distance = towardsPlayer.magnitude;

            // If the pickup is close enough to the player, apply an acceleration force to the pickup
            if (distance < attractDistance) {
                pickupRb.AddForce(towardsPlayer.normalized * attractForce / distance, ForceMode.Acceleration);
            }
        }
    }


    // Control the blinking effect when the pickup is about to disappear
    protected IEnumerator BlinkingControl() {
        // Wait for half the pickups life duration
        yield return new WaitForSeconds(0.5f * lifeDuration);

        // Start slow blinking until 80% of life duration
        while (timeSinceSpawn < 0.8f * lifeDuration) {
            foreach (MeshRenderer mr in meshRenderers) {
                mr.enabled = false;
            }
            yield return new WaitForSeconds(blinkDuration);

            foreach (MeshRenderer mr in meshRenderers) {
                mr.enabled = true;
            }
            yield return new WaitForSeconds(blinkDuration);
        }

        // Set blink duration to half of initial value
        blinkDuration *= 0.5f;

        // Start fast blinking until life duration reached
        while (timeSinceSpawn < lifeDuration) {
            foreach (MeshRenderer mr in meshRenderers) {
                mr.enabled = false;
            }
            yield return new WaitForSeconds(blinkDuration);

            foreach (MeshRenderer mr in meshRenderers) {
                mr.enabled = true;
            }
            yield return new WaitForSeconds(blinkDuration);
        }

        // Destroy the pickup
        Destroy(gameObject);
    }


    // If the player collides with the pickup, call OnPickup
    protected void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            OnPickup();
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickup();

}
