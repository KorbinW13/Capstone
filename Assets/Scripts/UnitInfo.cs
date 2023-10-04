using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitInfo : MonoBehaviour
{
    //public string name;
    public int lvl; // entity's level

    public float baseHP;
    public float currHP;

    public float baseMP;
    public float currMP;

    public int strength; //indicates effectiveness of physical atks
    public int magic; //indicates effectiveness of magic atks
    public int endurance; //indicates effectiveness of defense
    public int agility; //indicates effectiveness of hit and evasion rates
    public int luck; // possibility of crit hits and eva atks

    public int damage; //= (int)Mathf.Sqrt(strength);

    public bool TakeDamage(int dmg)
    {
        currHP -= dmg;

        if (currHP <= 0 ) 
        {
            return true;
        }
        else { return false; }
    }
}
