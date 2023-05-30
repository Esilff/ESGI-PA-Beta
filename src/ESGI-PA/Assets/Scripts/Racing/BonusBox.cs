using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BonusBox : MonoBehaviour
{
    private bool isTaken;
    [SerializeField] private List<GameObject> vehicles = new();
    

    public bool IsTaken
    {
        get
        {
            return isTaken;
        }
        set
        {
            isTaken = value;
        }
    }

    private bool disabled = false;

    [SerializeField] private Collider collider;

    [SerializeField] private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        isTaken = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (disabled) return;
        if (isTaken) StartCoroutine(disable());
    }

    private IEnumerator disable()
    {
        disabled = true;
        mesh.enabled = false;
        collider.enabled = false;
        yield return new WaitForSeconds(3);
        disabled = false;
        isTaken = false;
        mesh.enabled = true;
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (!other.CompareTag("Player")) return;
        // isTaken = true;
        // GameObject vehicle = other.GetComponent<PhysicCharacter>().vehicle = new GameObject();
        // other.GetComponent<PhysicCharacter>().vehicle =
        //     vehicles[Random.Range(0, vehicles.Count)]; //replace with random vehicle
    }
}
