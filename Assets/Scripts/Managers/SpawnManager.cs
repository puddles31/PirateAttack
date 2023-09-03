using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private UIManager uiManager;

    [SerializeField]
    private GameObject[] innerWalls, outerWalls;

    [SerializeField]
    private GameObject enemyPrefab;

    private int waveNumber, enemiesToSpawn, enemiesAlive, waveDelay = 10;
    private float spawnOffsetX = 0.1f, spawnOffsetZ = -80;
    private float minSpawnDelay = 4, maxSpawnDelay = 6, absoluteMinSpawnDelay = 1, spawnDelayChange = 0.25f;
    private int minSpawnNo = 1, maxSpawnNo = 1, wavesToIncreaseSpawnNo = 3, enemiesToSpawnIncrease = 5;
    private bool betweenWaves;

    public bool bulletTimeActive;
    public float bulletTimeStrength;
    

    private void Start()
    {
        // Get reference to UI Manager
        uiManager = transform.parent.GetComponentInChildren<UIManager>();

        // Increase wave stats, then start the first wave
        IncreaseWaveStats();
        StartCoroutine(StartWave());
    }

    private void Update() {
        // Count the number of enemies currently alive
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Enemies left in the wave = Enemies still to spawn + Enemies currently alive
        // Update the enemies text in the UI
        uiManager.UpdateEnemiesText(enemiesToSpawn + enemiesAlive);

        // If the wave is complete, run the End Wave coroutine
        if (!betweenWaves && enemiesAlive == 0 && enemiesToSpawn == 0) {
            StartCoroutine(EndWave());
            betweenWaves = true;
        }
    }

    // Return the delay between waves (used for UI)
    public int GetWaveDelay() {
        return waveDelay;
    }


    // Start the next wave
    private IEnumerator StartWave() {
        //Debug.Log("Wave " + waveNumber + " starting (contains " + enemiesToSpawn + " enemies)");

        // Update text in UI
        uiManager.UpdateWaveText(waveNumber);
        uiManager.UpdateEnemiesText(enemiesToSpawn);

        // Keep spawning enemies until required number have been spawned
        while (enemiesToSpawn > 0) {
            // Get a random spawn delay between the min and max
            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);

            // Choose a random number of enemies to spawn while not spawning more than enemiesToSpawn
            int noToSpawn = Mathf.Min(enemiesToSpawn, Random.Range(minSpawnNo, maxSpawnNo + 1));

            // Spawn the number of enemies
            for (int i = 0; i < noToSpawn; i++) {
                SpawnEnemy();
                enemiesToSpawn--;
            }
            //Debug.Log("Spawned " + noToSpawn + " enemies");
        }

        //Debug.Log("Done spawning enemies");
    }

    // End the current wave
    private IEnumerator EndWave() {
        //Debug.Log("Wave " + waveNumber + " complete. Next wave starting in " + waveDelay + " seconds...");

        // Wait before starting the next wave
        yield return new WaitForSeconds(waveDelay);

        // Increase the stats for the next wave
        IncreaseWaveStats();
        betweenWaves = false;
        
        // Start the next wave
        StartCoroutine(StartWave());
    }

    // Increase the stats for the next wave
    private void IncreaseWaveStats() {
        waveNumber++;

        // Enemies to spawn increases each wave
        enemiesToSpawn = waveNumber * enemiesToSpawnIncrease;

        // Increase the maximum spawn number after a number of waves
        if (waveNumber % wavesToIncreaseSpawnNo == 0) {
            maxSpawnNo++;
        }

        // Decrease the min and max spawn delays
        minSpawnDelay = Mathf.Max(absoluteMinSpawnDelay, minSpawnDelay - spawnDelayChange);
        maxSpawnDelay = Mathf.Max(absoluteMinSpawnDelay, maxSpawnDelay - spawnDelayChange);
    }



    // Spawn an enemy at a random position behind the inner wall boundaries
    private void SpawnEnemy() {
        // Pick a random side to spawn from
        int randomSide = Random.Range(0, innerWalls.Length);

        // Get the tranform and the bounds (size) of the walls
        Transform wallTransform = innerWalls[randomSide].transform;
        Bounds wallBounds = innerWalls[randomSide].GetComponent<MeshFilter>().mesh.bounds;

        // Generate a random X position based on the size of the wall bounds
        float randomX = Random.Range(wallBounds.min.x - spawnOffsetX, wallBounds.max.x + spawnOffsetX);

        // Create a spawn position with the random X position, and an offset for the Z position
        Vector3 spawnPos = wallTransform.TransformPoint(new Vector3(randomX, 0, spawnOffsetZ));

        // Spawn the enemy
        var enemy = Instantiate(enemyPrefab, spawnPos, wallTransform.rotation);

        if (bulletTimeActive) {
            enemy.GetComponent<Enemy>().SetBulletTimeActive(true, bulletTimeStrength);
        }
    }


}
