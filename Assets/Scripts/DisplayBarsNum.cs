using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBarsNum : MonoBehaviour
{
    public int HP;
    public int MP;
    public int MaxHP;
    public int MaxMP;

    public Text hpText;
    public Text darkHpText;
    public Text mpText;
    public Text darkMpText;

    public Slider hpSlider;
    public Slider mpSlider;

    public void SetHUD(int HP, int MP, int MaxHP, int MaxMP)
    {
        HPandMPText(HP, MP);
        hpSlider.maxValue = MaxHP;
        mpSlider.maxValue = MaxMP;
        hpSlider.value = HP;
        mpSlider.value = MP;
    }

    void Update()
    {
        SetHUD(HP, MP, MaxHP, MaxMP);
    }

    void HPandMPText(int HP, int MP)
    {
        //for HP
        if (HP == 0) { hpText.text = ""; darkHpText.text = "000"; }
        else if (HP < 10)
        {
            hpText.text = HP.ToString(); darkHpText.text = "00";
        }
        else if (HP < 100) { hpText.text = HP.ToString(); darkHpText.text = "0"; }
        else { hpText.text = HP.ToString(); darkHpText.text=""; }

        //for MP
        if (MP == 0) { mpText.text = ""; darkMpText.text = "000"; }
        else if (MP < 10)
        {
            mpText.text = MP.ToString(); darkMpText.text = "00";
        }
        else if (MP < 100) { mpText.text = MP.ToString(); darkMpText.text = "0"; }
        else { mpText.text = MP.ToString(); darkMpText.text = ""; }
    }
}
