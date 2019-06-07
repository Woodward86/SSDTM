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
    protected Vector3 velocity;

    public float fallGravityMultiplier = 5.0f;
    public float lowJumpGravityMultiplier = 3.0f;


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
        
    }


    private void FixedUpdate()
    {
        JumpModfier();

        //Vector3 deltaPosition = rb.velocity * Time.deltaTime;

        //Vector3 move = Vector3.up * deltaPosition.y;

        //Movement(move);
        Debug.DrawRay(rb.position, rb.velocity);
    }


    void Movement(Vector3 move)
    {
        rb.position = rb.position + move;
    }


    void JumpModfier()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Physics.gravity * (fallGravityMultiplier - 1) * Time.deltaTime;
        }
//         else if (rb.velocity.y > 0 && !jumpRequest)
//         {
//             rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
//         }
    }

}
