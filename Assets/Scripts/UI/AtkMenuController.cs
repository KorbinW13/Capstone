using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AtkMenuController : MonoBehaviour
{
    public BattleStateMachine BattleSystem;
    public EnemySelection enemySelection;
    public int index;
    public int maxIndex;
    public InputSystem input;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GameObject PrevPanel;
    [SerializeField] GameObject ParentPanel;
    public GameObject button;
    public TMP_Text DescriptionBox;

    //return to main panel
    public AudioSource SystemAudio;
    public AudioClip UIBack;
    bool PlayedBack;

    List<ActionSkills> SkillList = new List<ActionSkills>(); //temp adding of skills to buttons
    

    void Awake()
    {
        //BattleSystem = GameObject.Find("Battle System").GetComponent<BattleStateMachine>();
        
        input = new InputSystem();
        input.Enable();
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        input.Enable();
        SkillList = new List<ActionSkills>(BattleSystem.playerInfo.SkillList);
        Debug.Log("SkillList Size: " + SkillList.Count);

        for (int index = 0; index < SkillList.Count; index++)
        {
            CreateSkillButton(button, rectTransform, index);


            Debug.Log("index: " + index);
            Debug.Log("SiblingIndex: " + button.transform.GetSiblingIndex() + " : " + SkillList[index]);
        }
        
        Debug.Log(rectTransform.childCount);

        //maxIndex = rectTransform.childCount - 1; //making it dynamic i think based to how many skills the player has

    }

    private void OnDisable()
    {
        for (int index = transform.childCount - 1; index >= 0; index--)
        {
            Transform child = transform.GetChild(index);
            Destroy(child.gameObject);//destroys the child gameObject in our case the buttons
        }
    }

    void Update()
    {
        MenuScroll();//Scroll thourgh the options
    }

    void MenuScroll()
    {
        if (input.UI.Scroll.ReadValue<float>() != 0 || input.UI.ScrollUp.WasPressedThisFrame() || input.UI.ScrollDown.WasPressedThisFrame())
        {
            if (input.UI.Scroll.ReadValue<float>() < 0 || input.UI.ScrollDown.WasPressedThisFrame())
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
            else if (input.UI.Scroll.ReadValue<float>() > 0 || input.UI.ScrollUp.WasPressedThisFrame())
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

        if (PrevPanel != null & input.UI.Back.WasPressedThisFrame())
        {
            SystemAudio.PlayOneShot(UIBack);
            PrevPanel.SetActive(true);
            ParentPanel.SetActive(false);
        }
    }

    AtkMenuButton CreateSkillButton(GameObject button, RectTransform rectTransform, int index)
    {
        var newSkill = Instantiate(button, rectTransform);
        var Skill = newSkill.GetComponent<AtkMenuButton>();
        Skill.enemySelection = enemySelection;
        Skill.thisIndex = index;
        Skill.Skill = SkillList[index];
        
        return Skill;
    }

}
