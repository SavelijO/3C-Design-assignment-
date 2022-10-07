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
    public float waveTimerCount;
    public int enemiesSpawned = 0;
    private Vector3 spawnPosition;
    private bool spawned;
    private bool isColliding;
    private bool isOnObstacle;
    private LayerMask whatIsGround;
    private LayerMask whatIsObstacle;
    private bool waveIsStarted;
    public int enemiesKilled = 0;
    private int spawnPointsCount;
    
    // Start is called before the first frame update
    void Start()
    {
        whatIsGround = LayerMask.GetMask("Walkable");
        waveTimerCount = waveTimer;
        whatIsGround = LayerMask.GetMask("Obstacle");
        spawnPointsCount = spawnPoints.Length;
        
        //InvokeRepeating("RandomSpawns", 180, 4);

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
        waveTimerCount = Mathf.Clamp(waveTimer + CalculateWaveTimer(), 0, 120);
        enemiesSpawned = 0;
        enemiesKilled = 0;
    }

    private Vector3 CalculateRandomPosition()
    {
        int randomSpawnPoint = Random.Range(0, spawnPointsCount);
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

    void RandomSpawns()
    {
        SpawnEnemy(spawnPoints[Random.Range(0, spawnPointsCount)].transform.position);
    }
}
