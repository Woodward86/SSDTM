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
    public Vector3 wallJumpClimb;
    public float fallGravityMultiplier = 5.0f;
    public float lowJumpMultiplier = 3.0f;
    public float walkSpeed = 6.0f;
    public float wallSlideSpeed;
    public float timeToWallUnstick;

    protected float moveInput;
    protected float facingDirection;
    protected bool jumpRequest;
    protected int jumpCounter;
    protected float jumpRequestTime;
    protected float jumpTimeCounter;
    protected bool isGrounded;
    protected bool isContactAbove;
    protected bool isContactRight;
    protected bool isContactLeft;
    protected int wallDirection;
    protected bool isWallSliding;
    protected float wallStickTime = 0.25f;



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
        WallSlidingTest();

        JumpModfier();
        WallSlidingModifier();

        //TODO walking down and up slopes needs to be fixed so you stick to the ground might need to change this over to rb.position
        rb.velocity = new Vector3(moveInput * walkSpeed, rb.velocity.y, rb.velocity.z);

        if (isContactLeft && moveInput < 0.0f || isContactRight && moveInput > 0.0f)
        {
            rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
        }

        if (jumpRequest)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            if (wallDirection == moveInput)
            {
                Vector3 forceToAdd = new Vector3(-wallDirection * wallJumpClimb.x, wallJumpClimb.y, rb.velocity.z);
                rb.AddForce(forceToAdd, ForceMode.Impulse);
                Debug.Log("Wall Jump Climb");
            }
        }
               
    }


    void CollisionTests()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + 0.1f);
        isContactAbove = Physics.Raycast(transform.position, Vector3.up, coll.bounds.extents.y + 0.1f);
        isContactRight = Physics.Raycast(transform.position, Vector3.right, coll.bounds.extents.x + 0.1f);
        isContactLeft = Physics.Raycast(transform.position, Vector3.left, coll.bounds.extents.x + 0.1f);
        
        if(isGrounded)
        {
            jumpCounter = 1;
        }

        if (isContactLeft)
        {
            wallDirection = -1;
        }
        else
        {
            wallDirection = 1;
        }
    }


    void WallSlidingTest()
    {

        if ((isContactLeft || isContactRight) && !isGrounded && rb.velocity.y < 0.0f)
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
            //Not sure about this below might have to remove it so player can climb walls
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
        else if (rb.velocity.y > 0/* && !jumpRequest*/)
        {
            rb.velocity += Physics.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


    public void ResetJump()
    {
        jumpRequestTime = 0.0f;
        jumpRequest = false;
    }


    void ResetWallSlide()
    {
        timeToWallUnstick = 0f;
        isWallSliding = false;
    }

}
