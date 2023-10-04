using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuController : MonoBehaviour
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
                    if (index > 1 && index < maxIndex)
                    {
                        rectTransform.offsetMax -= new Vector2(0, -12);
                    }
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
                    if (index < maxIndex - 1 && index > 0)
                    {
                        rectTransform.offsetMax -= new Vector2(0, 12);
                    }
                }
                else
                {
                    index = maxIndex;
                    rectTransform.offsetMax = new Vector2(0, (maxIndex - 2) * 12);
                }
            }
        }
    }
}
