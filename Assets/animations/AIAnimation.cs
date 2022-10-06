using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AIAnimation : MonoBehaviour
{
    private Animator myAnimator;
    private AI aiScript;
    private bool alreadyAttacked;
    private float hitAnimationDuration = 0.54f;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = this.GetComponent<Animator>();
        aiScript = this.GetComponent<AI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aiScript.isAttacking)
        {
            Attack();
            StartCoroutine(HitAnimationCooldown());
        }
        else if (aiScript.isMoving)
        {
            Walk();
        }
        else
        {
            Idle();
        }
    }

    private void Attack()
    {
        myAnimator.SetBool("walking", false);
        myAnimator.SetBool("hitting", true);
    }
    
    private void Walk()
    {
        myAnimator.SetBool("walking", true);
        myAnimator.SetBool("hitting", false);
    }
    
    private void Idle()
    {
        myAnimator.SetBool("walking", false);
        myAnimator.SetBool("hitting", false);
    }

    private IEnumerator HitAnimationCooldown()
    {
        yield return new WaitForSeconds(hitAnimationDuration);
        Walk();
    }
}
