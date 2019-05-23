using UnityEngine;


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
    private float moveInput;

    bool jumpRequest;
    int jumpCounter;
    public float jumpTime;
    float jumpTimeCounter;
    float jumpRequestTime;

    bool isGrounded;
    public Transform feetPosition;
    public float checkRadius;
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
        Walk();
        Jump();

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        JumpModfier();
    }


    void Update()
    {
        TestGroundedState();
    }


    void Walk()
    {
        ResetMovementToZero();

        moveInput = Input.GetAxisRaw(horizontalCtrl);
            
        if (moveInput != 0f)
        {
            velocity += Vector3.right * moveInput * stats.walkSpeed.GetValue();
        }
    }


   
    void Jump()
    {
        if (Input.GetButton(jumpButton))
        {
            if (jumpRequestTime == 0f)
            {
                if (isGrounded || jumpRequestTime < 1f && jumpCounter < 2)
                {
                    jumpRequest = true;
                    jumpCounter++;
                    isGrounded = false;
                    jumpTimeCounter = jumpTime;
                    rb.velocity = Vector3.up * stats.jumpHeight.GetValue();
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
            jumpRequestTime += Time.deltaTime;
        }
        else
        {
            ResetJump();
        }
    }


    // Used checkSphere to have a layer mask... this can be used to set different interactions depending on ground type.. sticky floor harder to jump etc.
    void TestGroundedState()
    {
        if (Physics.CheckSphere(feetPosition.position, checkRadius, whatIsGround))
        {
            isGrounded = true;
            jumpCounter = 1;
        }
        else
        {
            isGrounded = false;
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


    void ResetMovementToZero()
    {
        velocity = Vector3.zero;
    }

    
    void ResetJump()
    {
        jumpRequestTime = 0f;
        jumpRequest = false;
    }


}


