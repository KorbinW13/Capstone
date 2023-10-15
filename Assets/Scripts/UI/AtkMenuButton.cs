using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static ActionSkills;
using System;

public class AtkMenuButton : MonoBehaviour
{
    AtkMenuController menuController;
    BattleStateMachine BattleSystem;
    public ActionSkills Skill; //scriptable object prefab
    public RectTransform m_Rect;
    public TMP_Text SkillName; //textbox object
    public Animator animator;
    public int thisIndex;
    InputSystem input;

    void Start()
    {
        menuController = transform.parent.gameObject.GetComponent<AtkMenuController>();
        BattleSystem = gameObject.GetComponent<BattleStateMachine>();
        m_Rect = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        GetSkillName();
    }

    void OnEnable()
    {
        input = new InputSystem();
        input.Enable();
    }

    void Update()
    {
        input.Enable();
        ButtonAction();
    }
    
    public void ButtonAction()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(m_Rect, Input.mousePosition))
        {
            menuController.index = thisIndex;
        }

        if (menuController.index == thisIndex)
        {
            animator.SetBool("Selected", true);
            if (input.UI.PrimaryAction.WasPressedThisFrame())
            {
                animator.SetBool("Pressed", true);
                //select enemy with selected atk here

                if (Skill.costType == CostType.HP)
                {
                    if (BattleSystem.playerInfo.currHP - Skill.cost != 0)
                    {
                        BattleSystem.OnSkillButton(Skill.damageValue, Skill.damageType, false, Skill.cost);
                    }
                    else
                    {
                        //no action made
                        animator.SetBool("Pressed", false);
                    }
                }
                else if (Skill.costType == CostType.MP)
                {
                    if (BattleSystem.playerInfo.currMP - Skill.cost != 0)
                    {
                        BattleSystem.OnSkillButton(Skill.damageValue, Skill.damageType, true, Skill.cost);
                    }
                    else
                    {
                        //no action made
                        animator.SetBool("Pressed", false);
                    }
                }
                //BattleSystem.OnSkillButton(Skill.damageValue, Skill.damageType, Skill.costType, Skill.cost); pass skill data to BattleStateMachine


            }
            else if (animator.GetBool("Pressed"))
            {
                animator.SetBool("Pressed", false);
            }
        }
        else
        {
            animator.SetBool("Selected", false);
        }
    }

    public void GetSkillName()
    {
        if (Skill != null)
        {
            SkillName.SetText(Skill.skillName);
        }
        else
        {
            SkillName.SetText("No Name");
        }
    }
}
