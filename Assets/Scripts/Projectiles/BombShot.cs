using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShot : MonoBehaviour {

    private float speed;
    public float Speed { set { speed = value; } }

    private int impactDamage;
    public int ImpactDamage { set { impactDamage = value; } }

    private int blastDamage;
    public int BlastDamage { set { blastDamage = value; } }

    private float knockbackForce;
    public float KnockbackForce { set { knockbackForce = value; } }

    private float blastRadius;
    public float BlastRadius { set { blastRadius = value; } }


    private PlayerController player;
    private UIManager uiManager;


    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        player.bombShotActive = true;
    }

    void Update() {
        // Move bomb shot forward at constant speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    // Explode bomb shot on impact with enemy or wall
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Wall")) {
            Explode();  // Explode bomb shot
        }
        else if (other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().DecreaseHealth(impactDamage); // Apply impact damage
            Explode(); // Explode bomb shot
        }
    }


    // Explode the bomb shot, dealing damage and applying knockback to nearby enemies
    public void Explode() {
        player.bombShotActive = false;
        player.StartAltFireCooldown();

        uiManager.SetAltFireOutlineActive(false);

        Destroy(gameObject);
        Debug.Log("BOOM");
    }

}
