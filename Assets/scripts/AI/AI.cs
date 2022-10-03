using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour
{
    //General variables
    private GameObject player;

    //Movement variables
    private NavMeshAgent myAgent;
    [SerializeField] private float speedRangeTop = 15;
    [SerializeField] private float speedRangeBottom = 6;
    private Transform goal;
    private Rigidbody myRigidbody;
    private NavMeshPath newPath;
    
    //Player in range variables
    private RaycastHit raycastHit;
    private bool playerInRange;
    private LayerMask whatIsPlayer;
    [SerializeField] private float sphereRangeRadius = 20;
    
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
        myAgent.speed = Random.Range(speedRangeBottom, speedRangeTop);
        myRigidbody = this.GetComponent<Rigidbody>();
        newPath = new NavMeshPath();
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
        //Don't move agent with NavMesh
        /*
        myAgent.updatePosition = false;
        myAgent.updateRotation = false;

        if (myAgent.CalculatePath(player.transform.position, newPath))
        {
            //myRigidbody.MovePosition(newPath.corners[0] * (myAgent.speed * Time.fixedDeltaTime));

            for (int i = 0; i < newPath.corners.Length - 1; i++)
            {
                myAgent.nextPosition = newPath.corners[i];
                myRigidbody.MovePosition(newPath.corners[i] * (speedRangeBottom * Time.fixedDeltaTime) + myRigidbody.position);
                //transform.position = Vector3.MoveTowards(transform.position, newPath.corners[i], speedRangeBottom * Time.fixedDeltaTime);
            }
        }
        
        for (int i = 0; i < newPath.corners.Length - 1; i++)
            Debug.DrawLine(newPath.corners[i], newPath.corners[i + 1], Color.red);
        */

        myAgent.SetDestination(player.transform.position);
        this.transform.LookAt(player.transform.position);

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

    private void ChangeColourBasedOnHealth()
    {
        //Change colour of AI based on damage
    }
    
    private IEnumerator AttackCoolDown()
    {
        attacked = true;
        yield return new WaitForSeconds(cooldown);
        attacked = false;
    }

}
