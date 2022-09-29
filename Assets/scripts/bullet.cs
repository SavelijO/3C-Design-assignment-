using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private GameObject impactPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(impactPrefab, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
