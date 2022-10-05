using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem trailPartSys;
    public float fadeTime;
    public float damageDistanceLimit;
    public float damage;
    public bool hasCollided;
    private Vector3 initPosition;

    void Start()
    {
        initPosition = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AI"))
        {
            other.GetComponent<AI>().ReduceHealth(CalculateProximityDamage());
        }

        if (!other.gameObject.CompareTag("Bullet"))
        {
            DestroyProjectile();
            hasCollided = true;
        }

        
    }

    int CalculateProximityDamage()
    {
        float proximityDamage = damage;
        float distance = (this.transform.position - initPosition).magnitude / damageDistanceLimit;
        proximityDamage *= Mathf.Lerp(1,0, distance);
        return (int)proximityDamage;
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
