using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelected : MonoBehaviour
{
    public GameObject EnemySelectionObj;
    public EnemySelection enemySelection;
    public UnitInfo unitInfo;
    public EnemyHPSlider HPBar;
    public int thisIndex;

    public BattleStateMachine BattleSystem;
    public AudioSource SystemAudio;
    public AudioClip Selected;
    bool PlayedSelected;


    public void OnEnable()
    {
        BattleSystem = GameObject.Find("Battle System").GetComponent<BattleStateMachine>();
        SystemAudio = BattleSystem.GetComponent<AudioSource>();
        EnemySelectionObj = GameObject.Find("EnemySelectPanel");
        enemySelection = EnemySelectionObj.GetComponent<EnemySelection>();
        thisIndex = this.gameObject.transform.GetSiblingIndex();
        HPBar = GetComponent<EnemyHPSlider>();

        if (enemySelection.index == thisIndex && SystemAudio.isPlaying)
        {
            PlayedSelected = true;
        }
    }

    public void OnDisable()
    {
        PlayedSelected = false;
        HPBar.DisableBar();
    }

    void Update()
    {
        enemySelected();
    }

    public GameObject SelectionPanel(GameObject Panel)
    {
        EnemySelectionObj = Panel;
        return EnemySelectionObj;
    }

    public void enemySelected()
    {
        if (enemySelection.index == thisIndex)
        {
            if (!PlayedSelected)
            {
                PlayedSelected = true;
                SystemAudio.PlayOneShot(Selected);
                HPBar.EnableBar();
            }
            HPBar.EnableBar();
        }
        else
        {
            PlayedSelected = false;
            HPBar.DisableBar();
        }
    }
}