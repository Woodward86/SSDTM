using UnityEngine;


public class EnemyController : Controller
{
    //Target will need to be replaced with something that dynamically fills when game starts
    public Transform target;
    public float distanceFromTarget;
    public float lookRadius = 10f;

    protected override void Update()
    {
        Vector3 offset = target.position - transform.position;
        distanceFromTarget = offset.sqrMagnitude;
        base.Update();
    }


    protected override void GetInput()
    {
        if (distanceFromTarget < (lookRadius * lookRadius))
        {
            basicAttackRequest = true;
        }
        else
        {
            ResetBasicAttack();
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}