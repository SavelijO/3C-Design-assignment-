using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTplayer : MonoBehaviour
{
    //health
    public int maxHealth = 100;
    public int currentHealth;
        
    public HealthBar healthBar;

               

    private void Start()
    {
        //health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
             
           
    }

    private void Update()
    {
        //testing taking damange by pressing H
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage (int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        
    }

   
    
}
