using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Player : MonoBehaviour
{
    /*[SerializeField] private PlayerInput input;
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerStats stats;
    
    private bool _isIAControlled = false;

    private Vector2 movementAxis = Vector2.zero;
    private bool isJumping = false;

    private Vector3 movement = Vector3.zero;
    private bool canJump;
    public bool IsIAControlled
    {
        get => _isIAControlled;
        set
        {
            _isIAControlled = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // input.actions["Jump"].started += jump();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_isIAControlled) return;
        ReadInput();
    }

   

    private void FixedUpdate()
    {
        if (_isIAControlled) return;
        Behave();
        controller.Move(movement * Time.deltaTime);
    }

    private void ReadInput()
    {
        movementAxis = input.actions["Move"].ReadValue<Vector2>();
        isJumping = input.actions["Jump"].IsPressed();
    }

    private void Behave()
    {
        if (isJumping)
        {
            Jump();
            
        } else if (!controller.isGrounded)
        {
            movement.y = -stats.weight * 10;
        }
    }

    private void Jump()
    {
        movement.y = Mathf.Lerp(0,stats.jumpStrength * 10, 1.0f);
    }
    /*private void FixedUpdate()
    {
        controller.Move(new Vector3(0, -0.1f, 0));
    }#1#
    
    //A player should do specific actions, move, turn, jump and dash*/
    
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Camera cam;
    [SerializeField] float cameraOffset, sensi, rotateSensi;
    [SerializeField] Vector2 rotation;
    
    [SerializeField] CharacterController controller;
    [SerializeField] float speed, gravityForce, jumpForce;

    bool jump;

    [SerializeField] MainCar car;
    
    void Update()
    {
        if (car)
        {
            if (playerInput.actions["Interact"].triggered)
            {
                controller.detectCollisions = true;
                transform.parent = null;
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
                car = null;
                return;
            }

            return;
        }
        
        Vector2 look = playerInput.actions["Look"].ReadValue<Vector2>();
        Debug.Log(look);

        rotation += sensi * Time.deltaTime * look;

        rotation.y = rotation.y > Mathf.PI * 0.9f ? Mathf.PI * 0.9f : rotation.y < Mathf.PI * 0.1f ? Mathf.PI * 0.1f : rotation.y;
        
        Vector3 cameraposition = transform.position + 
                                 new Vector3(Mathf.Sin(rotation.x) * cameraOffset * Mathf.Sin(rotation.y), 
                                     Mathf.Cos(rotation.y) * cameraOffset, 
                                     Mathf.Cos(rotation.x) * cameraOffset * Mathf.Sin(rotation.y));

        cam.transform.position = cameraposition;
        cam.transform.rotation = Quaternion.LookRotation(transform.position - cameraposition);

        if (playerInput.actions["Jump"].triggered && controller.isGrounded)
            jump = true;

        /*if (playerInput.actions["Interact"].triggered)
        {
            foreach (Collider c in Physics.OverlapSphere(transform.position, 2f))
            {
                if (car = c.GetComponent<MainCar>())
                {
                    controller.detectCollisions = false;
                    transform.position = car.transform.TransformPoint(car.CarOffset);
                    transform.rotation = car.transform.rotation;
                    transform.parent = car.transform;
                    cam.transform.position = transform.TransformPoint(car.CamOffset);
                    cam.transform.LookAt(transform.position);
                    break;
                }
            }
        }*/
    }

    void FixedUpdate()
    {
        if (car)
        {
            car.Move(playerInput);

            car.CameraMovement(cam);
            return;
        }
        
        Vector3 movement = Vector3.zero;

        if (jump)
        {
            jump = false;
            movement.y = jumpForce;
        }
        else if (controller.isGrounded)
        {
            movement.y = -1f;
        }
        else
        {
            movement.y = controller.velocity.y * Time.deltaTime - gravityForce * Time.deltaTime;
        }
        
        Vector2 move = playerInput.actions["Move"].ReadValue<Vector2>();

        if (move.sqrMagnitude != 0)
            movement += speed * Time.deltaTime * (Quaternion.Euler(0f, Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y, 0f) * Vector3.forward);

        controller.Move(movement);

        Vector3 velocity = new(controller.velocity.x, 0f, controller.velocity.z);
        if (velocity.sqrMagnitude != 0)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), rotateSensi * Time.deltaTime);
    }
}
