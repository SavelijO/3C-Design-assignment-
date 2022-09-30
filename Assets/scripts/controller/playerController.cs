using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class playerController : MonoBehaviour
{
    [SerializeField] private bool isUsingKeyboard;
    [SerializeField] public float deadzone;
    [SerializeField] private float movSpeed;
    [SerializeField] private float rotSpeed;

    [Header("")]
    public Vector3 leftInput;
    public Vector3 rightInput;

    [Header("")]
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private GameObject model;
    [SerializeField] public Transform firePoint;


    [SerializeField] private Camera rayCam;
    [SerializeField] private Transform rayCamPivot;
    




    void Update()
    {     
        if (isUsingKeyboard) {KeyboardUpdate();}
        else { ControllerUpdate();}
    }


    void FixedUpdate()
    {
        playerRb.MovePosition(transform.position + (transform.forward * leftInput.magnitude) * movSpeed * Time.fixedDeltaTime);
        model.transform.position = playerRb.position;
    }

    void KeyboardUpdate()
    {
        GetKeyboardInput();
        RotateRigidBody();
        RotateModel();
    }

    void ControllerUpdate()
    {
        GetControllerInput();
        RotateRigidBody();
        RotateModel();
    }

    void GetKeyboardInput()
    {
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        leftInput = new Vector3(keyboardInput.x, 0, keyboardInput.y);

        rayCamPivot.position = new Vector3(0,this.transform.position.y, 0);
        Ray ray = this.rayCam.ScreenPointToRay(Input.mousePosition);
        Plane rayPlane = new Plane(Vector3.up, new Vector3(0, this.transform.position.y + 0.2f, 0));
        float rayLength;

        if(rayPlane.Raycast(ray, out rayLength))
        {
            Vector3 hitPoint = ray.GetPoint(rayLength);
            Debug.DrawLine(ray.origin, hitPoint, Color.red);
            rightInput = new Vector3(hitPoint.x, 0, hitPoint.z).normalized;
        }
    }

    void GetControllerInput()
    {
        Vector2 stickInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (stickInput.magnitude < deadzone) { stickInput = Vector2.zero; }
        leftInput = new Vector3(stickInput.x, 0, stickInput.y);


        stickInput = new Vector2(Input.GetAxisRaw("HorizontalRight"), Input.GetAxisRaw("VerticalRight"));
        if (stickInput.magnitude < deadzone) { stickInput = Vector2.zero; }
        rightInput = new Vector3(stickInput.x, 0, stickInput.y);
        rightInput = rightInput.ToIso();

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
            Quaternion isoRot = Quaternion.LookRotation(rightInput, Vector3.up);


            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, isoRot, rotSpeed *  360 * Time.deltaTime);
        }

    }

}
