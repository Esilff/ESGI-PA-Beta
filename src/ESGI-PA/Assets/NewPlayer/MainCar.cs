using UnityEngine;
using UnityEngine.InputSystem;

public class MainCar : MonoBehaviour
{
    [SerializeField] Vector3 carOffset;
    public Vector3 CarOffset => carOffset;

    [SerializeField] float camSpeed;
    [SerializeField] Vector3 camOffset;
    public Vector3 CamOffset => camOffset;

    [SerializeField] CharacterController controller;
    [SerializeField] float accelerationForce, gravity, maxSpeed, frictionForce, rotForce, rotationLerp;

    [SerializeField] Vector3 normalAngle;

    void Start()
    {
        controller.Move(Vector3.down);
    }

    public void CameraMovement(Camera cam)
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, transform.TransformPoint(camOffset), camSpeed * Time.deltaTime);

        cam.transform.LookAt(transform.position);
    }

    public void Move(PlayerInput input)
    {
        Vector3 movement = Vector3.zero;

        Vector2 move = input.actions["Move"].ReadValue<Vector2>();

        if (controller.isGrounded)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, normalAngle) * transform.rotation, rotationLerp * Time.deltaTime);
            float zSpeed = transform.InverseTransformDirection(controller.velocity).z;
            movement = transform.forward *
                (zSpeed * Time.deltaTime +
                 move.y * accelerationForce * (1 - zSpeed / maxSpeed) * Time.deltaTime - Mathf.Sign(zSpeed) * frictionForce * Time.deltaTime);

            if (Mathf.Abs(zSpeed) > 0.5f)
                transform.Rotate(transform.up, move.x * Mathf.Sign(zSpeed) * rotForce * Time.deltaTime);
        }
        else
        {
            movement = controller.velocity * Time.deltaTime;
        }
        
        movement.y -= gravity * Time.deltaTime;

        controller.Move(movement);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        normalAngle = hit.normal;
    }
}
