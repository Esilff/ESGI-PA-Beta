/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IA : MonoBehaviour
{
    public List<GameObject> checkpoints;
    public float speed = 5.0f;
    private GameObject currentCheckpoint;
    private GameObject nextCheckpoint;
    public GameObject player;
    private const int SPEED_MULTIPLIER = 100000;
    private const int ROTATE_MULTIPLIER = 100;
    [SerializeField] private CarStats Stats;
    private bool isGrounded;
    private RaycastHit ground;
    [SerializeField] private Rigidbody rb;
    private Vector2 playerOutput = Vector2.zero;
    public int angle = 70;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpoints[i].SetActive(false);
        }
        checkpoints[0].SetActive(true);
        currentCheckpoint = checkpoints[0];
        nextCheckpoint = checkpoints[1];
    }

    void Update()
    {

        //utilser un raycast pour que l'ia soit toujours perpendiculaire au sol et si il ne l'est pas le faire tourner
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        Debug.DrawRay(transform.position + new Vector3(0,0,0.5f), -transform.up, Color.red, 10f);
        // Calculate the distance between the AI and the current checkpoint
        float distance = Vector3.Distance(transform.position, currentCheckpoint.transform.position);

        // Check if the AI has reached the current checkpoint
        if (currentCheckpoint.GetComponent<checkpoint>().passed == true)
        {
            // If so, update the current and next checkpoints
            currentCheckpoint.SetActive(false);
            nextCheckpoint.SetActive(true);
            currentCheckpoint = nextCheckpoint;

            if (currentCheckpoint.GetComponent<checkpoint>().passed == true && currentCheckpoint != checkpoints[checkpoints.Count - 1])
            {
                currentCheckpoint.SetActive(false);
                currentCheckpoint = nextCheckpoint;
                currentCheckpoint.SetActive(true);
                nextCheckpoint = checkpoints[checkpoints.IndexOf(currentCheckpoint) + 1];
            }
            if (currentCheckpoint.GetComponent<checkpoint>().passed == true && currentCheckpoint == checkpoints[checkpoints.Count - 1])
            {
                Debug.Log("victoire");
            }
        }
        else
        {
            //va vers le checkpoint suivant  et ajoute une valeur random de maximum 5 a la position du checkpoint suivant
            //recupere les coordonnées du checkpoint suivant
            Vector3 target = currentCheckpoint.transform.position;
            //cree 2 variables pour stocker les coordonnées du checkpoint suivant
            float x = target.x;
            float z = target.z;
            float y = target.y;
            //fait aller l'ia vers le checkpoint suivant avec les coordonnées x et z
            
            //si y est superieur a la position en y de l'ia alors il va tout droit en x et z
            if (y > transform.position.y +1)
            {
                //move the car forward
                transform.Translate(Vector3.forward * Time.deltaTime * (speed - 0.3f) );
                //ajoute un angle de 20 degres au transform de l'ia
                transform.Rotate(Vector3.up * Time.deltaTime * 10);
                Debug.Log("y > transform.position.y +1");
            }
            else if(y < transform.position.y -1)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * (speed+0.8f));
                //ajoute un angle de 20 degres au transform de l'ia
                transform.Rotate(Vector3.up * Time.deltaTime * -10);
                Debug.Log("y < transform.position.y -1");
            }
            else
            {
                //rotation lente vers le checkpoint suivant qui fait prendre un virage assez large pour eviter les obstacles
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 0.5f);
                //transform.Translate(Vector3.forward * Time.deltaTime * speed);

                //oriente l'ia vers le checkpoint suivant
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(x, transform.position.y, z)), Time.deltaTime * 2);
                Debug.Log("y = transform.position.y");
                //fait tourner doucement le lookat de l'ia
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, transform.position.y, z), speed * Time.deltaTime);
                //faire en sorte que le transform suive le lookat
                transform.rotation = Quaternion.LookRotation(new Vector3(x, transform.position.y, z));
                
            }



        }

            
            /*
            // Calculate the direction the AI should move in
            Vector3 direction = (currentCheckpoint.transform.position - transform.position).normalized;

            // Check if the current checkpoint is above or below the AI
            if (currentCheckpoint.transform.position.y > transform.position.y)
            {
                // If above, move in the direction of the checkpoint in the x and z axes only
                transform.position = transform.position + new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime;
            }
            else
            {
                // If below, move in the direction of the checkpoint
                transform.position = transform.position + direction * speed * Time.deltaTime;
            }#1#
            
    }
    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, 1f);
        Physics.Raycast(new Ray(transform.position + new Vector3(0,0,0.5f), -transform.up), out ground, 1f);
        if (isGrounded)
        {
            rb.useGravity = false;
            rb.AddRelativeForce(new Vector3(0,0 , playerOutput.y * SPEED_MULTIPLIER * Stats.acceleration * Time.deltaTime));
            transform.Rotate(Vector3.up * (ROTATE_MULTIPLIER * playerOutput.x * Stats.turnStrength * Time.deltaTime * Math.Min(rb.velocity.magnitude, 1)));
            transform.rotation = Quaternion.FromToRotation(transform.up, ground.normal) * transform.rotation;
            if (playerOutput == Vector2.zero) rb.angularVelocity = Vector3.zero;
            Debug.Log("isGrounded");
        }
        else
        {
            Debug.Log("isGrounded = false");
            rb.useGravity = true;
            transform.Rotate(-Vector3.forward * playerOutput.x);
            transform.Rotate(-Vector3.right * playerOutput.y);
            rb.angularVelocity = Vector3.zero;
        }
    }
}
*/
