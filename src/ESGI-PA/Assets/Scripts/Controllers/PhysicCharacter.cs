using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class PhysicCharacter : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Rigidbody body;

    [SerializeField] private PlayerInput input;
    [SerializeField] private Transform camera;
    [SerializeField] private CameraBehavior cameraScript;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject model;
    
    private Vector2 _axis;
    private bool shouldJump;
    private bool canJump = true;
    private bool canWallJump = false;

    private int _runAnim;
    
    private bool canDash = true;
    private bool dashing = false;
    private bool isRunning = false;
    private bool useBonus = false;
    private bool _shouldExit = false;
    

    public GameObject vehicle;
    private GameObject activeVehicle;
    private bool onVehicle = false;

        [SerializeField] private float turnSmoothTime = 0.1f;

    [SerializeField] private int speed = 1;

    [SerializeField] private int jumpForce = 1;

    [SerializeField] private float gravity = 1;

    [SerializeField] private float dashSpeed;

    [Range(1,2)]
    [SerializeField] private float runMultiplier = 1f;

    private RaycastHit leftWallInfo;
    private RaycastHit rightWallInfo;

    private bool leftWallHit;
    private bool rightWallHit;

    private bool stillOnWall = false;

    private bool isIAControlled = false;
    
    // Start is called before the first frame update
    void Start()
    {
        input.defaultActionMap = "Character";
        _runAnim = Animator.StringToHash("Running Threshold");
    }

    // Update is called once per frame
    void Update()
    {
        ReadInput();
        
    }

    private void FixedUpdate()
    {
        if (onVehicle)
        {
            if (_shouldExit)
            {
                DestroyVehicle();
                return;
            }
            character.position = activeVehicle.transform.position;
            character.rotation = activeVehicle.transform.rotation;
        }
        else
        {
            DefaultState();
        }
    }

    private void ReadInput()
    {
        if (onVehicle)
        {
            _shouldExit = input.actions["Escape"].IsPressed();
            return;
        }
        _axis = input.actions["Move"].ReadValue<Vector2>() * (Time.deltaTime * 5000f);
        shouldJump = input.actions["Jump"].IsPressed();
        dashing = input.actions["Dash"].IsPressed();
        isRunning = input.actions["Run"].IsPressed();
        useBonus = input.actions["Bonus"].IsPressed();
    }
    
    private void DefaultState()
    {
        if (useBonus) InvokeVehicle();
        CheckWalls();
        Gravity();
        Move();
        if (dashing) StartCoroutine(Dash());
        StartCoroutine(Jump());
        canJump = Physics.Raycast(new Ray(character.position, -character.up), 1.5f);
    }

    private void Move()
    {
        var forward = isIAControlled ? Vector3.forward : camera.forward;
        var right = isIAControlled ? Vector3.right : camera.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        var forceDirection = (forward * _axis.y) + (right * -_axis.x);
        body.AddForce(forceDirection * (speed * (!canJump ? 0.8f : 1) * (isRunning && canJump ? runMultiplier : 1))
            , ForceMode.Acceleration);
        if (forceDirection.magnitude > 0.2f)
        {
            animator.SetFloat(_runAnim, canJump ? 1 : 0);
            var targetRotation = Quaternion.LookRotation(forceDirection, Vector3.up);
            character.rotation = Quaternion.Lerp(character.rotation, targetRotation, turnSmoothTime); 
        }
        else
        {
            animator.SetFloat(_runAnim, 0);
        }
    }

    private IEnumerator Jump()
    {
        if (canJump && shouldJump)
        {
            body.AddForce(0,jumpForce * Time.deltaTime * 50f,0,ForceMode.Impulse);
            canJump = false;
        }
        else if (canWallJump && shouldJump)
        {
            body.AddForce(new Vector3(0,jumpForce * Time.deltaTime * 75f,0) + 
                          (leftWallHit ? leftWallInfo.normal : rightWallHit ?  rightWallInfo.normal : Vector3.zero) * (Time.deltaTime * 500f)
                ,ForceMode.Impulse);
            canWallJump = false;
            yield return new WaitForSeconds(0.5f);
            canWallJump = true;
        }
    }

    private void Gravity()
    {
        if (canJump) return;
        body.AddForce(Vector3.down * (body.mass * gravity * Time.deltaTime * 100));
    }

    private IEnumerator Dash()
    {
        if (!canDash) yield break;
        var forward = camera.forward;
        var right = camera.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        var forceDirection = (forward * _axis.y) + (right * -_axis.x);
        body.AddForce(forceDirection * (dashSpeed * (!canJump ? 0.8f : 1)), ForceMode.Impulse);
        if (forceDirection.magnitude > 0.5f)
        {
            if (canJump) animator.SetFloat(_runAnim, 1);
            else animator.SetFloat(_runAnim, 0);
            var targetRotation = Quaternion.LookRotation(forceDirection, Vector3.up);
            character.rotation = Quaternion.Lerp(character.rotation, targetRotation, turnSmoothTime); 
        }
        else
        {
            animator.SetFloat(_runAnim, 0);
        }
        canDash = false;
        yield return new WaitForSeconds(5f);
        canDash = true;
    }

    private void CheckWalls()
    {
        rightWallHit = Physics.Raycast(new Ray(character.position, camera.right), out rightWallInfo, 0.7f,
            LayerMask.GetMask("Wall"));
        leftWallHit = Physics.Raycast(new Ray(character.position, -camera.right), out leftWallInfo, 0.7f,
            LayerMask.GetMask("Wall"));
        canWallJump = rightWallHit || leftWallHit;
        if (rightWallHit) character.right = Vector3.Lerp(character.right, rightWallInfo.normal, 0.1f);
        if (leftWallHit) character.right = Vector3.Lerp(character.right, leftWallInfo.normal, 0.1f);;
    }

    private void InvokeVehicle()
    {
        if (!vehicle) return;
        activeVehicle = vehicle;
        GameObject newVehicle = Instantiate(vehicle, character.position, character.rotation);
        newVehicle.GetComponent<PhysicsVehicle>().Character = this;
        character.parent.parent = newVehicle.transform;
        vehicle = null;
        model.SetActive(false);
        cameraScript.Locked = true;
        cameraScript.Target = newVehicle.transform;
        onVehicle = true;
    }

    public void DestroyVehicle()
    {
        activeVehicle = null;
        character.position = character.parent.parent.transform.position;
        GameObject toDestroy = character.parent.parent.gameObject;
        character.parent.parent = null;

        Destroy(toDestroy);
        model.SetActive(true);
        cameraScript.Locked = false;
        cameraScript.Target = character;
        cameraScript.gameObject.transform.rotation = Quaternion.identity;
        onVehicle = false;
    }
}