using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy
{
    public string name;
    public int lvl;

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

    public Type EnemyType;


    public float baseHP;
    public float currHP;

    public float baseMP;
    public float currMP;

    /*
    public float baseStr;
    public float currStr;
    public float baseEnd;
    public float currEnd;
     */

    public int strength; //indicates effectiveness of physical atks
    public int magic; //indicates effectiveness of magic atks
    public int endurance; //indicates effectiveness of defense
    public int agility; //indicates effectiveness of hit and evasion rates
    public int luck; // possibility of crit hits and eva atks
}
