using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BonusBox : MonoBehaviour
{
    private bool isTaken;
    [SerializeField] private List<GameObject> vehicles = new();
    
    private List<Action<PhysicsVehicle>> bonusList = new()
    {
        Projectile, Heal, Shield
    };

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

    private static GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        isTaken = false;
        projectilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/projectile.prefab");
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
        if (!other.CompareTag("Player")) return;
        isTaken = true;
        PhysicCharacter character = other.GetComponent<PhysicCharacter>();
        Debug.Log("Giving bonus");
        if (character.activeVehicle)
        {
            character.activeVehicle.GetComponent<PhysicsVehicle>().bonus =
                bonusList[Random.Range(0, bonusList.Count)];
        }
        else
        {
            character.vehicle =
                vehicles[Random.Range(0, vehicles.Count - 1)];
        }
    }
    
    private static void Heal(PhysicsVehicle physicsVehicle)
    {
        Debug.Log("Healing");
        physicsVehicle.AddHealth(30);
    }

    private static void Shield(PhysicsVehicle physicsVehicle)
    {
        Debug.Log("Shield");
        physicsVehicle.SetResistance(1);
    }

    private static void Projectile(PhysicsVehicle physicsVehicle)
    {
        var forward = physicsVehicle.transform.forward;
        Vector3 position = physicsVehicle.gameObject.transform.position + forward;
        GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddForce(forward * (Time.deltaTime * 1000000f));

    }

    
}
