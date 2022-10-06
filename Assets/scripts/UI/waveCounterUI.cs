using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waveCounterUI : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private GameObject waveCounter;

    void Update()
    {
        waveCounter.GetComponent<Text>().text = spawnManager.wave.ToString();
    }
}
