using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Magic/Spell")]
public class Spell : ScriptableObject
{

    new public string name = "New Spell";
    public Sprite icon = null;
    public GameObject projectilePrefab;
    public ProjectileType projectileType;
    public CastPointTag castPoint;
    //Could change this "isAimable" into an enum with a few other options
    public bool isAimable;
    public GameObject visualEffects;
    public GameObject soundEffects;

    public int damage;
    public int manaCost;
    public float castTime;
    public float coolDown;
    public float coolDownTimer;
    public float spellRadius;

 
}


public enum CastPointTag {Sky, Ground, Face, Chest, RightHand, LeftHand, CenterMass}

public enum ProjectileType {Linear}