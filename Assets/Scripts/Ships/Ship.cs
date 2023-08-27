using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour {

    private Rigidbody shipRb;

    protected bool isPlayer;

    [SerializeField]
    private GameObject cannonballPrefab;

    [SerializeField]
    protected float acceleration;
    [SerializeField]
    private float maxVelocity, drag;

    [SerializeField]
    protected float angularAcceleration;
    [SerializeField]
    private float maxAngularVelocity, angularDrag;

    protected float verticalForce, horizontalForce;

    [SerializeField]
    protected float shootCooldown = 0.5f;
    [SerializeField]
    protected float cannonballSpeed = 25;
    [SerializeField]
    protected int cannonballDamage = 1;

    [SerializeField]
    protected int maxHealth = 3;
    protected int health;


    protected virtual void Start() {
        // Get Rigidbody component
        shipRb = GetComponent<Rigidbody>();

        // Set the ship's linear and angular drag and max velocity
        shipRb.maxLinearVelocity = maxVelocity;
        shipRb.drag = drag;

        shipRb.maxAngularVelocity = maxAngularVelocity;
        shipRb.angularDrag = angularDrag;

        // Set initial health
        health = maxHealth;
    }


    // Use FixedUpdate for handling Physics on ship
    protected void FixedUpdate() {
        // Add force and torque to the ship's rigidbody, relative to the ship, using acceleration mode
        shipRb.AddRelativeForce(Vector3.forward * verticalForce * acceleration, ForceMode.Acceleration);
        shipRb.AddRelativeTorque(Vector3.up * horizontalForce * angularAcceleration, ForceMode.Acceleration);
    }


    // Shoot a cannonball towards the target position
    protected void ShootCannonball(Vector3 targetPosition) {
        // Get the vector from the ship to the target
        Vector3 towardsTarget = (targetPosition - transform.position).normalized;
        towardsTarget.y = 0;

        // Create the cannonball and set its speed and faction
        var cannonballTemp = Instantiate(cannonballPrefab, transform.position, Quaternion.LookRotation(towardsTarget));
        cannonballTemp.GetComponent<Cannonball>().IsFriendly = isPlayer;
        cannonballTemp.GetComponent<Cannonball>().Speed = cannonballSpeed;
        cannonballTemp.GetComponent<Cannonball>().Damage = cannonballDamage;
    }


    // Increase the ship's health, without going over the maximum value
    public virtual void IncreaseHealth(int healthIncrease) {
        health = Mathf.Min(maxHealth, health + healthIncrease);
    }

    // Decrease the ship's health - if health is less than or equal to zero, destroy the ship
    public virtual void DecreaseHealth(int healthDecrease) {
        health -= healthDecrease;

        if (health <= 0) {
            DestroyShip();
        }
    }

    // Destroy the ship
    protected virtual void DestroyShip() {
        Destroy(gameObject);
    }
}
