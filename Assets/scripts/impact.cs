using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impact : MonoBehaviour
{
    [SerializeField] private ParticleSystem partSys;

    private void Update()
    {
        if (!partSys.isPlaying) { Destroy(gameObject); }
    }
}
