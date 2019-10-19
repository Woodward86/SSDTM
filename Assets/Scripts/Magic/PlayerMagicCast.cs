using UnityEngine;


[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerMagicCast : MagicCast
{
    protected PlayerController player;
    protected PlayerStats playerStats;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();
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
                playerStats.ConsumeMana(inventory.manaClasses[0].offensiveSpells[0].manaCost);

                projectile.GetComponent<Projectile>().projectileType = inventory.manaClasses[0].offensiveSpells[0].projectileType.ToString();

                if (player.isAiming && inventory.manaClasses[0].offensiveSpells[0].isAimable)
                {
                    projectile.GetComponent<Projectile>().castVector = player.aimVector.normalized;
                }
                else
                {
                    projectile.GetComponent<Projectile>().castVector = transform.right;
                }

                projectile.GetComponent<Projectile>().caster = casterCombat;
                projectile.GetComponent<Projectile>().projectileDamage = inventory.manaClasses[0].offensiveSpells[0].damage;
                PlaySpellEffect(inventory.manaClasses[0].offensiveSpells[0].visualEffects, cp.position, projectile);
            }
        }
    }


    public override void BasicBlock()
    {
        foreach (Transform cp in castPoints)
        {
            if (cp.name == inventory.manaClasses[1].defensiveSpells[0].castPoint.ToString())
            {
                //Debug.Log("Casting " + inventory.manaClasses[1].defensiveSpells[0].name + " from " + cp.name);
                GameObject shield = Instantiate(inventory.manaClasses[1].defensiveSpells[0].projectilePrefab, cp.position, cp.rotation) as GameObject;

                //Reduce Mana by manaCost
                playerStats.ConsumeMana(inventory.manaClasses[1].defensiveSpells[0].manaCost);

                shield.GetComponent<Projectile>().projectileType = inventory.manaClasses[1].defensiveSpells[0].projectileType.ToString();

                if (player.isAiming && inventory.manaClasses[1].defensiveSpells[0].isAimable)
                {
                    shield.GetComponent<Projectile>().castVector = player.aimVector.normalized;
                }
                else
                {
                    shield.GetComponent<Projectile>().castVector = transform.right;
                }

                shield.GetComponent<Projectile>().caster = casterCombat;
                shield.GetComponent<Projectile>().projectileDamage = inventory.manaClasses[1].defensiveSpells[0].damage;
                PlaySpellEffect(inventory.manaClasses[1].defensiveSpells[0].visualEffects, cp.position, shield);

            }
        }
    }
}
