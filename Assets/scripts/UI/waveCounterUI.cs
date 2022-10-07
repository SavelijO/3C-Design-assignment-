using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waveCounterUI : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private GameObject waveCounter;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject enemyCount;

    void Update()
    {
        waveCounter.GetComponent<Text>().text = spawnManager.wave.ToString();
        timer.GetComponent<Text>().text = ((int)spawnManager.waveTimerCount).ToString();
        enemyCount.GetComponent<Text>().text = (spawnManager.enemiesSpawned - spawnManager.enemiesKilled).ToString();
    }
}
