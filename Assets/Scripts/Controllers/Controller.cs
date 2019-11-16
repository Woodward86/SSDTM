using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterCollisions))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(MagicCast))]
[RequireComponent(typeof(CombatController))]

public class Controller : MonoBehaviour
{
    //setup
    protected Rigidbody rb;
    protected CharacterCollisions cCollisions;

    protected CharacterStats stats;
    protected Inventory inventory;
    protected MagicCast magicCast;
    protected CombatController combatC;

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

    public float preJumpHeight;
    public float fallGravityMultiplier = 5.0f;
    public float lowJumpMultiplier = 3.0f;
    public float walkSpeed = 6.0f;
    public float sprintSpeed = 12.0f;
    public float crouchSpeed = 4.0f;
    public float aimingSpeed = 3.0f;
    public float wallSlideSpeed = 3.0f;
    public float timeToWallUnstick;

    protected float leftYInput;
    protected float leftXInput;
    protected Vector3 direction;
    protected float facingDirection;
    protected float aimDirectionInput;
    protected bool jumpRequest;
    protected bool preJumpPress;
    protected int jumpCounter;
    protected float jumpRequestTime;
    protected float jumpTimeCounter;
    protected bool sprintRequest;
    protected bool crouchRequest;
    protected bool isWallSliding;
    protected float wallStickTime = .25f;

    protected bool aimRequest;
    public bool isAiming;
    public Vector3 aimVector;

    protected float ellapsedTime;
    protected bool basicAttackRequest;
    protected bool blockRequest;


