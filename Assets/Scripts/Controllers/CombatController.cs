using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CombatController : MonoBehaviour
{
    CharacterStats myStats;
    CombatController attacker;

    private void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }


    private void OnTriggerEnter(Collider weapon)
    {
        if (weapon.CompareTag("Weapon"))
        {
            attacker = weapon.GetComponentInParent<CombatController>();
            //Debug.Log(attacker);
            if (attacker != null && attacker.name != gameObject.name)
            {
                attacker.Attack(myStats);
            }
        }
    }


    public void Attack(CharacterStats targetStats)
    {
        targetStats.TakeDamage(myStats.damage.GetValue());
    }
}