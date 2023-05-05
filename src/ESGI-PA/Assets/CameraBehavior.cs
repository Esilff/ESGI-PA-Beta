using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    private Vector2 axis;
    private Vector3 velocity;
    private float currentPitch;
    private RaycastHit info;
    private float defaultDistance;
    void Start()
    {
        camera.position = target.position + new Vector3(0, 2, -options.distance);
        currentPitch = camera.transform.rotation.eulerAngles.x;
        defaultDistance = options.distance;
    }

    void Update()
    {
        axis = input.actions["Look"].ReadValue<Vector2>() * (options.sensitivity * 100 * Time.deltaTime);
        camera.RotateAround(target.position, Vector3.up, axis.x);
        currentPitch += -axis.y;
        currentPitch = Mathf.Clamp(currentPitch, -options.maxPitch, options.maxPitch);
        var cameraRot = camera.transform.rotation;
        camera.transform.rotation = Quaternion.Euler(currentPitch, cameraRot.eulerAngles.y, cameraRot.eulerAngles.z);
        Vector3 nextPosition = target.position - (camera.forward * options.distance) + new Vector3(0,2,0) ;
        camera.position = Vector3.Lerp(camera.position, nextPosition, Time.deltaTime * options.speed); 
    }

    private void FixedUpdate()
    {
        float detectionRadius = options.distance;

        // Cast a sphere in front of the camera to detect obstacles
        if (Physics.SphereCast(camera.position, options.detectionRadius, camera.forward, out RaycastHit hit, options.detectionRadius, LayerMask.GetMask("Default")))
        {
            // Adjust the camera distance based on the distance to the obstacle
            options.distance = hit.distance + 0.5f;
        }
        else
        {
            // Reset the camera distance to the default value if there are no obstacles
            options.distance = defaultDistance;
        }
    }
}
