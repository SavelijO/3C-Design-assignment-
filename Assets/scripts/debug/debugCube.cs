using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugCube : MonoBehaviour
{

    void FixedUpdate()
    {
        if(Mathf.Abs(transform.position.z) > 40)
        {
            this.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        this.GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward * 5 * Time.deltaTime);
    }
}
