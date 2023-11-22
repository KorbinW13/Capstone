using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static BattleStateMachine;

public class MenuButton : MonoBehaviour
{
    MenuButtonController menuButtonController;
    GameObject BattleObject;
    BattleStateMachine BattleSystem;
    public EnemySelection enemySelection;
    public RectTransform m_Rect;
    public Animator animator;
    public int thisIndex;
    InputSystem input;
    [SerializeField] GameObject menuPanelToOpen;
    GameObject ParentPanel;

    //UI sound
    public AudioSource SystemAudio;
    public AudioClip UISelected;
    public AudioClip UIConfrimed;
    bool PlayedSelected;
    bool PlayedConfirmed;

    public MenuOptions menuOptions;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (menuButtonController.index == thisIndex && !SystemAudio.isPlaying)
        {
            PlayedSelected = true;
        }
    }

    void Awake()
    {
        menuButtonController = transform.parent.gameObject.GetComponent<MenuButtonController>();
        BattleObject = GameObject.Find("Battle System");
        BattleSystem = BattleObject.GetComponent<BattleStateMachine>();
        SystemAudio = BattleObject.GetComponent<AudioSource>();

        input = new InputSystem();
        input.Enable();
    }

    private void OnEnable()
    {
        input.Enable();
        ParentPanel = transform.parent.gameObject.transform.parent.gameObject; //to get the parent of the parent object called ActionSelectorMenu
        PlayedSelected = false;
        PlayedConfirmed = false;
        if (menuButtonController.index == thisIndex && SystemAudio.isPlaying)
        {
            PlayedSelected = true;
        }
    }

    void Update()
    {
        input.Enable();
        ButtonFunction();
        
    }

    void ButtonFunction()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(m_Rect, input.UI.MousePosition.ReadValue<Vector2>()))
        {
            menuButtonController.index = thisIndex;
        }

        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("Selected", true);

            if (!PlayedSelected)
            {
                PlayedSelected = true;
                SystemAudio.PlayOneShot(UISelected);
            }


            if (input.UI.PrimaryAction.WasPressedThisFrame())
            {
                animator.SetBool("Pressed", true);

                if (!PlayedConfirmed)
                {
                    PlayedConfirmed = true;
                    SystemAudio.PlayOneShot(UIConfrimed);
                    switch (menuOptions)
                    {
                        case MenuOptions.Attack:
                            input.Disable();
                            menuButtonController.input.Disable();

                            enemySelection.PrevPanel = ParentPanel;
                            enemySelection.gameObject.SetActive(true);

                            Invoke("DisablePanel", 0.1f);

                            break;
                        case MenuOptions.Skills:
                            if (menuPanelToOpen != null)
                            {
                                menuButtonController.input.Disable();
                                input.Disable();
                                Invoke("DisablePanel", 0.1f);
                            }
                            else
                            {
                                return;
                            }
                            break;
                        case MenuOptions.Items:
                            if (menuPanelToOpen != null)
                            {
                                menuButtonController.input.Disable();
                                input.Disable();
                                Invoke("DisablePanel", 0.1f);
                            }
                            else
                            {
                                return;
                            }
                            break;
                        case MenuOptions.Pass:
                            menuButtonController.input.Disable();
                            BattleSystem.TurnPass();
                            break;
                    }
                }

            }
            else if (animator.GetBool("Pressed"))
            {
                animator.SetBool("Pressed", false);
                PlayedConfirmed = false;
            }
        }
        else
        {
            animator.SetBool("Selected", false);
            PlayedSelected = false;
        }
    }

    public void GetSoundEffect(AudioClip clip, bool boolian)
    {
        if (!boolian && !SystemAudio.isPlaying)
        {
            boolian = true;
            SystemAudio.PlayOneShot(clip);
        }
    }

    void DisablePanel()
    {
        input.Disable();
        ParentPanel.SetActive(false);
        if (menuPanelToOpen != null)
        {
            menuPanelToOpen.SetActive(true);
        }
    }
}
