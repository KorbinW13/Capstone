using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public MenuButtonController menuButtonController;
    public BattleStateMachine BattleSystem;
    public RectTransform m_Rect;
    public Animator animator;
    public int thisIndex;
    InputSystem input;
    [SerializeField] GameObject menuPanelToOpen;
    private GameObject ParentPanel;

    void Start()
    {
        m_Rect = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        input = new InputSystem();
        input.Enable();
    }

    private void OnEnable()
    {
        input.Enable();
        ParentPanel = GameObject.Find("ActionSelectorMenu");
    }

    void Update()
    {
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
                if (menuPanelToOpen != null)
                {
                    //menuButtonController.gameObject.SetActive(false);
                    Invoke("DisablePanel", 0.01f);
                    menuPanelToOpen.SetActive(true);
                }
                else if (this.gameObject == GameObject.Find("Attack"))
                {
                    BattleSystem.OnAttackButton();
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
}
