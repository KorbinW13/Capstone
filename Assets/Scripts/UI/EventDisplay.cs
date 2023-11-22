using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EventDisplay : MonoBehaviour
{
    public GameObject display;
    public TMP_Text Text;


    public void Enable(string message)
    {
        display.SetActive(true);
        StartCoroutine(DisplayMessage(message));
    }



    IEnumerator DisplayMessage(string message)
    {
        Text.SetText(message);
        yield return new WaitForSeconds(1f);
        display.SetActive(false);
    }


}
