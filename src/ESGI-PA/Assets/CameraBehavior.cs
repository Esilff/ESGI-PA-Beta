using System;
using System.Collections;
using System.Security.Cryptography;
using Grpc.Core.Logging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[Serializable]
struct CameraOptions
{
    public float distance;
    public float vehicleDistance;
    [Range(1, 10)] 
    
    public float sensitivity;
    public float speed;
    public float maxPitch;
    public float detectionRadius;
}

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Transform target;
    [SerializeField] private PhysicCharacter character;
    
    [SerializeField] private CameraOptions options;
    [SerializeField] private bool locked = false;
    public bool Locked
    {
        get => locked;
        set => locked = value;
    }

    public Transform Target
    {
        get => target;
        set => target = value;
    }
    
    private Vector2 axis;
    private Vector3 velocity;
    private float currentPitch;
    private RaycastHit info;
    void Start()
    {
        camera.position = target.position + new Vector3(0, 2, -options.distance);
        currentPitch = camera.transform.rotation.eulerAngles.x;
    }

    void Update()
    {
        
        if (!locked)
        {
            FreeCamera();
        }
    }

    private void FixedUpdate()
    {
        if (locked)
        {
            LockedCamera();
        }
    }

    private void LockedCamera()
    {
        // Debug.Log("Camera is locked");
        camera.position = Vector3.Lerp(camera.position, target.position - ((target.forward * options.vehicleDistance) - target.up * 2) , 0.9f);
        Quaternion diff = camera.rotation * Quaternion.Inverse(target.rotation);
        camera.rotation = Quaternion.Lerp(camera.rotation, target.rotation, 0.9f);
    }

    private void FreeCamera()
    {
        axis = character.LookAxis * (options.sensitivity * 100 * Time.deltaTime);
        var tPos = target.position;
        camera.RotateAround(tPos, Vector3.up, axis.x);
        currentPitch += -axis.y;
        currentPitch = Mathf.Clamp(currentPitch, -options.maxPitch, options.maxPitch);
        var cameraRot = camera.transform.rotation;
        camera.rotation = Quaternion.Lerp(camera.rotation, Quaternion.Euler(currentPitch, cameraRot.eulerAngles.y, cameraRot.eulerAngles.z),0.1f);
        Vector3 nextPosition = tPos - (camera.forward * options.distance) + new Vector3(0,2,0) ;
        camera.position = nextPosition;
    }


}
