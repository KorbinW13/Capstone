using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkMenuController : MonoBehaviour
{
    public int index;
    public int maxIndex;
    InputSystem input;
    [SerializeField] RectTransform rectTransform;

    void Awake()
    {
        input = new InputSystem();
        input.Enable();
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
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
