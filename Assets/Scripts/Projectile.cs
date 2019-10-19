using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string projectileType;
    public float speed;
    public float lifeTime;
    public Rigidbody rb;

    //Auto generated when MagicCast instantiates the projectile
    public int projectileDamage;
    public CombatController caster;
    public Vector3 castVector;

    private float lifeCounter;


    void Start()
    {
        if (projectileType != "Linear")
        {
            Debug.Log("Projectile is not linear.");
        }
        else
        {
            rb.velocity = castVector * speed;
        }
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


