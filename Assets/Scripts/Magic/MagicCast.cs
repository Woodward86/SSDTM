using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(CombatController))]
public class MagicCast : MonoBehaviour
{

    protected Player player;
    protected PlayerStats playerStats;
    protected Inventory inventory;
    protected CombatController casterCombat;

    public List<Transform> castPoints = new List<Transform>();


    private void Start()
    {
        player = GetComponent<Player>();
        playerStats = GetComponent<PlayerStats>();
        inventory = GetComponent<Inventory>();
        casterCombat = GetComponent<CombatController>();
    }


    public void BasicAttack()
    {
        foreach (Transform cp in castPoints)
        {
            if (cp.name == inventory.manaClasses[0].spells[0].castPoint.ToString())
            {
                Debug.Log("Casting " + inventory.manaClasses[0].spells[0].name + " from " + cp.name);
                GameObject projectile = Instantiate(inventory.manaClasses[0].spells[0].projectilePrefab, cp.position, cp.rotation);
                projectile.GetComponent<Projectile>().caster = casterCombat;
                projectile.GetComponent<Projectile>().projectileDamage = inventory.manaClasses[0].spells[0].damage;
                PlaySpellEffect(inventory.manaClasses[0].spells[0].visualEffects, cp.position, projectile);
            }
        }
    }


    public void SpecialAttack()
    {

    }


    public void PlaySpellEffect(GameObject effect, Vector3 origin, GameObject parent)
    {
        if (effect != null)
        {
            GameObject spellEffect = Instantiate(effect, origin, effect.transform.rotation);
            spellEffect.transform.parent = parent.transform;
        }
    }
}
