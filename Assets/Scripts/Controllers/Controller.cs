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
    public float jumpTime;
    public float fallGravityMultiplier = 5.0f;
    public float lowJumpMultiplier = 3.0f;

    protected float moveInput;
    protected float facingDirection;
    protected bool jumpRequest;
    protected int jumpCounter;
    protected float jumpRequestTime;
    protected float jumpTimeCounter;
    protected bool isGrounded;
    protected bool isContactRight;
    protected bool isContactLeft;
    protected bool isContactAbove;


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
        JumpModfier();

        //TODO walking down and up slopes needs to be fixed so you stick to the ground might need to change this over to rb.position
        rb.velocity = new Vector3(moveInput * stats.walkSpeed.GetValue(), rb.velocity.y, rb.velocity.z);

        if (jumpRequest)
        {
            rb.velocity = new Vector3(rb.velocity.x, stats.jumpHeight.GetValue(), rb.velocity.z);
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

}
