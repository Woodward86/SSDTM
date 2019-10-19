using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public float healthRegenSpeed;
    public float healthRegenTimer;
    public int maxMana = 100;
    public int currentMana { get; private set; }
    public float manaRegenSpeed;
    public float manaRegenTimer;

    public Stat armour;
    public Stat spellPower;
    public Stat healthRegen;
    public Stat manaRegen;


    private void Awake()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }


    public void TakeDamage(int damage)
    {
        damage -= armour.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public void RegenerateHealth(int reGen)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += reGen;
            Debug.Log(transform.name + " Regenerated " + healthRegen.GetValue() + " Health.");
        }
    }


    public void ConsumeMana(int manaCost)
    {
        currentMana -= manaCost;
        Debug.Log(transform.name + " Used " + manaCost + " Mana.");
    }


    public void RegenerateMana(int reGen)
    {
        if (currentMana < maxMana)
        {
            currentMana += reGen;
            Debug.Log(transform.name + " Regenerated " + manaRegen.GetValue() + " Mana.");
        }
    }


    public virtual void Die()
    {
        Debug.Log(transform.name + " died.");
    }

}
