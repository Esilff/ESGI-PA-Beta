using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerInput input;
    [SerializeField] private CharacterController controller;
    
    private Vector2 axis;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        axis = input.actions["Move"].ReadValue<Vector2>();
        Move(axis.y, axis.x);
    }

    private void Move(float x, float y)
    {
        controller.Move(new Vector3(x, 0, y) * Time.deltaTime);
    }
}
