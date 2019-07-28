using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerStats))]


public class Controller : MonoBehaviour
{
    //setup
    protected Rigidbody rb;
    protected Collider coll;
    protected PlayerStats stats;

    [Header("Movement Settings")]
    public float jumpHeight = 9.0f;
    public float jumpTime;

    public bool canWallJump = false;
    public Vector3 wallJumpClimb;
    public Vector3 wallJumpOff;
    public Vector3 wallLeap;

    public float fallGravityMultiplier = 5.0f;
    public float lowJumpMultiplier = 3.0f;
    public float walkSpeed = 6.0f;
    public float sprintSpeed = 12.0f;
    public float wallSlideSpeed;
    public float timeToWallUnstick;

    protected float moveInput;
    protected float facingDirection;
    protected bool jumpRequest;
    protected int jumpCounter;
    protected float jumpRequestTime;
    protected float jumpTimeCounter;
    protected bool sprintRequest;
    public float rayLength = .1f;
    protected bool isGrounded;
    protected bool isContactAbove;
    protected bool isContactRight;
    protected bool isContactLeft;
    protected int wallDirection;

    protected bool isWallSliding;
    protected float wallStickTime = .25f;

    float velocityXSmoothing;
    public float accelerationTimeGrounded = .1f;
    public float accelerationTimeAirborne = .2f;
    float velocityXTarget;
    float velocityXSmoothed;


    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        stats = GetComponent<PlayerStats>();
    }


    void Start()
    {
        
    }


    void Update()
    {
        GetInput();
    }


    protected virtual void GetInput()
    {

    }


    private void FixedUpdate()
    {
        Movement();
        Debug.DrawRay(rb.position, rb.velocity, Color.red);
    }


    void Movement()
    {
        CollisionTests();

        if (canWallJump)
        {
            WallSlidingTest();
        }


        //TODO walking down and up slopes needs to be fixed
        //TODO player sticks for a fraction of a second when he lands, this needs to be fixed, it should be smoothly transition
        velocityXTarget = moveInput * walkSpeed;
        velocityXSmoothed = Mathf.SmoothDamp(rb.velocity.x, velocityXTarget, ref velocityXSmoothing, (isGrounded) ? accelerationTimeGrounded : accelerationTimeAirborne);
        rb.velocity = new Vector3(velocityXSmoothed, rb.velocity.y, rb.velocity.z);
               
        if (sprintRequest)
        {
            velocityXTarget = moveInput * sprintSpeed;
            velocityXSmoothed = Mathf.SmoothDamp(rb.velocity.x, velocityXTarget, ref velocityXSmoothing, (isGrounded) ? accelerationTimeGrounded : accelerationTimeAirborne);
            rb.velocity = new Vector3(velocityXSmoothed, rb.velocity.y, rb.velocity.z);
            Debug.Log("Sprinting!" + velocityXTarget);
            Debug.Log(rb.velocity.x);
        }



        if (isContactLeft && moveInput < 0.0f || isContactRight && moveInput > 0.0f)
        {
            rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
        }


        if (jumpRequest)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            
            if (isWallSliding)
            { 
                if (wallDirection == moveInput)
                {
                    Vector3 forceToAdd = new Vector3(-wallDirection * wallJumpClimb.x, wallJumpClimb.y, rb.velocity.z);
                    rb.AddForce(forceToAdd, ForceMode.Impulse);
                    //Debug.Log("Wall Jump Climb");
                }
                else if (moveInput == 0)
                {
                    Vector3 forceToAdd = new Vector3(-wallDirection * wallJumpOff.x, wallJumpOff.y, rb.velocity.z);
                    rb.AddForce(forceToAdd, ForceMode.Impulse);
                    //Debug.Log("Wall Jump Off");
                }
                else
                {
                    Vector3 forceToAdd = new Vector3(-wallDirection * wallLeap.x, wallLeap.y, rb.velocity.z);
                    rb.AddForce(forceToAdd, ForceMode.Impulse);
                    //Debug.Log("Wall Leap");
                }
            }
        }
        
        JumpModfier();
        WallSlidingModifier();
    }

    //TODO Check out cast method to see how collisions feel or make .1f a higher number to make collision happen a little earlier
    void CollisionTests()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + rayLength);
        isContactAbove = Physics.Raycast(transform.position, Vector3.up, coll.bounds.extents.y + rayLength);
        isContactRight = Physics.Raycast(transform.position, Vector3.right, coll.bounds.extents.x + rayLength);
        isContactLeft = Physics.Raycast(transform.position, Vector3.left, coll.bounds.extents.x + rayLength);
        
        if(isGrounded)
        {
            jumpCounter = 1;
        }

        if (isContactLeft)
        {
            wallDirection = -1;
        }
        else if (isContactRight)
        {
            wallDirection = 1;
        }
        else
        {
            wallDirection = 0;
        }
    }


    void WallSlidingTest()
    {
        if ((isContactLeft || isContactRight) && !isGrounded)
        {
            isWallSliding = true;

            if (timeToWallUnstick > 0)
            {
                if (moveInput != wallDirection && moveInput != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
            jumpCounter = 1;
        }
        else
        {
            ResetWallSlide();
        }
    }


    void WallSlidingModifier()
    {
        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = Vector3.up * Physics.gravity.y * wallSlideSpeed * Time.deltaTime;
            }
            if (timeToWallUnstick > 0)
            {
                rb.velocity = new Vector3(0.0f, Physics.gravity.y * wallSlideSpeed * Time.deltaTime, rb.velocity.z);
            }
        }
    }


    void JumpModfier()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Physics.gravity * (fallGravityMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpRequest)
        {
            rb.velocity += Physics.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


    public void ResetJump()
    {
        jumpRequestTime = 0.0f;
        jumpRequest = false;
    }


    public void ResetSprint()
    {
        sprintRequest = false;
    }


    void ResetWallSlide()
    {
        timeToWallUnstick = 0f;
        isWallSliding = false;
    }

}
