using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletObject;
    [SerializeField] private ParticleSystem trailPartSys;
    [SerializeField] public float fadeTime;


    public IEnumerator Despawn()
    {

        Destroy(bulletObject.gameObject);

        ParticleSystemRenderer trailRenderer = trailPartSys.GetComponent<ParticleSystemRenderer>();

        float lerpTime = 0;

        while(lerpTime < 1)
        {
            trailRenderer.materials[1].color = new Color(0.33f, 0.33f, 0.33f, Mathf.Lerp(1f, 0f, lerpTime));
            lerpTime = lerpTime + (Time.deltaTime / fadeTime);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
