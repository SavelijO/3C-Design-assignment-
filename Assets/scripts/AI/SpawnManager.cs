using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int rangeOfSpawn = 50;
    [SerializeField] private float cooldownSpawn = 3;
    [SerializeField] private int enemiesToSpawn = 30;
    
    private int enemiesSpawned = 0;
    private Vector3 spawnPosition;
    private bool spawned;
    private bool isOnMap;
    private bool isNotColliding;
    private LayerMask whatIsGround;
    
    // Start is called before the first frame update
    void Start()
    {

        whatIsGround = LayerMask.GetMask("Walkable");

    }

    // Update is called once per frame
    void Update()
    {
        spawnPosition = CalculateRandomPosition();
        CheckIfOnMap();

        if (enemiesSpawned < enemiesToSpawn && !spawned && isOnMap)
        {
            SpawnEnemy(spawnPosition);
            enemiesSpawned++;
        }
    }

    private Vector3 CalculateRandomPosition()
    {
        return this.transform.position + new Vector3(Random.Range(-rangeOfSpawn, rangeOfSpawn), 0,
            Random.Range(-rangeOfSpawn, rangeOfSpawn));
    }
    
    private void SpawnEnemy(Vector3 whereToSpawn)
    {
        Instantiate(enemies[0], whereToSpawn, this.transform.rotation);
    }

    private void CheckIfOnMap()
    {
        isOnMap = Physics.CheckSphere(spawnPosition, 5f, whatIsGround);
    }

    private IEnumerator SpawnCooldown()
    {
        spawned = true;
        yield return new WaitForSeconds(cooldownSpawn);
        spawned = false;
    }
}