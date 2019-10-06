using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Animator))]


public class Controller : MonoBehaviour
{
    //setup
    protected Rigidbody rb;
    protected CapsuleCollider cColl;
    protected SphereCollider sColl;
    protected PlayerStats stats;

    //animator setup
    protected Animator animator;

    [Header("Movement Settings")]
    public float jumpHeight = 9.0f;
    public float jumpTime;
    public float numberOfJumps = 2;

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
    protected bool crouchRequest;
    protected bool isCrouching;
    public float rayLength = .1f;
    protected bool isGrounded;
    protected float groundedTimeCounter;
    protected bool isContactAbove;
    protected bool isContactRight;
    protected bool isContactLeft;
    protected int wallDirection;
    protected bool isWallSliding;
    protected float wallStickTime = .25f;

    float velocityXSmoothing;
    public float accelerationTimeGrounded = .1f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTouchedDown = .0f;
    float accelerationTime;
    float velocityXTarget;
    float velocityXSmoothed;
    float distanceToGround;


    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        cColl = GetComponent<CapsuleCollider>();
        sColl = GetComponent<SphereCollider>();
        stats = GetComponent<PlayerStats>();

        animator = GetComponent<Animator>();
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
        //DistanceTests();

        if (canWallJump)
        {
            WallSlidingTest();
        }
        
        //TODO walking down and up slopes needs to be fixed
        velocityXTarget = moveInput * walkSpeed;

        if (isGrounded && groundedTimeCounter < .2f)
        {
            accelerationTime = Mathf.SmoothDamp(accelerationTouchedDown, accelerationTimeGrounded, ref velocityXSmoothing, .2f);
        }
        else if (isGrounded)
        {
            accelerationTime = .1f;
        }
        else
        {
            accelerationTime = .2f;
        }

        velocityXSmoothed = Mathf.SmoothDamp(rb.velocity.x, velocityXTarget, ref velocityXSmoothing, accelerationTime);
        rb.velocity = new Vector3(velocityXSmoothed, rb.velocity.y, rb.velocity.z);
        //Debug.Log(accelerationTime);
        //Debug.Log(rb.velocity.x);
        
        if (sprintRequest && !isCrouching)
        {
            SprintModifier();
        }


        if (isContactLeft && moveInput < 0.0f || isContactRight && moveInput > 0.0f)
        {
            rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
        }


        if (jumpRequest)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            groundedTimeCounter = 0f;
            
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


        if (crouchRequest)
        {
            CrouchModifier();
        }

        
        JumpModfier();
        WallSlidingModifier();
    }

    //TODO Check out cast method to see how collisions feel or make .1f a higher number to make collision happen a little earlier
    //TODO Need to turn each of these into 3 rays(left, center, right)(top, center, bottom)
    void CollisionTests()
    {
        isGrounded = Physics.Raycast(transform.position + sColl.center, Vector3.down, sColl.bounds.extents.y + rayLength);
        isContactAbove = Physics.Raycast(transform.position + cColl.center, Vector3.up, cColl.bounds.extents.y + rayLength);
        isContactRight = Physics.Raycast(transform.position + cColl.center, Vector3.right, cColl.bounds.extents.x + rayLength);
        isContactLeft = Physics.Raycast(transform.position + cColl.center, Vector3.left, cColl.bounds.extents.x + rayLength);

        if (isCrouching)
        {
            isContactAbove = Physics.Raycast(transform.position + sColl.center, Vector3.up, sColl.bounds.extents.y + rayLength);
            isContactRight = Physics.Raycast(transform.position + sColl.center, Vector3.right, sColl.bounds.extents.x + rayLength);
            isContactLeft = Physics.Raycast(transform.position + sColl.center, Vector3.left, sColl.bounds.extents.x + rayLength);
        }
        
        if (isGrounded)
        {
            jumpCounter = 1;
            groundedTimeCounter += Time.deltaTime;
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


    void DistanceTests()
    {
        RaycastHit downHit;
        if (Physics.Raycast(transform.position, Vector3.down, out downHit))
            distanceToGround = downHit.distance;
            //Debug.Log("Distance from ground is: " + downHit.distance);
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


    void SprintModifier()
    {
        velocityXTarget = moveInput * sprintSpeed;
        velocityXSmoothed = Mathf.SmoothDamp(rb.velocity.x, velocityXTarget, ref velocityXSmoothing, accelerationTime);
        rb.velocity = new Vector3(velocityXSmoothed, rb.velocity.y, rb.velocity.z);
    }


    void CrouchModifier()
    {
        animator.SetBool("crouchPressed", true);
        cColl.enabled = false;
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


    public void ResetCrouch()
    {
        crouchRequest = false;
        isCrouching = false;
        cColl.enabled = true;
        animator.SetBool("crouchPressed", false);
    }


    public void ResetWallSlide()
    {
        timeToWallUnstick = 0f;
        isWallSliding = false;
    }

}
