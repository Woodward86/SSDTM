using UnityEngine;


[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyStats))]
public class EnemyMagicCast : MagicCast
{
    protected EnemyController enemy;
    protected EnemyStats enemyStats;

    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<EnemyController>();
        enemyStats = GetComponent<EnemyStats>();
    }


    public override void BasicAttack()
    {
        foreach (Transform cp in castPoints)
        {
            if (cp.name == inventory.manaClasses[0].offensiveSpells[0].castPoint.ToString())
            {
                //Debug.Log("Casting " + inventory.manaClasses[0].offensiveSpells[0].name + " from " + cp.name);
                GameObject projectile = Instantiate(inventory.manaClasses[0].offensiveSpells[0].projectilePrefab, cp.position, cp.rotation);
                
                //Reduce Mana by manaCost
                enemyStats.ConsumeMana(inventory.manaClasses[0].offensiveSpells[0].manaCost);

                projectile.GetComponent<Projectile>().projectileType = inventory.manaClasses[0].offensiveSpells[0].projectileType.ToString();

                //TODO replace the below with a cast vector that tracks the target somewhat
                projectile.GetComponent<Projectile>().castVector = transform.right;

                projectile.GetComponent<Projectile>().caster = casterCombat;

                projectile.GetComponent<Projectile>().projectileDamage = inventory.manaClasses[0].offensiveSpells[0].damage;
                PlaySpellEffect(inventory.manaClasses[0].offensiveSpells[0].visualEffects, cp.position, projectile);
            }
        }
    }

}
