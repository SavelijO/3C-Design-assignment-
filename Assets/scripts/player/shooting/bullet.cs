using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem trailPartSys;
    [SerializeField] public float fadeTime;
    [SerializeField] public bool hasCollided;

    private void OnTriggerEnter(Collider other)
    {
        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        this.GetComponent<MeshCollider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    public IEnumerator Despawn()
    {
        DestroyProjectile();

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
