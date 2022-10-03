using System.Collections;
using UnityEngine;

public class gun : MonoBehaviour
{
    [Header("Gun Properties")]
    [SerializeField] private float fireRate;
    [SerializeField] private int arc;
    [SerializeField] private float maxShootingDistance;
    [SerializeField] private int bulletCount;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float intialSpread;
    [SerializeField] private float postShotSpread;
    [SerializeField] private int damage;
    [SerializeField] private float trailFadeTime;

    [Header("")]
    [SerializeField] private GameObject bulletPrefab;
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
        if (bulletCount > 1) { rayAngleStep = arc / (bulletCount - 1); }
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

        Physics.Raycast(ray, out hit, maxShootingDistance);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<bullet>().fadeTime = trailFadeTime;
        StartCoroutine(SpawnBullet(ray, hit, bullet, ray.GetPoint(maxShootingDistance)));
    }

    private IEnumerator SpawnBullet(Ray ray, RaycastHit hit, GameObject bullet, Vector3 targetPoint)
    {
        float time = 0;
        Vector3 startPosition = ray.origin;

        while(time < 1)
        {
            bullet.transform.position = Vector3.Lerp(startPosition, targetPoint, time);
            float timeScale = maxShootingDistance / bulletSpeed;
            time = Time.deltaTime / timeScale + time;

            yield return null;
        }

        if (bullet.GetComponent<bullet>() != null) { bullet.GetComponent<bullet>().StartCoroutine(bullet.GetComponent<bullet>().Despawn()); }
    }

    private void SpawnImpact(RaycastHit hit, Vector3 targetPoint)
    {
        if (hit.normal != Vector3.zero) { Instantiate(impact, targetPoint, Quaternion.LookRotation(hit.normal)); }
        else { Instantiate(impact, targetPoint, Quaternion.identity); }
    }
}
