using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleResults : MonoBehaviour
{
    public BattleStateMachine BattleSystem;
    public TMP_Text ResultText;



    public void OnEnable()
    {
        switch (BattleSystem.turnState)
        {
            case TurnState.Won:
                ResultText.SetText("You Win");
                break;
            case TurnState.Lost:
                ResultText.SetText("You Lose");
                break;

        }
    }
}
