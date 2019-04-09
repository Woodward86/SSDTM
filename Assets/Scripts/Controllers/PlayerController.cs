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
    public string jumpButton = "Jump_P1";
    public string horizontalCtrl = "Horizontal_P1";

    //movement
    private Vector3 velocity = Vector3.zero;
    float horizontalVelocity;
    float verticalVelocity;

    float jumpPressTime;
    bool jumpRequest;

    public float fallMultiplier = 4.5f;
    public float lowJumpMultiplier = 3f;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        stats = GetComponent<PlayerStats>();
    }


    void Update()
    {
        //input calls
        Walk();
        Jump();


        rb.velocity = new Vector3(velocity.x, rb.velocity.y + verticalVelocity, velocity.z);
        
        //movement modifiers
        JumpModfier();
    }


    void Walk()
    {
        ResetMovementToZero();

        float xMove = Input.GetAxisRaw(horizontalCtrl);
        
        if (xMove != 0f)
        {
            velocity += Vector3.right * xMove * stats.walkSpeed.GetValue();
        }
    }


    void Jump()
    {
        if (Input.GetButton(jumpButton))
        {
            if (jumpPressTime == 0f)
            {
                jumpRequest = true;
                verticalVelocity = stats.jumpHeight.GetValue();
            }

            jumpPressTime += Time.deltaTime;
        }

        else
        {
            ResetJump();
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
        verticalVelocity = 0f;
    }

    
    void ResetJump()
    {
        jumpPressTime = 0f;
        jumpRequest = false;
    }


}


