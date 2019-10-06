using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    public override void Die()
    {
        base.Die();

        //TODO: Add death animation and loot drops

        Destroy(gameObject);
    }
}