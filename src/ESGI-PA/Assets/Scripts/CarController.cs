/*
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public struct CarStats
{
    public float acceleration;
    public int backAcceleration;
    public float turnStrength;
    public float driftStrength;
    public float weight;
    public int boostMultiplier;
    public float boostDuration;
    public GameObject projectilePrefab; 
    public float projectileVelocity; 
    public float projectileCooldown; 
    public string[] bonus; 
    public Transform vehicle;
    public int turn;
}

public class CarController : MonoBehaviour
{
    private const int SPEED_MULTIPLIER = 100000;
    private const int ROTATE_MULTIPLIER = 100;
    private bool isShieldActivated = false;
    private Coroutine shieldCoroutine;

    [SerializeField] private CarStats Stats;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Rigidbody rigidbody;
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;

    private Vector2 input = Vector2.zero;
    private bool isGrounded;
    private bool canRotateInAir;
    private RaycastHit _ground;
    private bool isDrifting;
    private float carSmooth = 0.75f;
    private bool usingBonus = false;

    private Ray[] disabledRays;
    public GameObject playerPrefab;

    private event Action<CarStats> useBonus;
    /*private bool isBoosting;
    private float boostTimer;
    private float projectileTimer;
	private string bonusValue;#1#

    private float speed;
    void Start()
    {
        // CameraBehavior.inCar = true;
        currentHealth = maxHealth;
        Stats.turn = 0;
        //healthBar.maxValue = maxHealth;
        //healthBar.value = currentHealth;
    }

    private void Awake()
    {
        _input.SwitchCurrentActionMap("Vehicle");
    }

    void Update()
    {
        input = _input.actions["Movement"].ReadValue<Vector2>();
            isDrifting = _input.actions["Drift"].IsPressed();
            usingBonus = _input.actions["Bonus"].IsPressed();
            speed = input.y switch
            {
                >0 => Stats.acceleration,
                <0 => -Stats.backAcceleration,
                _ => 0
            };
            if (usingBonus && useBonus != null)
            {
                useBonus(Stats);
                useBonus = null;
            }
    }

    private void FixedUpdate()
    {
            rigidbody.AddForce(Vector3.down * Stats.weight, ForceMode.Acceleration);
        isGrounded = Physics.Raycast(new Ray(transform.position + new Vector3(0,0,0.5f), -Stats.vehicle.up), out _ground, 2f);
        canRotateInAir = Physics.Raycast(new Ray(transform.position , -Stats.vehicle.up), out _, 10f);
        
        if (isGrounded)
        {
            //int boostMultiplier = isBoosting ? Stats.boostMultiplier : 1;
            rigidbody.AddForce(Stats.vehicle.forward * (speed * Time.deltaTime * SPEED_MULTIPLIER));
            //rigidbody.AddRelativeForce(new Vector3(0,0 , playerOutput.y * SPEED_MULTIPLIER * 
             //                                            ((playerOutput.y > 0) ? Stats.acceleration : Stats.backAcceleration) * Time.deltaTime * boostMultiplier));
            transform.Rotate(Vector3.up * (ROTATE_MULTIPLIER * input.x * Stats.turnStrength * Time.deltaTime * (isDrifting?Stats.driftStrength:1) * Math.Min(rigidbody.velocity.magnitude, 1)));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, _ground.normal) * transform.rotation, carSmooth);
            if (input == Vector2.zero) rigidbody.angularVelocity = Vector3.zero;
        }
        else if (!canRotateInAir)
        {
            carSmooth = 0.25f;
            transform.Rotate(-Vector3.forward * (input.x * 2));
            transform.Rotate(-Vector3.right * (input.y * 2));
            rigidbody.angularVelocity = Vector3.zero;
        }
        else
        {
            if (!isGrounded && Mathf.Abs(rigidbody.velocity.y) <= 0.001)
            {
                transform.rotation = Quaternion.Euler(0,transform.rotation.y,0);
            }
        }
    }

    private IEnumerator ResetCarSmooth()
    {
        yield return new WaitForSeconds(0.5f);
        carSmooth = 0.75f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bonus"))
        {
            other.GetComponent<BonusBox>().IsTaken = true;
            Debug.Log("Is a bonus : True" );
            useBonus += Bonus.Instance.getBonus();
        }
        else if (other.CompareTag("Projectile"))
        {
            if (isShieldActivated)
            {
                isShieldActivated = false;
                StopCoroutine(shieldCoroutine);
                Destroy(other.gameObject);
                return; 
            }
            Debug.Log("Hit by projectile hjdhzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
            TakeDamage(100f);
        }
    }	
    public void StartBoost(float duration)
    {
        StartCoroutine(Boost(duration));
    }
    

    private IEnumerator Boost(float duration)
    {
        float originalAcceleration = Stats.acceleration;
        float originalTurnStrength = Stats.turnStrength;
        Stats.acceleration *= 2;
        Stats.turnStrength *= 2;
        yield return new WaitForSeconds(duration);
        Stats.acceleration = originalAcceleration;
        Stats.turnStrength = originalTurnStrength;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //healthBar.value = currentHealth;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die(){
        Instantiate(playerPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    
    public void ActivateShield(float duration)
    {
        if (isShieldActivated) return; // already activated
        isShieldActivated = true;
        shieldCoroutine = StartCoroutine(Shield(duration));
    }

public void Jump(float jumpHeight, float jumpDuration)
    {
        StartCoroutine(JumpTime(jumpHeight, jumpDuration));
    }
    private IEnumerator JumpTime(float jumpHeight, float jumpDuration)
    {
        // Désactiver la gravité pour que le véhicule ne tombe pas pendant le saut
        Stats.vehicle.GetComponent<Rigidbody>().useGravity = false;

        // Calculer la position de saut et la position d'arrivée
        Vector3 startPos = Stats.vehicle.position;
        Vector3 jumpPos = startPos + Vector3.up * jumpHeight;
        Vector3 endPos = startPos;

        // Animer le saut
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / jumpDuration;
            Stats.vehicle.position = Vector3.Lerp(jumpPos, endPos, t);
            yield return null;
        }

        // Réactiver la gravité
        Stats.vehicle.GetComponent<Rigidbody>().useGravity = true;
    }


    
    
    private IEnumerator Shield(float duration)
    {
        Debug.Log("Shield activated");

        // Créer une sphère visuelle avec un collider pour représenter le bouclier
        GameObject shieldVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        shieldVisual.transform.localScale = new Vector3(4f, 4f, 4f);
        Renderer shieldRenderer = shieldVisual.GetComponent<Renderer>();
        Color shieldColor = new Color(0f, 0f, 1f, 0.5f); // Bleu transparent
        shieldRenderer.material.color = shieldColor;

        // Ajouter un collider au bouclier visuel
        SphereCollider shieldCollider = shieldVisual.AddComponent<SphereCollider>();
        shieldCollider.isTrigger = true;

        // Fixer la position de la sphère visuelle à la voiture
        shieldVisual.transform.SetParent(Stats.vehicle);
        shieldVisual.transform.localPosition = Vector3.zero;
        shieldVisual.transform.localRotation = Quaternion.identity;

        float originalWeight = Stats.weight;
        Stats.weight /= 2;
        isShieldActivated = true;

        yield return new WaitForSeconds(duration);

        Destroy(shieldVisual);
        Stats.weight = originalWeight;
        isShieldActivated = false;

        Debug.Log("Shield deactivated");
    }

    public int Turn
    {
        get => Stats.turn;
        set
        {
            Stats.turn = value;
        }
    }
}
*/
