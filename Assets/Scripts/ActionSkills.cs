using UnityEngine;
using UnityEngine.Rendering;

[System.Flags]
public enum Type
{
    Physical = 1,
    Fire = 2,
    Ice = 4,
    Thunder = 8,
    Wind = 16,
    Dark = 32,
    Light = 64
}

[CreateAssetMenu]
public class ActionSkills : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    //maybe animation goes here
    public AudioClip clip;

    public Type damageType;
    public int damageValue; //Base Power of the skill

    public enum CostType
    {
        HP,
        MP
    }
    public CostType costType;
    public int cost; //cost of the skill

    public enum TargetType
    {
        Single,
        Multi
    }
    public TargetType targetType;


}
