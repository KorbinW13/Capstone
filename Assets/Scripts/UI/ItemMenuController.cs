using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuController : MonoBehaviour
{
    BattleStateMachine BattleSystem;
    public int index;
    public int maxIndex;
    InputSystem input;
    [SerializeField] RectTransform rectTransform;

    [SerializeField] GameObject PrevPanel;
    [SerializeField] GameObject ParentPanel;

    public AudioSource SystemAudio;
    public AudioClip UIBack;

    void Awake()
    {
        BattleSystem = GameObject.Find("Battle System").GetComponent<BattleStateMachine>();

        input = new InputSystem();
        input.Enable();
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        MouseScroll();

        if (PrevPanel != null & input.UI.Back.WasPressedThisFrame())
        {
            SystemAudio.PlayOneShot(UIBack);
            PrevPanel.SetActive(true);
            ParentPanel.SetActive(false);
        }
    }

    void MouseScroll()
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
