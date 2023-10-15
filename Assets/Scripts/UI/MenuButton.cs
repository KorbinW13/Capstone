using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static BattleStateMachine;
using static PlayerStateMachine;

public class MenuButton : MonoBehaviour
{
    MenuButtonController menuButtonController;
    public BattleStateMachine BattleSystem;
    public RectTransform m_Rect;
    public Animator animator;
    public int thisIndex;
    InputSystem input;
    [SerializeField] GameObject menuPanelToOpen;
    GameObject ParentPanel;

    public MenuOptions menuOptions;

    void Start()
    {
        menuButtonController = transform.parent.gameObject.GetComponent<MenuButtonController>();
        m_Rect = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        input = new InputSystem();
    }

    private void OnEnable()
    {
        input.Enable();
        ParentPanel = transform.parent.gameObject.transform.parent.gameObject; //to get the parent of the parent object called ActionSelectorMenu
    }

    void FixedUpdate()
    {
        input.Enable();
        ButtonFunction();
    }

    void ButtonFunction()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(m_Rect, Input.mousePosition))
        {
            menuButtonController.index = thisIndex;
        }

        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("Selected", true);
            if (input.UI.PrimaryAction.WasPressedThisFrame())
            {
                animator.SetBool("Pressed", true);
                switch(menuOptions)
                {
                    case MenuOptions.Attack:
                        BattleSystem.OnAttackButton();
                        break;
                    case MenuOptions.Skills:
                        if (menuPanelToOpen != null)
                        {
                            Invoke("DisablePanel", 0.01f);
                            menuPanelToOpen.SetActive(true);
                        }
                        else
                        {
                            TurnPass();
                        }
                        break;
                    case MenuOptions.Items:
                        if (menuPanelToOpen != null)
                        {
                            Invoke("DisablePanel", 0.01f);
                            menuPanelToOpen.SetActive(true);
                        }
                        else
                        {
                            TurnPass();
                        }
                        break;
                    case MenuOptions.Pass:
                        /*if statement here for when party is added
                        if (true)
                        {
                            //next party member
                        }
                        else
                        {
                            TurnPass();
                        }*/
                        TurnPass();
                        break;
                }

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

    void DisablePanel()
    {
        input.Disable();
        ParentPanel.SetActive(false);
    }

    void TurnPass()
    {
        Invoke("DisablePanel", 0.01f);
        BattleSystem.turnState = TurnState.EnemyTurn;
        BattleSystem.StartCoroutine(BattleSystem.EnemyTurn());
    }
}
