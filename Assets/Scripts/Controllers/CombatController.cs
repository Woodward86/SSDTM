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


    private void OnTriggerEnter(Collider spell)
    {
        if (spell.CompareTag("Spell"))
        {
            attacker = spell.GetComponent<Projectile>().caster;
            //Debug.Log(attacker);
            if (attacker != null && attacker.name != gameObject.name)
            {
                attacker.Attack(myStats, spell.GetComponent<Projectile>().projectileDamage);
            }
        }
    }


    public void Attack(CharacterStats targetStats, int projectileDamage)
    {
        targetStats.TakeDamage(projectileDamage * myStats.spellPower.GetValue());
    }

}