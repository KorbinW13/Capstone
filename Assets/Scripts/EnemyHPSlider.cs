using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPSlider : MonoBehaviour
{
    [SerializeField] public Slider hpSlider;
    public UnitInfo unitInfo;

    public float currentHealth;
    public bool isDone;

    public void Start()
    {
        currentHealth = unitInfo.currHP;
        hpSlider.maxValue = unitInfo.baseHP;
        hpSlider.value = currentHealth;
    }


    public void ChangeHealth(int amount, TypeReaction reaction)
    {
        isDone = false;
        if (reaction == TypeReaction.Drain)
        {
            currentHealth += amount;
        }
        else
        {
            currentHealth -= amount;
        }
        currentHealth = Mathf.Clamp(currentHealth, 0, hpSlider.maxValue);
        StartCoroutine(UpdateBar());
    }

    IEnumerator UpdateBar()
    {
        while (hpSlider.value != currentHealth)
        {
            if (hpSlider.value > currentHealth)
            {
                hpSlider.value--;
                yield return new WaitForSeconds(.05f);
            }
            else if (hpSlider.value < currentHealth)
            {
                hpSlider.value++;
                yield return new WaitForSeconds(.05f);
            }
        }
        isDone = true;
        yield return null;
    }

    public void EnableBar()
    {
        hpSlider.gameObject.SetActive(true);
    }

    public void DisableBar()
    {
        hpSlider.gameObject.SetActive(false);
    }
}
