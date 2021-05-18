using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    //can move view around scene to look at planets around the system
  
    //Physics Components
    private Rigidbody rb;
    
    [SerializeField]
    //MOVE VARIABLES that are serialized fields
    private float TurnSpeed, propulsionMod, torqueMod;

    //Input and Move Variables
    private float AxisH, AxisV;
    private Vector2 lookInput;
    private Vector2 mouseValue;
    private Vector2 screenCenter;
    private bool Unlock;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;
    }

    // Update is called once per frame
    void Update()
    {
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;
        
        mouseValue.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseValue.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseValue = Vector2.ClampMagnitude(mouseValue, 1f);
        

        AxisH = Input.GetAxisRaw("Horizontal");
        AxisV = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        CamStarshipMovement(AxisH, AxisV, mouseValue);
    }

    void CameraMove(float AxisH, float AxisV)
    {
        if (Mathf.Abs(AxisH) > .3)
        {
            var pos = transform.rotation;
            pos.y += Time.fixedDeltaTime * AxisH * TurnSpeed;
            transform.rotation = pos;
        }   
    }

    public void CamStarshipMovement(float AxisH, float AxisV, Vector2 mouseValues)
    {

        if (AxisH != 0 || AxisV != 0)
        {
            rb.AddForce(transform.TransformDirection(Vector3.forward) * AxisV * (propulsionMod * 2) * Time.fixedDeltaTime);
            //rb.AddTorque(transform.TransformDirection(Vector3.forward) * AxisH * -(torqueMod / 10) * Time.fixedDeltaTime, ForceMode.Impulse);
            //rb.AddForce(transform.TransformDirection(Vector3.right) * AxisH * (torqueMod / 10) * Time.fixedDeltaTime, ForceMode.Impulse);
            rb.AddTorque(transform.TransformDirection(Vector3.up) * AxisH * (torqueMod / 5) * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity *= .95f;
            rb.angularVelocity *= .95f;
        }

        //if (mouseValues.magnitude >= .6f || mouseValues.magnitude <= -.6f)
        //{
        //    rb.AddTorque(transform.TransformDirection(Vector3.up) * mouseValues.x * (torqueMod / 2) * Time.fixedDeltaTime);
        //    rb.AddTorque(transform.TransformDirection(Vector3.right) * -mouseValues.y * (torqueMod / 2) * Time.fixedDeltaTime);
        //}
            
    }

}
