using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class MenuButtonController : MonoBehaviour
{
    public int index;
    public int maxIndex;
    public InputSystem input;
    public Text PlayerName;

    [SerializeField] RectTransform rectTransform;

    public BattleStateMachine BattleSystem;

    void Awake()
    {
        input = new InputSystem();
        input.Enable();
    }

    void OnEnable()
    {
        PlayerName.text = BattleSystem.playerInfo.name;
        input.Enable();
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        MenuScroll();
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
            if (input.UI.Scroll.ReadValue<float>() > 0 || input.UI.ScrollUp.WasPressedThisFrame())
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
}