    protected float velocityXSmoothing;
    public float accelerationTimeGrounded = .1f;
    public float accelerationTimeAirborne = .2f;
    protected float accelerationTime;
    protected float velocityXTarget;
    protected float velocityXSmoothed;
    public float distanceToGround;


    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        cCollisions = GetComponent<CharacterCollisions>();
        stats = GetComponent<CharacterStats>();
        inventory = GetComponent<Inventory>();
        magicCast = GetComponent<MagicCast>();
        combatC = GetComponent<CombatController>();
        animator = GetComponent<Animator>();
    }


    void Start()
    {
        
    }


    protected virtual void Update()
    {
        GetInput();
    }


    protected virtual void GetInput()
    {

    }


    private void FixedUpdate()
    {
        Movement();
        Combat();
        Regeneration();
        //Debug.DrawRay(rb.position, rb.velocity, Color.red);
    }


    void Movement()
    {
        DistanceTests();

        velocityXTarget = direction.x * walkSpeed;

        WallSlidingTest(canWallJump);
        
        AimModifier(aimRequest);

        CrouchModifier(crouchRequest);

        SprintModifier(sprintRequest, crouchRequest);

        Walk(direction, velocityXTarget);

        Jump(direction);

        JumpModfier();

        WallSlidingModifier();
        //Debug.Log("rb.velocity: " + rb.velocity + "velocityXSmoothed: " + velocityXSmoothed + ", velocity smoothing is: " + velocityXSmoothing);
    }

    //TODO walking down and up slopes needs to be fixed
    void Walk(Vector3 direction, float speed)
    {
        if (distanceToGround > .5f)
        {
            accelerationTime = accelerationTimeAirborne;
        }
        else
        {
            accelerationTime = accelerationTimeGrounded;
        }

        velocityXSmoothed = Mathf.SmoothDamp(rb.velocity.x, speed, ref velocityXSmoothing, accelerationTime);
        rb.velocity = new Vector3(velocityXSmoothed, rb.velocity.y, rb.velocity.z);

        if (cCollisions.isContactLeft && direction.x < 0.0f || cCollisions.isContactRight && direction.x > 0.0f)
        {
            rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
        }
    }

    void Jump(Vector3 direction)
    {
        if (jumpRequest)
        {
            if (isWallSliding)
            {
                if (cCollisions.wallDirection == direction.x)
                {
                    Vector3 forceToAdd = new Vector3(-cCollisions.wallDirection * wallJumpClimb.x, wallJumpClimb.y, rb.velocity.z);
                    rb.AddForce(forceToAdd, ForceMode.Impulse);
                    //Debug.Log("Wall Jump Climb");
                }
                else if (direction.x == 0)
                {
                    Vector3 forceToAdd = new Vector3(-cCollisions.wallDirection * wallJumpOff.x, wallJumpOff.y, rb.velocity.z);
                    rb.AddForce(forceToAdd, ForceMode.Impulse);
                    //Debug.Log("Wall Jump Off");
                }
                else
                {
                    Vector3 forceToAdd = new Vector3(-cCollisions.wallDirection * wallLeap.x, wallLeap.y, rb.velocity.z);
                    rb.AddForce(forceToAdd, ForceMode.Impulse);
                    //Debug.Log("Wall Leap");
                }
            }
            else if (rb.velocity.y < 0 && distanceToGround <= preJumpHeight)
            {
                preJumpPress = true;
                if (preJumpPress && cCollisions.isGrounded)
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
                    Debug.Log("Premature Jump!");
                    preJumpPress = false;
                }
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            }
        }
    }

    //TODO move this onto it's own component or onto the combatController
    void Combat()
    {
        //TODO look at using coroutines for these timers instead
        inventory.manaClasses[0].offensiveSpells[0].coolDownTimer -= Time.deltaTime;
        inventory.manaClasses[1].defensiveSpells[0].coolDownTimer -= Time.deltaTime;

        if (basicAttackRequest 
            && inventory.manaClasses[0].offensiveSpells[0].coolDownTimer <= 0f
            && inventory.manaClasses[0].offensiveSpells[0].manaCost < stats.currentMana)
        {
            inventory.manaClasses[0].offensiveSpells[0].coolDownTimer = inventory.manaClasses[0].offensiveSpells[0].coolDown;
            magicCast.BasicAttack();
        }


        if (blockRequest 
            && inventory.manaClasses[1].defensiveSpells[0].coolDownTimer <= 0f
            && inventory.manaClasses[1].defensiveSpells[0].manaCost < stats.currentMana)
        {
            inventory.manaClasses[1].defensiveSpells[0].coolDownTimer = inventory.manaClasses[1].defensiveSpells[0].coolDown;
            magicCast.BasicBlock();
        }
    }

    //TODO move this onto it's own component
    void Regeneration()
    {
        //TODO look at using coroutines for these timers instead
        stats.manaRegenTimer -= Time.deltaTime;
        stats.healthRegenTimer -= Time.deltaTime;

        if (stats.manaRegenTimer <= 0f)
        {
            stats.manaRegenTimer = stats.manaRegenSpeed;
            stats.RegenerateMana(stats.manaRegen.GetValue());
        }


        if (stats.healthRegenTimer <= 0f)
        {
            stats.healthRegenTimer = stats.healthRegenSpeed;
            stats.RegenerateHealth(stats.healthRegen.GetValue());
        }

    }

    
    void DistanceTests()
    {
        //TODO the Vector3.down in this should be replaced with a vector when added to transform.position results in zero
        if(!cCollisions.isGrounded)
        {
            RaycastHit downHit;
            if (Physics.Raycast(transform.position + Vector3.down, Vector3.down, out downHit))
                distanceToGround = downHit.distance;
                Debug.DrawRay(transform.position + Vector3.down, Vector3.down * downHit.distance, Color.yellow);
                //Debug.Log("Distance from ground is: " + downHit.distance);
        }
        else
        {
            distanceToGround = 0f;
        }

    }


    void WallSlidingTest(bool canWallJump)
    {
        if (canWallJump)
        {
            if ((cCollisions.isContactLeft || cCollisions.isContactRight) && !cCollisions.isGrounded)
            {
                isWallSliding = true;
                jumpCounter = 1;

                if (timeToWallUnstick > 0)
                {
                    if (direction.x != cCollisions.wallDirection && direction.x != 0)
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
            }
            else
            {
                ResetWallSlide();
            }
        }
        else
        {
            return;
        }
    }


    void WallSlidingModifier()
    {
        if (isWallSliding)
        {
            rb.velocity = new Vector3(0.0f, Physics.gravity.y * wallSlideSpeed * Time.deltaTime, rb.velocity.z);
        }
        else
        {
            return;
        }
    }


    void JumpModfier()
    {
        //Gravity
        if (rb.velocity.y < 0)
        {
            rb.velocity += Physics.gravity * (fallGravityMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpRequest)
        {
            rb.velocity += Physics.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


    void SprintModifier(bool sprintRequest, bool isCrouching)
    {
        if (sprintRequest && !isCrouching)
        {
            velocityXTarget = direction.x * sprintSpeed;
        }
        else
        {
            return;
        }
    }


    void CrouchModifier(bool crouchRequest)
    {
        if (crouchRequest)
        {
            velocityXTarget = direction.x * crouchSpeed;
            animator.SetBool("crouchPressed", true);
            cCollisions.cColl.enabled = false;
        }
        else
        {
            return;
        }
    }


    void AimModifier(bool aimRequest)
    {
        if (aimRequest)
        {
            velocityXTarget = direction.x * aimingSpeed;

            if (aimDirectionInput != 0)
            {
                facingDirection = aimDirectionInput;
                if (facingDirection >= 0.0f)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
            }
            Debug.DrawRay(rb.position, aimVector, Color.red);
        }
        else
        {
            if (direction.x != 0)
            {
                facingDirection = direction.x;
                if (facingDirection >= 0.0f)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
            }
        }
    }


    public void ResetJump()
    {
        jumpRequest = false;
        jumpRequestTime = 0.0f;
        if(cCollisions.isGrounded)
        {
            jumpCounter = 0;
            jumpTimeCounter = 0f;
        }
    }


    public void ResetSprint()
    {
        sprintRequest = false;
    }


    public void ResetCrouch()
    {
        crouchRequest = false;
        cCollisions.cColl.enabled = true;
        animator.SetBool("crouchPressed", false);
    }


    public void ResetWallSlide()
    {
        isWallSliding = false;
        timeToWallUnstick = 0f;
    }


    public void ResetAim()
    {
        aimRequest = false;
        isAiming = false;
        aimVector = transform.right;
    }


    public void ResetBasicAttack()
    {
        basicAttackRequest = false;
    }


    public void ResetBlock()
    {
        blockRequest = false;
    }

}
