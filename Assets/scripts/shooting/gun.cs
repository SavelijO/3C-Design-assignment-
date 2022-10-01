using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    [Header("Gun Properties")]
    [SerializeField] private float fireRate;
    [SerializeField] private int arc;
    [SerializeField] private int maxShootingDistance;
    [SerializeField] private int bulletCount;
    [SerializeField] private float intialSpread;
    [SerializeField] private float postShotSpread;
    [Header("")]
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private GameObject impact;
    [SerializeField] private Transform firePoint;




    private float nextFire = 0f;
    void Update()
    {
        if (Input.GetAxisRaw("Fire1") != 0 && Time.time > nextFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        Vector3 correctedForward = (firePoint.forward * Mathf.Cos(-arc / 2 * Mathf.Deg2Rad) + firePoint.right * Mathf.Sin(-arc / 2 * Mathf.Deg2Rad)).normalized;
        Vector3 correctedRight = Quaternion.AngleAxis(90, Vector3.up) * correctedForward;
        
        float rayStep = arc / (bulletCount - 1);
        for (int i = 0; i < bulletCount; i++)
		{
            float rayAngle = rayStep * i * Mathf.Deg2Rad;
            Vector3 rayDir = (correctedForward * Mathf.Cos(rayAngle) + correctedRight * Mathf.Sin(rayAngle)).normalized;
            
            float xSpread = Random.Range(-postShotSpread, postShotSpread);
            float ySpread = Random.Range(-postShotSpread, postShotSpread);
            
            Vector3 correctedDir = rayDir + new Vector3(xSpread, ySpread, 0);
            
            Ray ray = new Ray(firePoint.position + new Vector3(Random.Range(-intialSpread, intialSpread),0, Random.Range(-intialSpread, intialSpread)), correctedDir);
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, maxShootingDistance))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(maxShootingDistance);
            }

            TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(ray, hit, trail, targetPoint));

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<AI>().health -= 10;
                }
            }
        }
        
        nextFire = Time.time + fireRate;
    }

    private IEnumerator SpawnTrail(Ray ray, RaycastHit hit, TrailRenderer trail, Vector3 targetPoint)
    {
        float time = 0;
        Vector3 startPosition = ray.origin;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, targetPoint, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = targetPoint;
        if (hit.normal != Vector3.zero) { Instantiate(impact, targetPoint, Quaternion.LookRotation(hit.normal)); }
        else { Instantiate(impact, targetPoint, Quaternion.identity); }

        Destroy(trail.gameObject, trail.time);
    }
}
