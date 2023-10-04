using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TurnState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Won,
    Lost
}

public class BattleStateMachine : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    [SerializeField] GameObject ActionPanel;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public TurnState turnState;
    UnitInfo playerInfo;
    UnitInfo enemyInfo;

    public BattleHUD playerHUD;
    //public BattleHUD enemyHUD;


    void Start()
    {
        turnState = TurnState.Start;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerInfo = playerGO.GetComponent<UnitInfo>();

        GameObject enemyGo = Instantiate(enemyPrefab, enemyBattleStation);
        enemyInfo = enemyGo.GetComponent<UnitInfo>();

        playerHUD.SetHUD(playerInfo);

        yield return new WaitForEndOfFrame();

        turnState = TurnState.PlayerTurn;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        ActionPanel.SetActive(true);
    }

    IEnumerator PlayerAttack()
    {
        //Damage the enemy
        playerInfo.damage = (int)Mathf.Sqrt(playerInfo.strength);
        bool isDead = enemyInfo.TakeDamage(playerInfo.damage);

        //They updated enemy Hud here but i wont to make it more diffcult battle

        yield return new WaitForEndOfFrame();

        //Check if the enemy is dead
        if (isDead)
        {
            //End the battle
            turnState = TurnState.Won;
            EndBattle();
        }
        else
        {
            //End Turn
            turnState = TurnState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
        // Change the state based on what happened
    }

    void EndBattle() //Not finished
    {
        if(turnState == TurnState.Won) 
        {
            //Temp
            Debug.Log("Fight Over");
        }
        else if (turnState == TurnState.Lost)
        {
            //Temp
            Debug.Log("You lost");
        }
    }

    IEnumerator EnemyTurn()
    {
        //Attack name here

        yield return new WaitForEndOfFrame();
        enemyInfo.damage = (int)Mathf.Sqrt(playerInfo.strength);
        bool isDead = playerInfo.TakeDamage(enemyInfo.damage);

        playerHUD.SetHUD(playerInfo);

        yield return new WaitForEndOfFrame();

        if (isDead)
        {
            turnState = TurnState.Lost;
            EndBattle();
        }
        else
        {
            turnState = TurnState.PlayerTurn;
            PlayerTurn();
        }
    }

    public void OnAttackButton()
    {
        if(turnState != TurnState.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }

    void Update()
    {
        Debug.Log(turnState);
    }
}
