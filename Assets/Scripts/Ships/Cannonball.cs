using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour {

    private float speed;
    public float Speed { set { speed = value; } }

    private int damage;
    public int Damage { set { damage = value; } }

    private bool isFriendly;
    public bool IsFriendly { set { isFriendly = value; } }


    void Update() {
        // Move cannonball forward at constant speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    // Destroy cannonball when out of bounds or on collision
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Wall")) {
            Destroy(gameObject);
        }
        else if (isFriendly && other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().DecreaseHealth(damage); // Decrease enemy health
            Destroy(gameObject); // Destroy cannonball
        }
        else if (!isFriendly && other.CompareTag("Player")) {
            if (other.GetComponent<PlayerController>().heavyArmourEnabled) {
                isFriendly = true;
                transform.Rotate(Vector3.up, 180);
            }
            else {
                other.GetComponent<PlayerController>().DecreaseHealth(damage); // Decrease player health
                Destroy(gameObject); // Destroy cannonball
            }
        }
    }

}
