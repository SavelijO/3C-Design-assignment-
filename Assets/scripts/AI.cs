using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class AI : MonoBehaviour
{
    //General variables
    [SerializeField] private GameObject player;
    
    //Movement variables
    [SerializeField] private float speed = 3f;
    [SerializeField] private float acceleration = 10;
    private Rigidbody aiRigidbody;
    private Vector3 destination;

    //Player in range variables
    private RaycastHit raycastHit;
    private bool playerInRange;
    private LayerMask whatIsPlayer;
    [SerializeField] private float sphereRangeRadius = 3;
    
    //Health system variables
    [SerializeField] private int health = 100;
    
    //Attack variables
    [SerializeField] private int damage = 20;
    [SerializeField] private float cooldown = 3;
    private bool attacked;

    // Start is called before the first frame update
    void Start()
    {
        aiRigidbody = this.GetComponent<Rigidbody>();
        whatIsPlayer = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            DestroyYourself();
        }

        AttackCooldown();
        
        AIMovement();
        
        CheckIfPlayerInRange();

        
        if (playerInRange && Input.GetKeyDown(KeyCode.G))
        {
            Attack();
            attacked = true;
        }

        Debug.LogError(cooldown);
        
    }

    private void AIMovement()
    {
        destination = player.transform.position;
        transform.LookAt(destination);
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        //If you haven't reached half of the top speed, accelerate
        if (aiRigidbody.velocity.sqrMagnitude < speed/2)
        {
            aiRigidbody.AddForce(acceleration * this.transform.forward, ForceMode.Acceleration);
        }
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
        DestroyImmediate(this);
    }

    private void AttackCooldown()
    {
        if (attacked && cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else if(cooldown <= 0 && attacked)
        {
            cooldown = 0;
            attacked = false;
        }
    }
    
}
