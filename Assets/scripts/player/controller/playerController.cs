using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.Experimental.GraphView.GraphView;

public class playerController : MonoBehaviour
{
    [Header("")]
    [Header("Controls")]
    [SerializeField] private bool isUsingKeyboard;
    [SerializeField] public float deadzone;
    
    [Header("")]
    public Vector3 leftInput;
    public Vector3 rightInput;


    [Header("")]
    [Header("Dash")]
    [SerializeField] private float dashMultiplier;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    private float currentDashTime = 0;
    private float currentDashCooldown = 0;


    [Header("")]
    [Header("RigidBody")]
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private float maxMovSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float desceleration;
    private float movSpeed;

    [Header("")]
    [Header("Model")]
    [SerializeField] private GameObject model;
    [SerializeField] public Transform firePoint;
    [SerializeField] private float rotSpeed;

    [Header("")]
    [Header("Camera")]
    [SerializeField] private Camera rayCam;
    [SerializeField] private Transform rayCamPivot;

    private bool isMoving = false;
    private bool isDashing = false;
    private bool isCollidingWithScenery = false;
    private float time;



    void Start()
    {
        if (!isUsingKeyboard) 
        {
            Cursor.visible = false;              
        }
    }



    void Update()
    {
        time = Time.time;
        if (isUsingKeyboard) { GetKeyboardInput(); }
        else { GetControllerInput();}
        GetMovSpeed();


        CheckDash();
        RotateModel();
    }


    void FixedUpdate()
    {
        if (isDashing) { Dash(); }
        else
        {
            RotateRigidBody();
            MoveModel();
            UpdateRigidBodyMovement();
        }
    }





    void GetKeyboardInput()
    {
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        leftInput = new Vector3(keyboardInput.x, 0, keyboardInput.y);
        if (leftInput == Vector3.zero) { isMoving = false; }
        else { isMoving = true; }

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
        if (stickInput.magnitude < deadzone) { stickInput = Vector2.zero; isMoving = false; }
        else { isMoving = true; }
        leftInput = new Vector3(stickInput.x, 0, stickInput.y);

        stickInput = new Vector2(Input.GetAxisRaw("HorizontalRight"), Input.GetAxisRaw("VerticalRight"));
        if (stickInput.magnitude < deadzone) { stickInput = Vector2.zero; }
        rightInput = new Vector3(stickInput.x, 0, stickInput.y);
        rightInput = rightInput.ToIso();
    }

    void MoveModel()
    {
        model.GetComponent<Rigidbody>().MovePosition(playerRb.position);
    }
    
    void RotateModel()
    {

        if (leftInput != Vector3.zero && rightInput == Vector3.zero)
        {

            Quaternion isoRot = Quaternion.LookRotation(leftInput.ToIso(), Vector3.up);

            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, isoRot, rotSpeed * 360 * Time.fixedDeltaTime);
        }
        if (rightInput != Vector3.zero && !isDashing)
        {
            Quaternion isoRot = Quaternion.LookRotation(rightInput, Vector3.up);


            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, isoRot, rotSpeed * 360 * Time.fixedDeltaTime);
        }

    }


    void RotateRigidBody()
    {
        if (leftInput != Vector3.zero && !isDashing)
        {
            Quaternion isoRot = Quaternion.LookRotation(leftInput.ToIso(), Vector3.up);
            playerRb.MoveRotation(isoRot);
        }
    }

    void UpdateRigidBodyMovement()
    {
        playerRb.MovePosition(transform.forward * movSpeed * Time.fixedDeltaTime + playerRb.position);
    }


    void GetMovSpeed()
    {

        if(leftInput !=  Vector3.zero)
        {
            movSpeed = Mathf.Lerp(movSpeed, maxMovSpeed, Time.deltaTime * acceleration);
        }
        else
        {
            movSpeed = Mathf.Lerp(movSpeed, 0, Time.deltaTime * desceleration);
        }
    }
    
    void CheckDash()
    {
        if (currentDashCooldown < time && isMoving && Input.GetButtonDown("Dash") && !isCollidingWithScenery)
        {
            isDashing = true;
            currentDashTime = dashTime + time;
            currentDashCooldown = dashCooldown + time;
        }
        else if (currentDashTime > time)
        {
            currentDashTime -= Time.deltaTime;
        }
        else if (currentDashCooldown > time)
        {
            isDashing = false;
            currentDashCooldown -= Time.deltaTime;
        }
    }

    void Dash()
    {
        playerRb.MovePosition(transform.position + transform.forward * maxMovSpeed * dashMultiplier * Time.fixedDeltaTime);
    }
   

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Scenery")) { isCollidingWithScenery = true; }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Scenery")) { isCollidingWithScenery = false; }
    }
}
