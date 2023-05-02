using System;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    [SerializeField] private Transform camera;

    [SerializeField] private Vector3 offset, position;

    [SerializeField] private float cameraSpeed, lerpSpeed;

    [SerializeField] private Transform target;
    // Start is called before the first frame update
    public static bool inCar;
    void Start()
    {
        camera.position = target.position + offset;
        camera.LookAt(target);
        camera.rotation = Quaternion.Slerp(camera.rotation, target.rotation, cameraSpeed);
        
    }


    /*private void FixedUpdate()
    {
        //if (Physics.Raycast(camera.position, -camera.up, 2f)) offset.y = 1;
    }*/
    private void FixedUpdate()
    {
        if (!inCar) MoveCamera();
    }

    private void Update()
    {
        if (inCar) MoveCamera();
    }

    private void MoveCamera()
    {
        position = Vector3.Lerp(position, target.position + (target.forward * offset.z) + Vector3.up * offset.y, lerpSpeed * Time.deltaTime);
        camera.rotation = Quaternion.Lerp(camera.rotation, Quaternion.LookRotation(target.position - camera.position), lerpSpeed * Time.deltaTime);
        
        if (Physics.Raycast(target.position, position - target.position,
                out RaycastHit hit, Vector3.Distance(position, target.position)))
            camera.position = Vector3.Lerp(hit.point, target.position, 0.2f);
        else
            camera.position = position;
    }

    private void FreeCamera()
    {
        
    }
}
