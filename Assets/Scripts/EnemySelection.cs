using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    public int index;
    public int maxIndex;

    public GameObject thisPanel;
    public GameObject PrevPanel;
    public GameObject EnemySpawnObj;
    public List<UnitInfo> selectable_targets = new List<UnitInfo>();
    public UnitInfo EnemySelected;
    public InputSystem input;

    public BattleStateMachine BattleSystem;

    public bool isSkill = false;
    public ActionSkills Skill;

    //UI Sounds
    public AudioSource SystemAudio;
    public AudioClip Confirmed;
    public AudioClip Back;
    public AudioClip Denied;
    bool PlayedConfirmed;
    bool PlayedBack;
    bool PlayedDenied;

    private void Start()
    {
        thisPanel = this.gameObject;
        SystemAudio = BattleSystem.GetComponent<AudioSource>();
    }

    void Awake()
    {
        input = new InputSystem();
        input.Enable();
    }

    void OnEnable()
    {
        maxIndex = BattleSystem.enemyCount - 1;
        selectable_targets = BattleSystem.EnemyMembers;
        foreach (Transform child in EnemySpawnObj.transform)
        {
            if (child.tag == "Enemy")
            {
                child.GetComponent<EnemySelected>().SelectionPanel(thisPanel);
                child.GetComponent<EnemySelected>().enabled = true;
            }
        }
        input = new InputSystem();
        input.Enable();
    }

    void OnDisable()
    {
        foreach (Transform child in EnemySpawnObj.transform)
        {
            if (child.tag == "Enemy")
            {
                child.GetComponent<EnemySelected>().enabled = false;
            }
        }

        PlayedConfirmed = false;
        PlayedBack = false;
        PlayedDenied = false;
    }

    void Update()
    {
        EnemyScroll();
        DisableSelection();
        SelectEnemyConfirmed();
    }

    public void EnemyScroll()
    {
        if (input.UI.Scroll.ReadValue<float>() != 0 || input.UI.ScrollLeft.WasPressedThisFrame() || input.UI.ScrollRight.WasPressedThisFrame())
        {
            if (input.UI.Scroll.ReadValue<float>() < 0 || input.UI.ScrollLeft.WasPressedThisFrame())
            {
                if (index < maxIndex)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
            }
            if (input.UI.Scroll.ReadValue<float>() > 0 || input.UI.ScrollRight.WasPressedThisFrame())
            {

                if (index > 0)
                {
                    index--;
                }
                else
                {
                    index = maxIndex;
                }
            }
        }
    }

    public void DisableSelection()
    {
        if (input.UI.Back.WasPressedThisFrame())
        {
            if (!PlayedBack)
            {
                PlayedBack = true;
                SystemAudio.PlayOneShot(Back);
            }

            PrevPanel.SetActive(true);
            thisPanel.SetActive(false);
        }
    }

    public void SelectEnemyConfirmed()
    {
        if (input.UI.PrimaryAction.WasPressedThisFrame())
        {
            EnemySelected = selectable_targets[index];
            if (EnemySelected.currHP != 0)
            {
                if (!PlayedConfirmed)
                {
                    PlayedConfirmed = true;
                    SystemAudio.PlayOneShot(Confirmed);

                    BattleSystem.enemyInfo = EnemySelected;
                    input.Disable();
                    if (isSkill)
                    {
                        isSkill = false;
                        Debug.Log(Skill.name + " attacked at " + EnemySelected.name);
                        BattleSystem.OnSkillButton(Skill);
                        thisPanel.SetActive(false);
                    }
                    else
                    {
                        BattleSystem.OnAttackButton();
                        thisPanel.SetActive(false);
                    }
                }
            }
            else 
            {
                if (!PlayedDenied)
                {
                    PlayedDenied = true;
                    SystemAudio.PlayOneShot(Denied);
                }
                return; 
            }
        }
    }
}
