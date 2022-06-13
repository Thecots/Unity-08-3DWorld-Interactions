using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    private float horizontal;
    private float vertical;

    // velocidad de movimiento
    [Header("Velocidad de movimiento")]
    private Vector3 direction;
    public float walkingSpeed = 5f;
    public float runningSpeed = 10f; 
    public float speed;

    // push power
    [Header("Fuerza de empuje")]
    public float pushRunningPower = 10.0f;
    public float pushWalkingPower = 5.0f;
    public float pushPower = 5.0f;
    private float targetMass;

    // gravedad
    [Header("Gravedad")]
    public Vector3 velocity;
    public float gravity = 9.8f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;
    
    // salto
    [Header("Salto")]
    public float jumpHeight = 30f;
    public int jumps = 0;
    public int maxJumps = 2;

    // camara
    [Header("Camara")]
    public Transform cam;
    private Vector3 movePlayer;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // deslizar pendientes
    [Header("Deslizar pendientes")]
    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity = 7f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = walkingSpeed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            SpeedController();
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            slideDown();

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }     

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            jumps += 1;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * -gravity);
        }else if(Input.GetButtonDown("Jump") && jumps < maxJumps)
        {
            jumps += 1;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * -gravity);
        }else if(isGrounded && jumps != 0)
        {
            jumps = 0;
        }

        velocity.y += -gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }

   
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;

        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic) return;
        if (hit.moveDirection.y < -0.3) return;

        targetMass = body.mass;
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * pushPower / targetMass;
    }

    private void slideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= controller.slopeLimit;

        if (isOnSlope)
        {
            direction.x += hitNormal.x * slideVelocity;
            direction.z += hitNormal.z * slideVelocity;

        }
    }

    private void SpeedController()
    {
        if (Input.GetKey(KeyCode.LeftShift) && speed < runningSpeed)
        {
            pushPower = pushRunningPower;
            speed += 0.1f;
            if (speed > runningSpeed)
            {
                speed = runningSpeed;
            }
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && speed > walkingSpeed)
        {
            pushPower = pushWalkingPower;
            speed -= 0.1f;
            if (speed < walkingSpeed)
            {
                speed = walkingSpeed;
            }
        }
    }

   

}
