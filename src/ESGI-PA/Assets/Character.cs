using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[Serializable]
struct CharacterComponents
{
    public Transform character;
    public Transform camera;
    public CharacterController controller;
    public PlayerInput input;
    public Animator animator;
}

[Serializable]
struct CharacterStats
{
    public int weight;
    [Range(10,25)]
    public int baseSpeed;

    public int jumpPower;
}

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterComponents components;
    [SerializeField] private CharacterStats stats;

    private Vector2 _axis;
    private RaycastHit _groundInfo;
    private bool _isGrounded;

    private Vector3 movement;

    private float turnSmoothVelocity;
    private float verticalVelocity;
    private float jumpTime = 0;

    private void Start()
    {
        verticalVelocity = stats.weight;
    }

    void Update()
    {
        _axis = components.input.actions["Move"].ReadValue<Vector2>();
        if (!components.controller.isGrounded)
        {
            verticalVelocity -= 0.1f;
        } else if (components.input.actions["Jump"].IsPressed())
        {
            verticalVelocity += (verticalVelocity < stats.jumpPower ? stats.jumpPower / 10f : 0);
        }
        Move(_axis.x, _axis.y);
        
        
    }
    
    private void Move(float x, float y)
    {
        /*float targetAngle = 0;
        if (movement.magnitude >= 0.1f)
        {
            Debug.Log("Running");
            components.animator.SetFloat("Running Threshold", 1);
            targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + components.camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized
                                    * (stats.baseSpeed * (!_isGrounded ? 2f : 1));
            components.controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            components.animator.SetFloat("Running Threshold", 0);
        }*/
        
    }
    /*private void FixedUpdate()
    {
        _isGrounded = components.controller.isGrounded;
        // Debug.DrawRay(components.character.position + new Vector3(0,0.5f,0), -components.character.up * 5, Color.red, 10000f);
        // _isGrounded = Physics.SphereCast(new Ray(components.character.position + new Vector3(0,0.8f,0), -components.character.up), 1f);
        Debug.Log(_isGrounded);
    }

    private void Move(float x, float y)
    {
        float targetAngle = 0;
        if (movement.magnitude >= 0.1f)
        {
            Debug.Log("Running");
            components.animator.SetFloat("Running Threshold", 1);
            targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + components.camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized
                                    * (stats.baseSpeed * (!_isGrounded ? 2f : 1));
            components.controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            components.animator.SetFloat("Running Threshold", 0);
        }
        
    }

    private void Jump()
    {
        Debug.Log("Jumping");
        verticalVelocity = (-1.5f * Mathf.Pow(10, 2) + 2.5f * 10) * stats.jumpPower * Time.deltaTime * 10f;
        components.controller.Move(new Vector3(0, -verticalVelocity, 0));
        Debug.Log($"Vertical velocity : { verticalVelocity}");
        //components.controller.Move(new Vector3(0,verticalVelocity,0));
    }

    private void CheckGround()
    {
        _isGrounded = components.controller.isGrounded;
    }*/
}
