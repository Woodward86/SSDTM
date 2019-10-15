using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed;
    public float lifeTime;
    public Rigidbody rb;

    //Auto generated when MagicCast instantiates the pro
    public int projectileDamage;
    public CombatController caster;

    private float lifeCounter;


    void Start()
    {
        rb.velocity = transform.right * speed;
    }


    private void OnTriggerEnter(Collider hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }


    void Update()
    {
        lifeCounter += Time.deltaTime;
        if (lifeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
