using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletForce;
    [SerializeField] private Camera shootCam;



    private float nextFire = 0f;
    void Update()
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
            targetPoint = ray.GetPoint(75);
        }

        Debug.DrawLine(ray.origin, hit.point, Color.red);



        nextFire = Time.time + fireRate;
    }
}
