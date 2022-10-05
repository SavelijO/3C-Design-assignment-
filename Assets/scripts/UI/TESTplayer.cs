using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTplayer : MonoBehaviour
{
    //health
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    /*/ammo
    private UImanager uiManager;
    */

    private void Start()
    {
        //health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        /*/ammo
        uiManager = GameObject.Find("Canvas").GetComponent<UImanager>();
        */
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

    /*/ammo: in function for shooting
    //example:
    void Shoot()
    {
        uiManager.UpdateAmmo(currentAmmo);
    }*/

    /*/ammo, reload
    IEnumerator Reload()
    {
        uiManager.UpdateAmmo(currentAmmo);
    }*/

}
