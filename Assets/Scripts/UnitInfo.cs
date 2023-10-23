using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UnitInfo : MonoBehaviour
{
    //public string name;
    public int lvl; // entity's level

    public float baseHP; //max HP
    public float currHP; // current HP

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

    //Accuracy math time from different games
    // Dexterity + (Agility/2) + (Luck/4) + Hit Rate of current Weapon
    
    //Evasion Rate from different games
    // Agility + (Dexterity/2) + (Luck/4) + Evade of all equipment


    public List<ActionSkills> SkillList = new List<ActionSkills>();

    public bool TakeDamage(int BasePower)
    {
        currHP -= BasePower/(int)Mathf.Sqrt(endurance*8 + armorDefense);

        if (currHP <= 0 ) 
        {
            return true;
        }
        else { return false; }
    }

    public bool HealDamage(int HealPower)
    {
        //temp
        if (currHP > 0)
        {
            return true;
        }
        else { return false; }
    }
}
