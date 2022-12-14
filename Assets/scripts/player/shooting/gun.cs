using System.Collections;
using UnityEngine;

public class gun : MonoBehaviour
{
    [Header("Gun Properties")]
    [SerializeField] private float fireRate;
    [SerializeField] private int arc;
    [SerializeField] private float maxShootingDistance;
    [SerializeField] private int bulletPerShotCount;
    [SerializeField] private int maxShotCount;
    [SerializeField] private float firstBulletReloadTime;
    [SerializeField] private float additionalBulletReloadBonus;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float intialSpread;
    [SerializeField] private float postShotSpread;
    [SerializeField] private int damage;
    [SerializeField] private int damageDistanceLimit;
    [SerializeField] private float trailFadeTime;

    [Header("")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject impact;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator animator;

    private Vector3 correctedForward;
    private Vector3 correctedRight;
    private float rayAngleStep;
    public int shotCount;
    private bool stopReloading = false;
    private bool isReloading;


    private void Start()
    {
        shotCount = maxShotCount;
    }

    private float nextFire = 0f;
    void Update()
    {
        if (Input.GetAxisRaw("Fire1") != 0 && Time.time > nextFire)
        {
            if(shotCount == 0 && !isReloading)
            {
                stopReloading = false;
                StartCoroutine(Reload());
            }
            else if (shotCount > 0)
            {
                Fire();
                stopReloading = true;
            }
        }

        if(Input.GetButtonDown("Reload") && !isReloading)
        {
            stopReloading = false;
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {

        CreateArc();
        StartCoroutine(FireAnim());
        for (int i = 0; i < bulletPerShotCount; i++)
		{
            HitScan(i);
        }
        shotCount--;
        nextFire = Time.time + fireRate;
    }

    IEnumerator FireAnim()
    {
        animator.SetBool("isShootingAnim", true);
        animator.SetBool("isReloadingAnim", false);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isShootingAnim", false);
    }

    void CreateArc()
    {
        correctedForward = (firePoint.forward * Mathf.Cos(-arc / 2 * Mathf.Deg2Rad) + firePoint.right * Mathf.Sin(-arc / 2 * Mathf.Deg2Rad)).normalized;
        correctedRight = Quaternion.AngleAxis(90, Vector3.up) * correctedForward;
        if (bulletPerShotCount > 1) { rayAngleStep = arc / (bulletPerShotCount - 1); }
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
        bullet.GetComponent<bullet>().damage = damage;
        bullet.GetComponent<bullet>().damageDistanceLimit = damageDistanceLimit;
        StartCoroutine(SpawnBullet(ray, hit, bullet, ray.GetPoint(maxShootingDistance)));
    }

    private IEnumerator SpawnBullet(Ray ray, RaycastHit hit, GameObject bullet, Vector3 targetPoint)
    {
        float time = 0;
        Vector3 startPosition = ray.origin;

        while(time < 1)
        {
            if(bullet.GetComponent<bullet>().hasCollided) { break; }
            bullet.transform.position = Vector3.Lerp(startPosition, targetPoint, time);
            float timeScale = maxShootingDistance / bulletSpeed;
            time = Time.deltaTime / timeScale + time;

            yield return null;
        }


        bullet.GetComponent<bullet>().StartCoroutine(bullet.GetComponent<bullet>().Despawn());
    }

    private void SpawnImpact(RaycastHit hit, Vector3 targetPoint)
    {
        if (hit.normal != Vector3.zero) { Instantiate(impact, targetPoint, Quaternion.LookRotation(hit.normal)); }
        else { Instantiate(impact, targetPoint, Quaternion.identity); }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        float currentLoaded = 0f;
        for (int i = shotCount; i < maxShotCount; i++)
        {
            animator.SetBool("isReloadingAnim", true);
            currentLoaded++;
            yield return new WaitForSeconds(firstBulletReloadTime - currentLoaded * additionalBulletReloadBonus);
            animator.SetBool("isReloadingAnim", false);
            if (stopReloading) { break; }
            shotCount++;
            nextFire = Time.time + fireRate;
        }
        isReloading = false;
        currentLoaded = 0;
        yield return null;
    }
}
