using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static ActionSkills;
using System;

public class AtkMenuButton : MonoBehaviour
{
    public AtkMenuController menuController;
    GameObject BattleObject;
    BattleStateMachine BattleSystem;
    GameObject MainPanel; //for later
    public ActionSkills Skill; //scriptable object prefab
    public RectTransform m_Rect;
    public TMP_Text SkillName; //textbox object
    public Animator animator;
    public int thisIndex;
    InputSystem input;
    GameObject ParentPanel;
    

    //UI sound
    public AudioSource SystemAudio;
    public AudioClip UISelected;
    public AudioClip UIConfrimed;
    public AudioClip UIBack;
    bool PlayedSelected;
    bool PlayedConfirmed;

    
    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();

        ParentPanel = transform.parent.gameObject.transform.parent.gameObject;
        menuController = transform.parent.gameObject.GetComponent<AtkMenuController>();
        BattleObject = GameObject.Find("Battle System");
        BattleSystem = BattleObject.GetComponent<BattleStateMachine>();


        SystemAudio = BattleObject.GetComponent<AudioSource>();
        MainPanel = GameObject.Find("ActionSelectorMenu");

        if (menuController.index == thisIndex && SystemAudio.isPlaying)
        {
            PlayedSelected = true;
        }

        input = new InputSystem();
        input.Enable();

    }

    private void Start()
    {
        m_Rect.SetSiblingIndex(thisIndex);
        GetSkillObj();
    }


    void Update()
    {
        ButtonAction(); //returns error on last child
    }


    public void ButtonAction()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(m_Rect, Input.mousePosition))
        {
            menuController.index = thisIndex;
        }

        if (menuController.index == thisIndex)//returns error on last child
        {
            animator.SetBool("Selected", true);

            if (!PlayedSelected & !SystemAudio.isPlaying)
            {
                PlayedSelected = true;
                SystemAudio.PlayOneShot(UISelected);
            }

            if (input.UI.PrimaryAction.WasPressedThisFrame())
            {
                animator.SetBool("Pressed", true);


                //select enemy with selected atk here

                if (Skill.costType == CostType.HP)
                {
                    if (BattleSystem.playerInfo.currHP - Skill.cost != 0)
                    {
                        if (!PlayedConfirmed && !SystemAudio.isPlaying)
                        {
                            PlayedConfirmed = true;
                            SystemAudio.PlayOneShot(UIConfrimed);
                        }
                        Invoke("DisablePanel", 0.01f);
                        BattleSystem.OnSkillButton(Skill);
                    }
                    else
                    {
                        
                        animator.SetBool("Pressed", false);
                    }
                }
                else if (Skill.costType == CostType.MP)
                {
                    if (BattleSystem.playerInfo.currMP - Skill.cost != 0)
                    {
                        if (!PlayedConfirmed && !SystemAudio.isPlaying)
                        {
                            PlayedConfirmed = true;
                            SystemAudio.PlayOneShot(UIConfrimed);
                        }
                        Invoke("DisablePanel", 0.01f);
                        BattleSystem.OnSkillButton(Skill);
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
                PlayedConfirmed = false;
                
            }
        }
        else
        {
            animator.SetBool("Selected", false);
            PlayedSelected = false;
            
        }


        /* Will probably need later when enemy selection is a thing
        if (input.UI.Back.WasPressedThisFrame())
        {
            SystemAudio.PlayOneShot(UIBack);
            //Invoke("DisablePanel", 1f);
            //MainPanel.SetActive(true);//Take us pack to Main Panel
        }
        */
    }

    public void GetSkillObj()
    {
        if (Skill != null)
        {
            name = Skill.skillName;
            SkillName.SetText(Skill.skillName);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void GetSoundEffect(AudioClip clip, bool boolian)
    {
        if (!boolian && !SystemAudio.isPlaying)
        {
            boolian = true;
            SystemAudio.PlayOneShot(clip);
        }
    }

    void DisablePanel()
    {
        input.Disable();
        ParentPanel.SetActive(false);
    }
}
