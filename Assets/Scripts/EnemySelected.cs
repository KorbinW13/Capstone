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

    public void OnEnable()
    {
        EnemySelectionObj = GameObject.Find("EnemySelectPanel");
        enemySelection = EnemySelectionObj.GetComponent<EnemySelection>();
        thisIndex = this.gameObject.transform.GetSiblingIndex();
        HPBar = GetComponent<EnemyHPSlider>();
    }

    public void OnDisable()
    {
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
            HPBar.SetSlider(unitInfo);
        }
        else
        {
            HPBar.DisableBar();
        }
    }
}