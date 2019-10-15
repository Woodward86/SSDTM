using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Magic/Spell")]
public class Spell : ScriptableObject
{

    new public string name = "New Spell";
    public Sprite icon = null;
    public GameObject projectilePrefab;
    public CastPointTag castPoint;
    public GameObject visualEffects;
    public GameObject soundEffects;


    public int damage;
    public float castTime;
    public float coolDown;

}


public enum CastPointTag {Sky, Ground, Face, Chest, RightHand, LeftHand}