using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private float smoothTime;
    [SerializeField] private GameObject camOffsetObj;
    [SerializeField] private float maxCameraOffset;

    private playerController player;


    private Vector3 smoothDampVelocity = Vector3.zero;


    void Start()
    {
        player = playerObj.GetComponent<playerController>();
    }

    void Update()
    {
        CalculateCameraOffset();
    }

    void FixedUpdate()
    {
        MoveCamera();
    }

    void CalculateCameraOffset()
    {
        camOffsetObj.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(player.leftInput.magnitude - player.deadzone, 0, maxCameraOffset));
    }

    void MoveCamera()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, camOffsetObj.transform.position, ref smoothDampVelocity, smoothTime);
    }

}
