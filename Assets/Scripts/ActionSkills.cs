using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class ActionSkills : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    //maybe animation goes here
    public AudioClip clip;

    public enum Type
    {
        Physical,
        Fire,
        Ice,
        Thunder,
        Wind,
        Dark,
        Light,
        Support
    }
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
