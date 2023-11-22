using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text hpText;
    public Text darkHpText;
    public Text mpText;
    public Text darkMpText;

    public Slider hpSlider;
    public Slider mpSlider;

    public void SetHUD(UnitInfo unit)
    {
        HPandMPText(unit);
        hpSlider.maxValue = unit.baseHP;
        mpSlider.maxValue = unit.baseMP;
        hpSlider.value = unit.currHP;
        mpSlider.value = unit.currMP;
    }

    void HPandMPText(UnitInfo unit)
    {
        //for HP
        if (unit.currHP == 0) { hpText.text = ""; darkHpText.text = "000"; }
        else if (unit.currHP < 10)
        {
            hpText.text = unit.currHP.ToString(); darkHpText.text = "00";
        }
        else if (unit.currHP < 100) { hpText.text = unit.currHP.ToString(); darkHpText.text = "0"; }
        else { hpText.text = unit.currHP.ToString(); darkHpText.text=""; }

        //for MP
        if (unit.currMP == 0) { mpText.text = ""; darkMpText.text = "000"; }
        else if (unit.currMP < 10)
        {
            mpText.text = unit.currMP.ToString(); darkMpText.text = "00";
        }
        else if (unit.currMP < 100) { mpText.text = unit.currMP.ToString(); darkMpText.text = "0"; }
        else { mpText.text = unit.currMP.ToString(); darkMpText.text = ""; }
    }
}
