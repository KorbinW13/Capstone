using UnityEngine;

[CreateAssetMenu]
public class Damage : ScriptableObject
{
    public enum Type
    {
        Physical,
        Fire,
        Ice,
        Thunder,
        Wind,
        Dark,
        Light
    }
    public Type damageType;

    public int damageValue;

    public int cost;
}
