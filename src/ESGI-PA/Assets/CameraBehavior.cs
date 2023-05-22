using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[Serializable]
struct CameraOptions
{
    public float distance;
    [Range(1,10)]
    public float sensitivity;
    public float speed;
    public float maxPitch;
    public float detectionRadius;
}

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Transform target;
    [SerializeField] private PlayerInput input;
    
    [SerializeField] private CameraOptions options;
    [SerializeField] private bool locked = false;
    
    private Vector2 axis;
    private Vector3 velocity;
    private float currentPitch;
    private RaycastHit info;
    private float defaultDistance;
    [Range(0,1)]
    [SerializeField] private float cameraFollowSpeed;
    void Start()
    {
        camera.position = target.position + new Vector3(0, 2, -options.distance);
        currentPitch = camera.transform.rotation.eulerAngles.x;
        defaultDistance = options.distance;
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (locked)
        {
            LockedCamera();
        }
        else
        {
            FreeCamera();
        }
    }

    private void LockedCamera()
    {
        // camera.position = Vector3.Lerp(camera.position,
        //     target.position - ((target.forward * options.distance) - target.up * 2) , cameraFollowSpeed);
        //camera.LookAt(target);
        /*camera.position = 
        camera.rotation = Quaternion.Lerp(camera.rotation, target.rotation, 0.1f); //Quaternion.Lerp(camera.rotation, target.rotation, 0.9f);*/
        // camera.LookAt(target);
        // Vector3 cameraPos = target.position + new Vector3(0, 2, -options.distance);
        camera.position = Vector3.Lerp(camera.position, target.position - ((target.forward * options.distance) - target.up * 2) , 0.9f);
        Quaternion diff = camera.rotation * Quaternion.Inverse(target.rotation);
        camera.rotation = Quaternion.Lerp(camera.rotation, target.rotation, 0.9f);
        
       
        // Debug.Log("Quaternion inverse : " + diff);
    }

    private void FreeCamera()
    {
        axis = input.actions["Look"].ReadValue<Vector2>() * (options.sensitivity * 100 * Time.deltaTime);
        camera.RotateAround(target.position, Vector3.up, axis.x);
        currentPitch += -axis.y;
        currentPitch = Mathf.Clamp(currentPitch, -options.maxPitch, options.maxPitch);
        var cameraRot = camera.transform.rotation;
        camera.transform.rotation = Quaternion.Euler(currentPitch, cameraRot.eulerAngles.y, cameraRot.eulerAngles.z);
        Vector3 nextPosition = target.position - (camera.forward * options.distance) + new Vector3(0,2,0) ;
        camera.position = nextPosition;
    }
}
