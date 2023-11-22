using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageDisplay : MonoBehaviour
{
    public TMP_Text Text;

    public void DamageText(int damage, TypeReaction reaction)
    {

        StartCoroutine(UpdateText(damage, reaction));
    }

    IEnumerator UpdateText(int damage, TypeReaction reaction)
    {
        switch(reaction)
        {
            default:
                Text.color = Color.white;
                Text.SetText(damage.ToString());
                break;
            case TypeReaction.Weak:
                Text.color = Color.red;
                Text.SetText("Weak");
                yield return new WaitForSeconds(.4f);
                Text.SetText(damage.ToString());
                break;
            case TypeReaction.Resist:
                Text.color = Color.red;
                Text.SetText("Resist");
                yield return new WaitForSeconds(.4f);
                Text.SetText(damage.ToString());
                break;
            case TypeReaction.Null:
                Text.color = Color.black;
                Text.SetText("Null");
                break;
            case TypeReaction.Drain:
                Text.color = Color.green;
                Text.SetText("Drain");
                yield return new WaitForSeconds(.4f);
                Text.SetText(damage.ToString());
                break;
        }
        yield return new WaitForSeconds(1f);
        DisableText();
        yield return null;
    }

    public void EnableText()
    {
        Text.gameObject.SetActive(true);
    }

    public void DisableText()
    {
        Text.gameObject.SetActive(false);
    }
}
