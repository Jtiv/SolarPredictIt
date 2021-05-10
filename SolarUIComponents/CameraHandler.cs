using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    //can move view around scene to look at planets around the system

    //MOVE VARIABLES   
    private float AxisH, AxisV;
    [SerializeField]
    //MOVE VARIABLES that are serialized fields
    private float CameraMoveSpeed, TurnSpeed;

    // Update is called once per frame
    void Update()
    {
        AxisH = Input.GetAxisRaw("Horizontal");
        AxisV = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        CameraMove(AxisH, AxisV);
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
}
