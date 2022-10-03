using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    [SerializeField] private int damage;
    [Header("")]
    [SerializeField] private TrailRenderer smokeTrailPrefab;
    [SerializeField] private TrailRenderer hotTrailPrefab;
    [SerializeField] private GameObject impact;
    [SerializeField] private Transform firePoint;

    private Vector3 correctedForward;
    private Vector3 correctedRight;
    private float rayAngleStep;




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
        CreateArc();    
        
        for (int i = 0; i < bulletCount; i++)
		{
            HitScan(i);
        }
        
        nextFire = Time.time + fireRate;
    }

    void CreateArc()
    {
        correctedForward = (firePoint.forward * Mathf.Cos(-arc / 2 * Mathf.Deg2Rad) + firePoint.right * Mathf.Sin(-arc / 2 * Mathf.Deg2Rad)).normalized;
        correctedRight = Quaternion.AngleAxis(90, Vector3.up) * correctedForward;
        rayAngleStep = arc / (bulletCount - 1);
    }
    
    Vector3 CorrectedDir(int index)
    {
        float rayAngle = rayAngleStep * index * Mathf.Deg2Rad;
        Vector3 rayDir = (correctedForward * Mathf.Cos(rayAngle) + correctedRight * Mathf.Sin(rayAngle)).normalized;

        float xSpread = Random.Range(-postShotSpread, postShotSpread);
        float ySpread = Random.Range(-postShotSpread, postShotSpread);

        return rayDir + new Vector3(xSpread, ySpread, 0);
    }

    void HitScan(int index)
    {
        Ray ray = new Ray(firePoint.position + new Vector3(Random.Range(-intialSpread, intialSpread), 0, Random.Range(-intialSpread, intialSpread)), CorrectedDir(index));
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

        TrailRenderer smokeTrail = Instantiate(smokeTrailPrefab, firePoint.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(ray, hit, smokeTrail, ray.GetPoint(maxShootingDistance)));

        SpawnImpact(hit, targetPoint);

        DoDamage(hit);
    }

    void DoDamage(RaycastHit hit)
    {
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<AI>().health -= damage;
            }
        }
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

        Destroy(trail.gameObject, trail.time);
    }

    private void SpawnImpact(RaycastHit hit, Vector3 targetPoint)
    {
        if (hit.normal != Vector3.zero) { Instantiate(impact, targetPoint, Quaternion.LookRotation(hit.normal)); }
        else { Instantiate(impact, targetPoint, Quaternion.identity); }
    }
}