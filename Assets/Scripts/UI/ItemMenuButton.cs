using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuButton : MonoBehaviour
{
    public ItemMenuController menuController;
    public RectTransform m_Rect;
    public Animator animator;
    public int thisIndex;
    InputSystem input;
    [SerializeField] GameObject PrevPanel;
    [SerializeField] GameObject ParentPanel;

    void Start()
    {
        m_Rect = GetComponent<RectTransform>();
    }

    void Awake()
    {
        input = new InputSystem();
        input.Enable();
    }

    void Update()
    {
        input.Enable();
        if (RectTransformUtility.RectangleContainsScreenPoint(m_Rect, Input.mousePosition))
        {
            menuController.index = thisIndex;
        }

        if (menuController.index == thisIndex)
        {
            animator.SetBool("Selected", true);
            if (input.UI.PrimaryAction.WasPressedThisFrame())
            {
                animator.SetBool("Pressed", true); //select player with selected item here

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

        if (PrevPanel != null & input.UI.Back.WasPressedThisFrame())
        {
            PrevPanel.SetActive(true);
            ParentPanel.SetActive(false);
        }
    }
}
