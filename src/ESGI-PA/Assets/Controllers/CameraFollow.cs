using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] Transform target, rotatorX;
    [SerializeField] float sensi, rotX, maxX;

    void Start()
    {
        transform.position = target.position;
    }

    void Update()
    {
        transform.position = target.position;
        
        Vector2 look = input.actions["Look"].ReadValue<Vector2>();
        
        transform.Rotate(Vector3.up, look.x * sensi * Time.deltaTime);

        rotX -= look.y * sensi * Time.deltaTime;

        if (rotX > maxX) rotX = maxX;
        else if (rotX < -maxX) rotX = -maxX;
        
        rotatorX.localRotation = Quaternion.Euler(rotX, 0, 0);
    }
}