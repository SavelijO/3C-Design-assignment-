using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{

    [SerializeField] private RawImage[] bullets;
    [SerializeField] public float bulletCount;

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            bullets[i].enabled = false;
        }
        for (int i = 0; i < bulletCount; i++)
        {
            bullets[i].enabled = true;
        }
    }
}
