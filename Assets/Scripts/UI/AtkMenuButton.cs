using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static ActionSkills;

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
    public TMP_Text SkillCost;
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

        if (Skill != null) 
        {
            if (Skill.costType == CostType.MP)
            {
                SkillCost.SetText(Skill.cost.ToString() + " SP");
            }
            else
            {
                int cost;
                cost = (int)(BattleSystem.playerInfo.currHP - Mathf.RoundToInt((BattleSystem.playerInfo.baseHP * Skill.cost) / 100));
                SkillCost.SetText(cost.ToString() + " HP");
            }

            if (BattleSystem.SkillCostCheck(BattleSystem.playerInfo, Skill) != true)
            {
                SkillName.color = Color.gray;
                SkillCost.color = Color.gray;
                menuController.DescriptionBox.color = Color.gray;
            }
            else
            {
                SkillName.color = Color.white;
                SkillCost.color = Color.white;
                menuController.DescriptionBox.color = Color.white;
            }
        }
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

            if (!PlayedSelected)
            {
                PlayedSelected = true;
                SystemAudio.PlayOneShot(UISelected);
            }

            menuController.DescriptionBox.SetText(Skill.skillDescription);

            if (input.UI.PrimaryAction.WasPressedThisFrame())
            {
                animator.SetBool("Pressed", true);


                //select enemy with selected atk here

                
                if(BattleSystem.SkillCostCheck(BattleSystem.playerInfo, Skill) == true)
                {
                    input.Disable();
                    menuController.input.Disable();
                    if (!PlayedConfirmed)
                    {
                        PlayedConfirmed = true;
                        SystemAudio.PlayOneShot(UIConfrimed);
                    }

                    PassToSelection();
                    Invoke("DisablePanel", 0.01f);
                }
                else
                {
                    SystemAudio.PlayOneShot(UIBack);
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
