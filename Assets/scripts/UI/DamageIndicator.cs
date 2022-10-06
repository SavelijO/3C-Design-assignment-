using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Text text;
    public float lifeTime = 0.6f;
    public float minDist = 2f;
    public float maxDist = 3f;

    private Vector3 iniPos;
    private Vector3 targetPos;
    private float timer;

    private void Start()
    {
        //find and look at camera - so that 2D objects always turn to camera
        //transform.LookAt(2 * transform.position - Camera.main.transform.position);
        transform.Rotate(0, 45, 0);

        //position of the damage pop-ups numbers
        float direction = Random.rotation.eulerAngles.z;
        iniPos = transform.position;
        float dist = Random.Range(minDist, maxDist);
        targetPos = iniPos + (Quaternion.Euler(0, 0, direction) * new Vector3(dist, dist, 0f));
        transform.localScale = Vector3.zero;
              
    }

    private void Update()
    {
        //lifetime of damage pop-ups
        timer += Time.deltaTime;
        //fade effect on the numbers
        float fraction = lifeTime / 2f; 

        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }

        //fade effect
        else if(timer > fraction)
        {
            text.color = Color.Lerp(text.color, Color.clear, (timer - fraction)/(lifeTime - fraction));
        }

        transform.position = Vector3.Lerp(iniPos, targetPos, Mathf.Sin(timer / lifeTime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifeTime));
    }

    //set the value of the damage text
    public void SetDamageText (int damage)
    {
        text.text = damage.ToString();
    }

}
