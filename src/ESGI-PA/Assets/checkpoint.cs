using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public bool passed = false;
    public GameObject player;
    public bool lastCheckpoint = false;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        passed = true;
        Debug.Log("Checkpoint passed");
        if (lastCheckpoint)
        {
            other.gameObject.GetComponent<CarController>().Turn++;
            lastCheckpoint = false;
        }
    }
    public void ResetCheckpoint()
    {
        passed = false;
    }
    public bool IsPassed()
    {
        return passed;
    }

    public bool IsLast
    {
        get => lastCheckpoint;
        set => lastCheckpoint = value;
    }
}


