using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private float accTime;
    [SerializeField] private float decTime;
    [SerializeField] private GameObject camOffsetObj;
    [SerializeField] private float maxCameraOffset;
    [SerializeField] private float decDelay;

    private playerController controller;

    private float returnTime = 0;
    private bool isTimerSet = false;



    private Vector3 smoothDampVelocity = Vector3.zero;


    void Start()
    {
        controller = playerObj.GetComponent<playerController>();
    }

    void Update()
    {
        CalculateCameraOffset();

        if (Mathf.Approximately(this.transform.position.x, camOffsetObj.transform.position.x) && Mathf.Approximately(this.transform.position.z, camOffsetObj.transform.position.z))
        {
            Debug.Log("the positions are the same");
        }
    }

    void FixedUpdate()
    {
        MoveCamera();
    }

    void CalculateCameraOffset()
    {
        camOffsetObj.transform.localPosition = new Vector3(0, 0, Mathf.Clamp((controller.leftInput.magnitude - controller.deadzone) * maxCameraOffset, 0, maxCameraOffset));
    }

    void MoveCamera()
    {
        if(controller.leftInput.magnitude > 0)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, camOffsetObj.transform.position, ref smoothDampVelocity, accTime);
        }
        
        else if (controller.leftInput.magnitude == 0 && !isTimerSet)
        {
            Debug.Log("setTimer");
            isTimerSet = true;
            returnTime = Time.time + decDelay;
        }
        
        else if(controller.leftInput.magnitude == 0 && Time.time > returnTime)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, camOffsetObj.transform.position, ref smoothDampVelocity, decTime);
            if(Mathf.Approximately(this.transform.position.x, camOffsetObj.transform.position.x) && Mathf.Approximately(this.transform.position.z, camOffsetObj.transform.position.z))
            { 
                isTimerSet = false;
                Debug.Log("setPos");
            }
        }

    }


}
