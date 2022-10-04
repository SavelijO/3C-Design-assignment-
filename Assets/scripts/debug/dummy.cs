using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummy : MonoBehaviour
{
    public int health = 100;

    public void ReduceHealth(int bulletDamage)
    {
        health -= bulletDamage;
        if(health < 0) { health = 100; Debug.Log("dead"); } 
        Debug.Log(health);
    }


}
