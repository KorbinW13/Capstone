using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPSlider : MonoBehaviour
{
    public Slider hpSlider;
    public UnitInfo unitInfo;

    public void Start()
    {
        hpSlider.maxValue = unitInfo.baseHP;
        hpSlider.value = unitInfo.currHP;
    }

    public void SetSlider(UnitInfo unit)
    {
        hpSlider.gameObject.SetActive(true);
        hpSlider.maxValue = unit.baseHP;
        hpSlider.value = unit.currHP;
    }

    public void DisableBar()
    {
        hpSlider.gameObject.SetActive(false);
    }
}
