using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] enemies;
    //[SerializeField] private int rangeOfSpawn = 50;
    [SerializeField] private float cooldownSpawn = 1;
    [SerializeField] public int wave = 1;
    [SerializeField] private float waveEnemiesMultiplier = 2;
    [SerializeField] private float waveTimer = 20;
    [SerializeField] private float waveTimerMultiplier = 1;
    [SerializeField] private GameObject[] spawnPoints;

    private int enemiesToSpawnForWave = 2;
    private float waveTimerCount;
    private int enemiesSpawned = 0;
    private Vector3 spawnPosition;
    private bool spawned;
    private bool isColliding;
    private bool isOnObstacle;
    private LayerMask whatIsGround;
    private LayerMask whatIsObstacle;
    private bool waveIsStarted;
    private int enemiesKilled = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        whatIsGround = LayerMask.GetMask("Walkable");
        waveTimerCount = waveTimer;
        whatIsGround = LayerMask.GetMask("Obstacle");

    }

    // Update is called once per frame
    void Update()
    {
        
        if (waveTimerCount > 0 && enemiesKilled < enemiesToSpawnForWave)
        {
            waveTimerCount -= Time.deltaTime;
            ContinueWave();
        }
        else if(waveTimerCount <= 0 || enemiesKilled == enemiesToSpawnForWave)
        {
            RestartWave();
            wave++;
        }
        
    }

    private void ContinueWave()
    {
        if (enemiesSpawned == 0)
        {
            CalculateEnemiesToSpawn();
        }

        CheckIfOnObstacle();
        
        if (enemiesSpawned < enemiesToSpawnForWave && !spawned && !isColliding && !isOnObstacle )
        {
            spawnPosition = CalculateRandomPosition();
            SpawnEnemy(spawnPosition);
            enemiesSpawned++;
            StartCoroutine(SpawnCooldown());
        }
    }

    private void RestartWave()
    {
        waveTimerCount = waveTimer + CalculateWaveTimer();
        enemiesSpawned = 0;
        enemiesKilled = 0;
    }

    private Vector3 CalculateRandomPosition()
    {
        int randomSpawnPoint = Random.Range(0, 4);
        return spawnPoints[randomSpawnPoint].transform.position;
    }
    
    private void SpawnEnemy(Vector3 whereToSpawn)
    {
        Instantiate(enemies[0], whereToSpawn, this.transform.rotation);
    }

    private void CheckIfOnObstacle()
    {
        isOnObstacle = Physics.CheckSphere(spawnPosition, 2f, whatIsObstacle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isColliding = (collision.gameObject.CompareTag("Player") && collision.gameObject.layer != whatIsGround);
    }

    private void CalculateEnemiesToSpawn()
    {
        enemiesToSpawnForWave = (int)Math.Pow(waveEnemiesMultiplier, wave);
    }

    private float CalculateWaveTimer()
    {
        return (float)Math.Pow(waveTimerMultiplier, wave);
    }

    private IEnumerator SpawnCooldown()
    {
        spawned = true;
        yield return new WaitForSeconds(cooldownSpawn);
        spawned = false;
    }

    public void EnemyDied()
    {
        enemiesKilled++;
    }
}
