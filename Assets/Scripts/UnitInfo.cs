using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public int weaponPower; //equipment if i get to it
    public int armorDefense; //equipment if i get to it
    public int damage; //needed placeholder for math

    public bool TakeDamage(int BasePower)
    {
        currHP -= BasePower/(int)Mathf.Sqrt(endurance*8 + armorDefense);

        if (currHP <= 0 ) 
        {
            return true;
        }
        else { return false; }
    }

    public bool TakeSkillDamage(int SkillPower)
    {
        currHP -= 1; //temp

        if (currHP <= 0)
        {
            return true;
        }
        else { return false; }
    }
}
