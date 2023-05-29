using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Transform obj = other.transform;
        obj.position = other.GetComponent<PhysicsVehicle>().lastCheckpoint.transform.position;
        obj.rotation = Quaternion.identity;

    }
}
