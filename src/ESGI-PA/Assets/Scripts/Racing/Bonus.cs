using System;
using System.Collections.Generic;
using UnityEngine;


public class Bonus : MonoBehaviour
{
    [SerializeField] private List<GameObject> vehicles = new();

    private List<Action<PhysicsVehicle>> bonusList = new()
    {
        
    };

  

    /*private static Bonus instance = null;
    
    private Bonus(){}

    public static Bonus Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Bonus();
            }
            return instance;
        }
    }

    private Action<CarStats>[] bonusList = new Action<CarStats>[]
    {
        projectile,
        jump,
        shield,
        boost

    };

    private static void boost(CarStats stats)
    {
        Debug.Log("Im a booster");

        // Activer le boost pendant boostDuration secondes
        float boostDuration = 1.5f;
        float boostMultiplier = 1.2f;
        stats.boostMultiplier *= (int)boostMultiplier;
        stats.vehicle.GetComponent<CarController>().StartBoost(boostDuration);
    }

    private static void projectile(CarStats stats)
    {
        Debug.Log("Im a projectile");
        if (stats.projectilePrefab != null)
        {
            GameObject projectile = GameObject.Instantiate(stats.projectilePrefab,
                stats.vehicle.position + new Vector3(0, 2, 5), Quaternion.identity);
            //projeter le projectile
            projectile.GetComponent<Rigidbody>().AddForce(stats.vehicle.forward * 1000f);
            

        }
    }
    
    private static void jump(CarStats stats)
    {
        Debug.Log("I'm a jump bonus");
        if (stats.vehicle != null)
        {
            // Activer le saut
            float jumpForce = 1000f;
            stats.vehicle.GetComponent<CarController>().Jump(100f, 0.1f);
        }
    }

    private static void shield(CarStats stats)
    {
        Debug.Log("I'm a shield bonus");
        if (stats.vehicle != null)
        {
            // Activer le bouclier
            float shieldDuration = 5f;
            stats.vehicle.GetComponent<CarController>().ActivateShield(shieldDuration);
        }
    }

    public Action<CarStats> getBonus()
    {
        return bonusList[UnityEngine.Random.Range(0, bonusList.Length)];
    }*/
}