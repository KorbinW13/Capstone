using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public BaseHero player;

    public enum TurnState
    {
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;


    void Start()
    {
        
    }

    void Update()
    {
        switch (currentState)
        {
            case (TurnState.ADDTOLIST):

                break;
            case (TurnState.WAITING):

                break;
            case (TurnState.SELECTING):

                break;
            case (TurnState.ACTION):

                break;
            case (TurnState.DEAD):

                break;
        }
    }


}
