using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
struct CharacterStats
{
    public int weight;
    [Range(1,15)]
    public int baseSpeed;
}

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] private Transform character;
    [SerializeField] private Transform character;
    [SerializeField] private PlayerInput input;
    // [SerializeField] private CharacterController controller;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private Transform camera;
    
    [SerializeField] private CharacterController controller;
    //private float gravity = -Mathf.Pow(Time.deltaTime, 2) + Mathf.Sqrt(Time.deltaTime * 2);
    private Vector2 axis;
    private Vector3 cameraDirection;

    private bool isGrounded;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        axis = input.actions["Move"].ReadValue<Vector2>();
        cameraDirection = camera.rotation.eulerAngles;
        //gravity = -Mathf.Pow(Time.deltaTime, 2) + Mathf.Sqrt(Time.deltaTime * 2);
        //Move(axis.y, axis.x);

    }

    private void FixedUpdate()
    {
        // isGrounded = Physics.Raycast(new Ray(character.position, Vector3.down), 1f, LayerMask.GetMask("Default"));
        Move(axis.x, axis.y);
    }

    private void Move(float x, float y)
    {
        controller.Move(new Vector3(x * stats.baseSpeed * camera.forward.x, -5, y * stats.baseSpeed * camera.forward.y) * Time.deltaTime);
    }
}
