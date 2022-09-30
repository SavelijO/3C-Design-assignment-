using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    //General variables
    [SerializeField] private GameObject player;

    //Movement variables
    private NavMeshAgent myAgent;
    
    //Player in range variables
    private RaycastHit raycastHit;
    private bool playerInRange;
    private LayerMask whatIsPlayer;
    [SerializeField] private float sphereRangeRadius = 3;
    
    //Health system variables
    [SerializeField] public int health = 100;
    
    //Attack variables
    [SerializeField] private int damage = 20;
    [SerializeField] private float cooldown = 3;
    private bool attacked;

    // Start is called before the first frame update
    void Start()
    {
        whatIsPlayer = LayerMask.GetMask("Player");
        myAgent = this.GetComponent<NavMeshAgent>();
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            DestroyYourself();
        }
        
        CheckIfPlayerInRange();
        
        if (playerInRange && !attacked)
        {
            Attack();
            StartCoroutine(AttackCoolDown());
        }   
    }

    private void FixedUpdate()
    {
        AIMovement();
    }

    private void AIMovement()
    {
        myAgent.SetDestination(player.transform.position);
    }

    private void CheckIfPlayerInRange()
    {
        playerInRange = Physics.CheckSphere(transform.position, sphereRangeRadius, whatIsPlayer);
    }

    //Debug the SphereCast
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRangeRadius);
    }

    private void Attack()
    {
        //player.TakeDamage(damage);
    }

    private void DestroyYourself()
    {
        Destroy(gameObject);
    }
    
    private IEnumerator AttackCoolDown()
    {
        attacked = true;
        yield return new WaitForSeconds(cooldown);
        attacked = false;
    }

}
