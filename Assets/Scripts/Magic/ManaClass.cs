using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Class", menuName = "Magic/Mana Class")]
public class ManaClass : ScriptableObject
{

    new public string name = "New Class";

    public List<Spell> offensiveSpells = new List<Spell>();
    public List<Spell> defensiveSpells = new List<Spell>();


}
