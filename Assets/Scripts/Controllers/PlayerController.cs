using UnityEngine;


public enum FacingDirection
{
    Up,
    Down,
    Left,
    Right
}


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerStats))]


public class PlayerController : MonoBehaviour
{
    //setup
    protected Rigidbody rb;
    protected Collider coll;
    protected PlayerStats stats;

    //input
    public bool useController;
    public string jumpButton = "Jump";
    public string horizontalCtrl = "Horizontal";

    //movement
    private Vector3 velocity = Vector3.zero;
    FacingDirection facing = FacingDirection.Right;
    private float moveInput;

    public float walkSpeed = 6;
    public float jumpHeight = 9;
    bool jumpRequest;
    int jumpCounter;
    public float jumpTime;
    float jumpTimeCounter;
    float jumpRequestTime;

    public Vector3 wallJumpClimb;
    public Vector3 wallJumpOff;
    public Vector3 wallLeap;

    public float wallSlideSpeed = 3;
    bool isWallSliding = false;
    int wallDirX;
    float wallStickTime = .25f;
    public float timeToWallUnstick;
    

    bool isContactRight;
    bool isContactLeft;
    bool isGrounded;
    public LayerMask whatIsGround;

    public float fallMultiplier = 4.5f;
    public float lowJumpMultiplier = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        stats = GetComponent<PlayerStats>();
    }


    private void FixedUpdate()
    {

        ApplyMovement();
  

        //rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        
        JumpModfier();
        WallSlidingModifier();
    }


    void Update()
    {
        CheckInput();

        TestGroundedState();
        TestWallSlidingState();
    }


    private void CheckInput()
    {
        moveInput = Input.GetAxisRaw(horizontalCtrl);

        if (Input.GetButton(jumpButton))
        {
            if (jumpRequestTime == 0f)
            {
                if ((isGrounded || jumpRequestTime < 1f && jumpCounter < 2) && !isWallSliding)
                {
                    jumpRequest = true;
                    jumpCounter++;
                    isGrounded = false;
                    jumpTimeCounter = jumpTime;
                    rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
                }
            }
            if (jumpRequestTime > 0f && jumpRequest)
            {
                if (jumpTimeCounter > 0)
                {
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    jumpRequest = false;
                }
            }
            if (isWallSliding)
            {
                if (wallDirX == moveInput)
                {
                    Vector3 forceToAdd = new Vector3(-wallDirX * wallJumpClimb.x, wallJumpClimb.y, rb.velocity.z);
                    rb.AddForce(forceToAdd, ForceMode.Impulse);
                    Debug.Log("Wall Jump Climb");
                }
                else if (moveInput == 0)
                {
                    Vector3 forceToAdd = new Vector3(-wallDirX * wallJumpOff.x, wallJumpOff.y, rb.velocity.z);
                    rb.AddForce(forceToAdd, ForceMode.Impulse);
                    Debug.Log("Wall Jump Off");
                }
                else
                {
                    Vector3 forceToAdd = new Vector3(-wallDirX * wallLeap.x, wallLeap.y, rb.velocity.z);
                    rb.AddForce(forceToAdd, ForceMode.Impulse);
                    Debug.Log("Wall Leap");
                }

            }
            jumpRequestTime += Time.deltaTime;

        }
        else
        {
            ResetJump();
        }
    }


    void ApplyMovement()
    {
        rb.velocity = new Vector3(moveInput * walkSpeed, rb.velocity.y, rb.velocity.z);
        Debug.DrawRay(transform.position, rb.velocity);
    }

    
    //TODO:  Need to add more rays Left, Middle, Right of character
    void TestGroundedState()
    {
        if (Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + .225f, whatIsGround))
        {
            isGrounded = true;
            jumpCounter = 1;
        }
        else
        {
            isGrounded = false;
        }
    }


    //TODO:  Need to add more rays Top, Middle, Bottom of character 
    void TestWallSlidingState()
    {
        isContactRight = Physics.Raycast(transform.position, Vector3.right, coll.bounds.extents.x + 0.1f);
        isContactLeft = Physics.Raycast(transform.position, Vector3.left, coll.bounds.extents.x + 0.1f);

        if (isContactLeft)
        {
            wallDirX = -1;
        }
        else
        {
            wallDirX = 1;
        }

        if ((isContactLeft || isContactRight) && !isGrounded && rb.velocity.y < 0.0f)
        {
            isWallSliding = true;

            if (timeToWallUnstick > 0)
            {
                if (moveInput != wallDirX && moveInput != 0)
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


    void JumpModfier()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpRequest)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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


    void ResetMovementToZero()
    {
        rb.velocity = Vector3.zero;
    }

    
    void ResetJump()
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


