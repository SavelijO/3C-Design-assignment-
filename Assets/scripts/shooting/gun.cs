using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate;
    [SerializeField] private Camera shootCam;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private GameObject impact;



    private float nextFire = 0f;
    void FixedUpdate()
    {
        if(Input.GetAxisRaw("Fire1") != 0 && Time.time > nextFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        Ray ray = shootCam.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(10);
        }

        Debug.DrawLine(ray.origin, targetPoint, Color.red);

        TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

        StartCoroutine(SpawnTrail(trail, targetPoint));

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<AI>().health -= 10;
            }
        }

        nextFire = Time.time + fireRate;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 targetPoint)
    {
        float time = 0;
        Vector3 startPosition = firePoint.transform.position;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, targetPoint, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = targetPoint;
        Instantiate(impact, targetPoint, Quaternion.identity);

        Destroy(trail.gameObject, trail.time);
    }
}
