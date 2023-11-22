using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitInfo : MonoBehaviour
{
    public string UnitName;
    public Animator animator;
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
    // (Agility/2) + (Luck/4) + Hit Rate of current Weapon

    //Evasion from different games
    // Agility + (Luck/4) + Evade of all equipment

    // hit or miss would probably be playerAccuracy/EnemyEvasionRate %

    public Type typeWeak;

    public Type typeResist;

    public Type typeNull;

    public Type typeDrain;

    public List<ActionSkills> SkillList = new List<ActionSkills>();

    public bool TakeDamage(int BasePower, TypeReaction reaction)
    {
        BasePower = BasePower / (int)Mathf.Sqrt(endurance * 8 + armorDefense);
        if (reaction == TypeReaction.Drain)
        {
            currHP += BasePower;
            currHP = Mathf.Clamp(currHP, 0, baseHP);
        }
        else if (reaction == TypeReaction.Null)
        {
            return false;
        }
        else
        {
            currHP -= BasePower;
            currHP = Mathf.Clamp(currHP, 0, baseHP);
        }

        if (currHP <= 0 ) 
        {
            animator.SetBool("isDead", true);
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
