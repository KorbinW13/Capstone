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
    public EnemySelection enemySelection;
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
        if (RectTransformUtility.RectangleContainsScreenPoint(m_Rect, input.UI.MousePosition.ReadValue<Vector2>()))
        {
            menuController.index = thisIndex;
        }

        if (menuController.index == thisIndex)//returns error on last child
        {
            animator.SetBool("Selected", true);

            if (!PlayedSelected && !SystemAudio.isPlaying)
            {
                PlayedSelected = true;
                SystemAudio.PlayOneShot(UISelected);
            }

            if (input.UI.PrimaryAction.WasPressedThisFrame())
            {
                animator.SetBool("Pressed", true);


                //select enemy with selected atk here

                
                if(BattleSystem.SkillCostCheck(BattleSystem.playerInfo, Skill) == true)
                {
                    input.Disable();
                    menuController.input.Disable();
                    if (!PlayedConfirmed && !SystemAudio.isPlaying)
                    {
                        PlayedConfirmed = true;
                        SystemAudio.PlayOneShot(UIConfrimed);
                    }

                    PassToSelection();
                    Invoke("DisablePanel", 0.01f);
                }
                else
                {
                    animator.SetBool("Pressed", false);
                }
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

    void PassToSelection()
    {
        enemySelection.PrevPanel = ParentPanel;
        enemySelection.Skill = Skill;
        enemySelection.isSkill = true;
        enemySelection.gameObject.SetActive(true);
    }
}
