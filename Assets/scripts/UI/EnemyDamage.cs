using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour

{

    //damage pop-ups for enemies
    public GameObject damageText;
    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 2, 0);
    }

   public void SpawnDamageText(int damage)
    {
        DamageIndicator indicator = Instantiate(damageText, transform.position + offset, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(damage);
    }
    
}
