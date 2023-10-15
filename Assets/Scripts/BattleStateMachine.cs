using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleStateMachine;


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
    public UnitInfo playerInfo;
    public UnitInfo enemyInfo;

    public BattleHUD playerHUD;
    //public BattleHUD enemyHUD;

    public enum MenuOptions //for the first menu selection
    {
        Attack,
        Skills,
        Items,
        Pass
    }

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

    IEnumerator PlayerAttack(int damage)
    {
        //Damage the with base attack enemy
        playerInfo.damage = damage;

        bool isDead = enemyInfo.TakeDamage(playerInfo.damage);

        //They updated enemy Hud here but i wont to make it more diffcult battle

        yield return new WaitForSeconds(1.0f);

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

    public IEnumerator EnemyTurn()
    {
        //Attack name here

        yield return new WaitForSeconds(1.0f);
        enemyInfo.damage = (int)Mathf.Sqrt(playerInfo.strength);
        bool isDead = playerInfo.TakeDamage(enemyInfo.damage);

        playerHUD.SetHUD(playerInfo);

        yield return new WaitForSeconds(1.0f);

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
        int damage = (int)Mathf.Sqrt(playerInfo.weaponPower / 2) * (int)Mathf.Sqrt(playerInfo.strength);
        if (turnState != TurnState.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerAttack(damage));
    }

    public void OnSkillButton(int SkillPower, Enum DamageType, bool CostMP, int Cost)
    {
        //here will be skill damage version
        switch(CostMP)
        {
            case true:

                playerInfo.currMP = playerInfo.currMP - Cost;
                SkillPower = (int)Mathf.Sqrt(SkillPower) * (int)Mathf.Sqrt(playerInfo.magic);

                break;
            case false:

                playerInfo.currHP = playerInfo.currHP - Cost;
                SkillPower = (int)Mathf.Sqrt(SkillPower) * (int)Mathf.Sqrt(playerInfo.strength);

                break;
        }


        SkillPower = (int)Mathf.Sqrt(SkillPower)*(int)Mathf.Sqrt(playerInfo.magic);

        if (turnState != TurnState.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerAttack(SkillPower));
    }

    void Update()
    {
        Debug.Log(turnState);
    }
}
