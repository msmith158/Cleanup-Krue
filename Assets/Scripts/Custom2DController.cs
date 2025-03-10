// Custom2DController

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Custom2DController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Maximum horizontal speed.")]
    public float maxSpeed = 5f;
    [Tooltip("Acceleration rate when input is applied.")]
    public float acceleration = 10f;
    [Tooltip("Deceleration rate (drift) when no input is provided.")]
    public float deceleration = 8f;
    [Tooltip("Initial jump impulse.")]
    public float jumpSpeed = 8f;
    [Tooltip("Gravity force applied each frame.")]
    public float gravity = 20f;

    [Header("Jump Settings")]
    [Tooltip("Maximum number of jumps (1 for single jump, 2 for double jump, etc.).")]
    public int maxJumps = 2;
    [Tooltip("Delay (in seconds) before another jump is allowed after a jump.")]
    public float jumpDelay = 0.2f;

    [Header("Slope Settings")]
    [Tooltip("Distance below the character to check for ground/slope.")]
    public float slopeCheckDistance = 0.5f;
    [Tooltip("Layers considered as ground/slope.")]
    public LayerMask groundMask;

    [Header("Wall Settings")]
    [Tooltip("Extra distance for the wall check (should be at least the CharacterController radius).")]
    public float wallCheckDistance = 0.1f;
    [Tooltip("Layers considered as walls.")]
    public LayerMask wallMask;

    // Interal variables
    [HideInInspector] public bool canMove = true;
    private CharacterController controller;
    private float horizontalSpeed = 0f;    
    private float verticalVelocity = 0f;    
    private int jumpCount = 0;              
    private float jumpTimer = 0f;            
    private bool jumpInputReleased = true; 
    private float lastInputDirection = 0f;   

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool grounded = controller.isGrounded;
        if (grounded)
        {
            jumpCount = 0;
            jumpTimer = 0f;
        }
        else
        {
            if (jumpTimer > 0f)
                jumpTimer -= Time.deltaTime;
        }
        
        float input;
        if (canMove)
            input = Input.GetAxisRaw("Horizontal");
        else input = 0;
        
        if (input != 0)
        {
            lastInputDirection = input;
        }

        bool touchingWall = IsTouchingWall();

        if (input != 0)
        {
            if (touchingWall)
            {
                if (Mathf.Sign(input) != Mathf.Sign(horizontalSpeed) && Mathf.Abs(horizontalSpeed) > 0.01f)
                {
                    horizontalSpeed = 0f;
                }
                horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, input * maxSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, input * maxSpeed, acceleration * Time.deltaTime);
            }
        }
        else
        {
            if (!touchingWall)
            {
                horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, 0, deceleration * Time.deltaTime);
            }
        }

        if (!Input.GetButton("Jump"))
        {
            jumpInputReleased = true;
        }

        if (Input.GetButtonDown("Jump") && canMove && jumpInputReleased && jumpTimer <= 0f && jumpCount < maxJumps)
        {
            verticalVelocity = jumpSpeed;
            jumpCount++;
            jumpInputReleased = false;
            jumpTimer = jumpDelay;
        }

        verticalVelocity -= gravity * Time.deltaTime;

        Vector3 horizontalMove = new Vector3(horizontalSpeed, 0, 0);

        if (grounded)
        {
            Vector3 slopeNormal = GetSlopeNormal();
            if (slopeNormal != Vector3.up)
            {
                horizontalMove = Vector3.ProjectOnPlane(horizontalMove, slopeNormal);
            }
        }

        Vector3 move = horizontalMove;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    bool IsTouchingWall()
    {
        float rayDirectionSign = (Mathf.Abs(horizontalSpeed) > 0.01f) ? Mathf.Sign(horizontalSpeed) : Mathf.Sign(lastInputDirection);
        Vector3 rayOrigin = transform.position + controller.center;
        Vector3 rayDirection = new Vector3(rayDirectionSign, 0, 0);
        float checkDistance = controller.radius + wallCheckDistance;
        return Physics.Raycast(rayOrigin, rayDirection, checkDistance, wallMask);
    }

    Vector3 GetSlopeNormal()
    {
        Vector3 origin = transform.position + controller.center + Vector3.down * (controller.height / 2);
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, slopeCheckDistance + 0.1f, groundMask))
        {
            return hit.normal;
        }
        return Vector3.up;
    }

    void OnDrawGizmos()
    {
        if (controller != null)
        {
            Vector3 wallRayOrigin = transform.position + controller.center;
            float rayDir = (Mathf.Abs(horizontalSpeed) > 0.01f) ? Mathf.Sign(horizontalSpeed) : Mathf.Sign(lastInputDirection);
            Vector3 wallRayDir = new Vector3(rayDir, 0, 0);
            float wallRayDistance = controller.radius + wallCheckDistance;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallRayOrigin, wallRayOrigin + wallRayDir * wallRayDistance);

            Vector3 slopeRayOrigin = transform.position + controller.center + Vector3.down * (controller.height / 2);
            float slopeRayDistance = slopeCheckDistance + 0.1f;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(slopeRayOrigin, slopeRayOrigin + Vector3.down * slopeRayDistance);
        }
    }
}
