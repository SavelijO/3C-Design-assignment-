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
    private SpawnManager spawnManager;

    //Movement variables
    private NavMeshAgent myAgent;
    [SerializeField] private float speedRangeTop = 15;
    [SerializeField] private float speedRangeBottom = 6;
    private Transform goal;
    private Rigidbody myRigidbody;
    public bool isMoving;
    
    //Player in range variables
    private RaycastHit raycastHit;
    private bool playerInRange;
    private LayerMask whatIsPlayer;
    [SerializeField] private float sphereRangeRadius = 20;
    
    //Health system variables
    [SerializeField] public int health = 100;
    [SerializeField] private float pushback = 2;
    [SerializeField] private float recoilCooldown = 1;

    //Attack variables
    [SerializeField] private int damage = 20;
    [SerializeField] public float attackCooldown = 3;
    private bool attacked;
    public bool isAttacking;
    private float sphereHitRadius = 2;
    public bool hitted;
    private EnemyDamage myEnemyDamage;
    private float attackDelay = 0.5f;
    
    //Color variables
    private Gradient gradient;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject leftBackLeg;
    [SerializeField] private GameObject leftFrontLeg;
    [SerializeField] private GameObject rightBackLeg;
    [SerializeField] private GameObject rightFrontLeg;

    
    // Start is called before the first frame update
    void Start()
    {
        whatIsPlayer = LayerMask.GetMask("Player");
        myAgent = this.GetComponent<NavMeshAgent>();
        player = GameObject.Find("player");
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        myAgent.speed = Random.Range(speedRangeBottom, speedRangeTop);
        myEnemyDamage = this.GetComponent<EnemyDamage>();
        
        //Create color keys for gradient
        gradient = new Gradient();
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
            isAttacking = true;
            StartCoroutine(AttackCoolDown());
        }

        speedRangeTop += (spawnManager.wave * 0.2f);
        speedRangeBottom += (spawnManager.wave * 0.1f);

    }

    private void FixedUpdate()
    {
        if (attacked)
        {
            AIMovementAfterAttack();
        }
        else
        {
            AIMovement();
        }
        
    }

    private void AIMovement()
    {
        if (myAgent.enabled)
        {
            isMoving = true;
            myAgent.SetDestination(player.transform.position);
            this.transform.LookAt(player.transform.position);
        }
        else
        {
            isMoving = false;
        }
        
        
    }

    private void AIMovementAfterAttack()
    {
        if (myAgent.enabled)
        {
            isMoving = true;
            myAgent.SetDestination(player.transform.position - this.transform.position + (player.transform.position*0.005f));
            this.transform.LookAt(player.transform.position);
        }
        else
        {
            isMoving = false;
        }
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
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward*2, sphereHitRadius);
    }

    private void Attack()
    {
        StartCoroutine(AttackDelay());
  
    }
    private void DestroyYourself()
    {
        spawnManager.EnemyDied();
        player.GetComponent<playerController>().RestoreDrain();
        Destroy(gameObject);
    }

    private IEnumerator AttackCoolDown()
    {
        attacked = true;
        yield return new WaitForSeconds(0.9f);
        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown-0.9f);
        attacked = false;
    }
    
    private IEnumerator PushbackCoolDown()
    {
        yield return new WaitForSeconds(recoilCooldown);
        myAgent.enabled = true;
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        myAgent.enabled = false;
        if (Physics.CheckSphere(transform.position + transform.forward * 2, sphereHitRadius, whatIsPlayer) && !hitted)
        {
            player.GetComponent<playerController>().TakeDamage(damage);
        }
        yield return new WaitForSeconds(0.15f);
        myAgent.enabled = true;
    }

    public void ReduceHealth(int bulletDamage)
    {
        this.health -= bulletDamage;
        ChangeColor();
        myEnemyDamage.SpawnDamageText(bulletDamage);
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        //Disable Agent so Rigidbody can be used to apply the recoil and then reactivate it
        myAgent.enabled = false;
        Vector3 vectorPushback = this.transform.position + ((-this.transform.forward) * pushback);
        Vector3.Lerp(this.transform.position, vectorPushback, recoilCooldown);
        StartCoroutine(PushbackCoolDown());
    }

    //Change AI color based on its health
    private void ChangeColor()
    {
        body.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", gradient.Evaluate(health/100f));
        leftBackLeg.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", gradient.Evaluate(health/100f));
        leftFrontLeg.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", gradient.Evaluate(health/100f));
        rightBackLeg.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", gradient.Evaluate(health/100f));
        rightFrontLeg.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", gradient.Evaluate(health/100f));
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
