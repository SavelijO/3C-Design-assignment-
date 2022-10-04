using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] private float accTime;
    [SerializeField] private float maxCameraOffset;


    [Header("")]
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject camOffsetObj;
    private playerController controller;



    private Vector3 smoothDampVelocity = Vector3.zero;


    void Start()
    {
        controller = playerObj.GetComponent<playerController>();
    }

    void Update()
    {
        CalculateCameraOffset();
        MoveCamera();
    }


    void CalculateCameraOffset()
    {
        camOffsetObj.transform.localPosition = new Vector3(0, 0, Mathf.Clamp((controller.leftInput.magnitude - controller.deadzone) * maxCameraOffset, 0, maxCameraOffset));
    }

    void MoveCamera()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, playerObj.GetComponent<Rigidbody>().position, Time.deltaTime * 10);
    }


}
