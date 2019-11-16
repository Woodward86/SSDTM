using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(SphereCollider))]
public class CharacterCollisions : MonoBehaviour
{
    //setup
    public CapsuleCollider cColl;
    public SphereCollider sColl;

    public bool isGrounded;
    public bool isContactAbove;
    public bool isContactRight;
    public bool isContactLeft;
    public int wallDirection;
    public float rayLength = .1f;

    private void OnEnable()
    {
        cColl = GetComponent<CapsuleCollider>();
        sColl = GetComponent<SphereCollider>();
    }


    private void Update()
    {
        CollisionTests();
    }

    //TODO Check out cast method to see how collisions feel or make .1f a higher number to make collision happen a little earlier
    //TODO Need to turn each of these into 3 rays(left, center, right)(top, center, bottom)
    void CollisionTests()
    {
        isGrounded = Physics.Raycast(transform.position + sColl.center, Vector3.down, sColl.bounds.extents.y + rayLength);
        isContactAbove = Physics.Raycast(transform.position + cColl.center, Vector3.up, cColl.bounds.extents.y + rayLength);
        isContactRight = Physics.Raycast(transform.position + cColl.center, Vector3.right, cColl.bounds.extents.x + rayLength);
        isContactLeft = Physics.Raycast(transform.position + cColl.center, Vector3.left, cColl.bounds.extents.x + rayLength);

        if (!cColl.enabled)
        {
            isContactAbove = Physics.Raycast(transform.position + sColl.center, Vector3.up, sColl.bounds.extents.y + rayLength);
            isContactRight = Physics.Raycast(transform.position + sColl.center, Vector3.right, sColl.bounds.extents.x + rayLength);
            isContactLeft = Physics.Raycast(transform.position + sColl.center, Vector3.left, sColl.bounds.extents.x + rayLength);
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
}
