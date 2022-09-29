using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public Vector3 leftInput;
    public Vector3 rightInput;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private float movSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private GameObject model;
    [SerializeField] public float deadzone;





    void Update()
    {
        GetInput();

        RotateRigidBody();

        RotateModel();

    }


    void FixedUpdate()
    {
        playerRb.MovePosition(transform.position + (transform.forward * leftInput.magnitude) * movSpeed * Time.fixedDeltaTime);
        model.transform.position = playerRb.position;
    }


    void GetInput()
    {
        Vector2 stickInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (stickInput.magnitude < deadzone) { stickInput = Vector2.zero; }
        leftInput = new Vector3(stickInput.x, 0, stickInput.y);

        stickInput = new Vector2(Input.GetAxisRaw("HorizontalRight"), Input.GetAxisRaw("VerticalRight"));
        if (stickInput.magnitude < deadzone) { stickInput = Vector2.zero; }
        rightInput = new Vector3(stickInput.x, 0, stickInput.y);

    }

    void RotateRigidBody()
    {
        if (leftInput != Vector3.zero)
        {
            Quaternion isoRot = Quaternion.LookRotation(leftInput.ToIso(), Vector3.up);

            playerRb.transform.rotation = isoRot;
        }
    }

    void RotateModel()
    {

        if (leftInput != Vector3.zero && rightInput == Vector3.zero)
        {

            Quaternion isoRot = Quaternion.LookRotation(leftInput.ToIso(), Vector3.up);

            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, isoRot, rotSpeed * 360 *Time.deltaTime);
        }
        if(rightInput != Vector3.zero)
        {
            Quaternion isoRot = Quaternion.LookRotation(rightInput.ToIso(), Vector3.up);
            
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, isoRot, rotSpeed *  360 * Time.deltaTime);
        }

    }

}
