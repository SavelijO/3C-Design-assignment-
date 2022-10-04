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
    [SerializeField] private float recoil = 2;
    
    //Attack variables
    [SerializeField] private int damage = 20;
    [SerializeField] private float cooldown = 3;
    private bool attacked;
    
    //Color variables
    [SerializeField] private Gradient gradient;

    // Start is called before the first frame update
    void Start()
    {
        whatIsPlayer = LayerMask.GetMask("Player");
        myAgent = this.GetComponent<NavMeshAgent>();
        player = GameObject.Find("player");
        myAgent.speed = Random.Range(speedRangeBottom, speedRangeTop);
        myRigidbody = this.GetComponent<Rigidbody>();
        newPath = new NavMeshPath();
        //Create color keys for gradient
        CreateGradient();
        ChangeColor();

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
        this.transform.LookAt(player.transform.position);
    }

    private void CheckIfPlayerInRange()
    {
        playerInRange = Physics.CheckSphere(transform.position, sphereRangeRadius, whatIsPlayer);
    }

    //Debug the SphereCast used to check if the player is in range
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

    private void ReduceHealth(int bulletDamage)
    {
        this.health -= bulletDamage;
        ChangeColor();
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            ReduceHealth(10);
        }

        myAgent.enabled = false;
        myRigidbody.AddForceAtPosition(collision.transform.forward * recoil, collision.transform.position, ForceMode.Impulse);
        myAgent.enabled = true;

    }

    //Change AI color based on its health
    private void ChangeColor()
    {
        this.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", gradient.Evaluate(health/100f));
    }

    private void CreateGradient()
    {
        //Set the gradient mode
        gradient.mode = GradientMode.Blend;

        //Create the arrays for the gradient
        GradientColorKey[] myGradientColorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        
        //Set the colors to blend and where to blend
        myGradientColorKeys[0].color = Color.grey;
        myGradientColorKeys[1].color = Color.cyan;
        myGradientColorKeys[0].time = 0f;
        myGradientColorKeys[1].time = 1f;

        //Set the alpha to blend
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        //Set the keys and alpha arrays to be the ones for the gradient
        gradient.SetKeys(myGradientColorKeys, alphaKeys);
    }
}
