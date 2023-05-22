
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PhysicsVehicle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform vehicle;
    [SerializeField] private Rigidbody body;
    [SerializeField] private PlayerInput input;

    [Range(1, 15)]
    [SerializeField] private float speed = 1;

    [Range(1, 25)] 
    [SerializeField] private int weight = 1;

    [Range(1,10)][SerializeField] private int rotationSpeed = 1;
    [SerializeField] private float loopCap = 1f;
    
    private Vector2 _axis;
    private bool _isDrifting;

    private bool _isGrounded;
    private RaycastHit _groundInfo;

    private float _lastDriftDirection;
    private bool _driftHold;

    private Vector2 _defaultAxis;
    void Update()
    {
        _defaultAxis = input.actions["Movement"].ReadValue<Vector2>();
        _axis = input.actions["Movement"].ReadValue<Vector2>() * (Time.deltaTime * 50000f);
        _isDrifting = input.actions["Drift"].IsPressed();
        Debug.Log("Default axis : " + _defaultAxis);
    }

    private void FixedUpdate()
    {
        CheckConstraints();
        //vehicle.up = groundInfo.normal;
        Gravity();
        if (_isDrifting)
        {
            Drift();
        }
        else
        {
            Move();
        }
    }

    private void Move()
    {
        if (!_isGrounded) return;
        Vector3 force = (vehicle.forward * (_defaultAxis.y * 45 * speed));
        // Debug.Log("Force : " + force);
        //force += groundInfo.normal;
        _lastDriftDirection = 0;
        if (_axis != Vector2.zero)
        {
            vehicle.rotation *= Quaternion.Euler(new Vector3(0,rotationSpeed * 0.1f,0) * (_defaultAxis.x * 45));
        }
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, _ground.normal) * transform.rotation, carSmooth);
        var rotation = vehicle.rotation;
        transform.rotation = Quaternion.Lerp(rotation,
            Quaternion.FromToRotation(vehicle.up, _groundInfo.normal) * rotation, 0.5f);
        // vehicle.up = groundInfo.normal;
       
            // vehicle.rotation *= Quaternion.Euler(groundInfo.normal);
        
        body.AddForce(force, ForceMode.Acceleration);
    }

    private void Drift()
    {
        if (!_isGrounded || _axis.x == 0)
        {
            _driftHold = false;
            return;
        }

        vehicle.rotation *= Quaternion.Euler(new Vector3(0,rotationSpeed * 0.1f,0) * (_defaultAxis.x * 25));
        var rotation = vehicle.rotation;
        transform.rotation = Quaternion.Lerp(rotation,
            Quaternion.FromToRotation(vehicle.up, _groundInfo.normal) * rotation, 0.5f);
    

        Vector3 force = (vehicle.forward * (speed * 25)) + (vehicle.right * ((Math.Abs(_defaultAxis.x - (-1)) < 0.4f ? _defaultAxis.y : -_defaultAxis.y) * speed * 30));
        body.AddForce(force);
    }

    private void CheckConstraints()
    {
        _isGrounded = Physics.Raycast(new Ray(vehicle.position, -vehicle.up), out _groundInfo, 1.5f);
        if (Mathf.Abs(body.velocity.magnitude) > loopCap && _isGrounded)
        {
            body.useGravity = false;
        }
        else
        {
            body.useGravity = true;
        }
    }

    private void Gravity()
    {
        if (!_isGrounded)
        {
            body.AddForce(0,-weight * Time.deltaTime * 1000f,0);
        }
    }
}
