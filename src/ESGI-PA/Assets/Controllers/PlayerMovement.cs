using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] Rigidbody rb;
    [SerializeField] float forceMove, angleSpeed, jumpForce, groundCheck;

    [SerializeField] bool isJump, isGrounded;
    [SerializeField] private bool hasControl = true;

    [SerializeField] private float weight;

    [SerializeField] private GameObject car;

    private bool canDash = true;
    
    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * weight, ForceMode.Acceleration);
        isGrounded = Physics.Raycast(new Ray(transform.position, -transform.up), 1.2f);
        Debug.DrawRay(transform.position, -transform.up, Color.red, 300f);
        if (isJump)
        {
            isJump = false;
            
            rb.AddForce(Vector3.up * jumpForce);
        }



        Vector2 move = input.actions["Move"].ReadValue<Vector2>();


        if (move.magnitude == 0) return;
        
        float target = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

        transform.rotation = Quaternion.Lerp(transform.rotation, 
            Quaternion.Euler(0, target, 0), angleSpeed * Time.deltaTime);
        
        if (isGrounded) rb.AddForce(Quaternion.Euler(0, target, 0) * Vector3.forward * forceMove);
        input.actions["Dash"].started += Dash;
    }

    void Update()
    {
        if (input.actions["Jump"].triggered && isGrounded)
        {
            isJump = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bonus"))
        {
            Destroy(gameObject);
            other.GetComponent<BonusBox>().IsTaken = true;
            GameObject newController = Instantiate(car, transform.position + Vector3.forward * 10 + Vector3.up, Quaternion.identity);
        }
    }

    void Dash(InputAction.CallbackContext context)
    {
        if (!isGrounded || !canDash) return;
        Vector2 move = input.actions["Move"].ReadValue<Vector2>();
        float target = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        rb.AddForce(Quaternion.Euler(0, target, 0) * Vector3.forward * forceMove, ForceMode.VelocityChange);
        StartCoroutine(dashCooldown());
    }

    IEnumerator dashCooldown()
    {
        canDash = false;
        Debug.Log("Dash in cooldown");
        yield return new WaitForSeconds(5);
        canDash = true;
        Debug.Log("Dash recovered");
    }
}