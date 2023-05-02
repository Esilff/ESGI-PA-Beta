using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum CameraType
{
    CHARACTER,VEHICLE
}

public class CameraManager : MonoBehaviour
{
    //camera variables
    [SerializeField] PlayerInput input;
    [SerializeField] Transform target, rotatorX;
    [SerializeField] float sensi, rotX, maxX;
    // Update is called once per frame
    [SerializeField] private CameraType camera = CameraType.CHARACTER;
    
    void Update()
    {
        if (camera == CameraType.CHARACTER)
        {
            transform.position = target.position;
        
            Vector2 look = input.actions["Look"].ReadValue<Vector2>();
        
            transform.Rotate(Vector3.up, look.x * sensi * Time.deltaTime);

            rotX -= look.y * sensi * Time.deltaTime;

            if (rotX > maxX) rotX = maxX;
            else if (rotX < -maxX) rotX = -maxX;
        
            rotatorX.localRotation = Quaternion.Euler(rotX, 0, 0);
        }
        /*if (mainCamera.enabled == true && Input.GetKeyDown(KeyCode.F))
        {
            mainCamera.enabled = false;
            CameraFront.enabled = true;
        }
        else if (CameraFront.enabled == true && Input.GetKeyDown(KeyCode.F))
        {
            CameraFront.enabled = false;
            CameraBack.enabled = true;
        }
        else if (CameraBack.enabled == true && Input.GetKeyDown(KeyCode.F))
        {
            CameraBack.enabled = false;
            mainCamera.enabled = true;
        }*/

    }
}
