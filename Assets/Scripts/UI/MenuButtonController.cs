using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class MenuButtonController : MonoBehaviour
{
    public int index;
    public int maxIndex;
    InputSystem input;
    [SerializeField] RectTransform rectTransform;

    //GameObject BattleSystem = GameObject.Find("Battle System");

    void Awake()
    {
        input = new InputSystem();
        input.Enable();
    }

    void OnEnable()
    {
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
        if (input.UI.Scroll.ReadValue<float>() != 0)
        {
            if (input.UI.Scroll.ReadValue<float>() < 0)
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
            else if (input.UI.Scroll.ReadValue<float>() > 0)
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
