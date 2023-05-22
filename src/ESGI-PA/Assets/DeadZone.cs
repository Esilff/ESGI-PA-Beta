using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public Vector3 respawn;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Transform obj = other.transform;
        obj.position = respawn;
        obj.rotation = Quaternion.identity;

    }
}
