using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(CombatController))]
public class MagicCast : MonoBehaviour
{

    protected Inventory inventory;
    protected CombatController casterCombat;

    public List<Transform> castPoints = new List<Transform>();


    protected virtual void Start()
    {
        inventory = GetComponent<Inventory>();
        casterCombat = GetComponent<CombatController>();
    }


    public virtual void BasicAttack()
    {

    }


    public virtual void SpecialAttack()
    {

    }


    public virtual void BasicBlock()
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
