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

    public Camera MainCamera;
    public Camera PlayerCamera;
    public Camera PlayerAtkCamera;

    public RectTransform PlayerHUDPanel;
    public GameObject PlayerHUDprefab;

    public List<GameObject> PartyMembers = new List<GameObject>();
    public List<UnitInfo> PassMembers = new List<UnitInfo>();
    int PartyTurn;


    public GameObject enemyPrefab;
    [SerializeField] GameObject ActionPanel;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public TurnState turnState;

    public UnitInfo playerInfo;
    public UnitInfo enemyInfo;

    public BattleHUD playerHUD;
    public BattleHUD[] battleHUD;
    //public BattleHUD enemyHUD;

    public enum MenuOptions //for the first menu selection
    {
        Attack,
        Skills,
        Items,
        Pass
    }

    private void Awake()
    {
        battleHUD = new BattleHUD[PartyMembers.Count];
        PartyTurn = 0;
    }

    void Start()
    {
        MainCamera.enabled = true;
        turnState = TurnState.Start;
        Debug.Log(turnState);
        StartCoroutine(SetupBattle());
        
    }

    IEnumerator SetupBattle()
    {
        for (int index = 0; index < PartyMembers.Count; index++)
        {
            PassMembers[index] = CreatePartyMember(index);
            battleHUD[index] = CreatePartyHUDs(PassMembers[index]);
        }
        //GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        //playerInfo = playerGO.GetComponent<UnitInfo>();

        GameObject enemyGo = Instantiate(enemyPrefab, enemyBattleStation);
        enemyInfo = enemyGo.GetComponent<UnitInfo>();

        //playerHUD.SetHUD(playerInfo);

        yield return new WaitForEndOfFrame();

        turnState = TurnState.PlayerTurn;
        PlayerTurn(); //Starts the player's turn
    }

    void PlayerTurn()
    {
        Debug.Log(turnState);

        //some kind of switch case for party memembers here maybe
        switch (PartyTurn)
        {
            case 0:
                playerInfo = PassMembers[0];

                ActionPanel.SetActive(true); //Activates the player's command window
                break;
            case 1:
                playerInfo = PassMembers[1];

                ActionPanel.SetActive(true); //Activates the player's command window
                break;
            case 2:
                playerInfo = PassMembers[2];

                ActionPanel.SetActive(true); //Activates the player's command window
                break;
            case 3:
                playerInfo = PassMembers[3];

                /*

                foreach (Camera child in PassMembers[3].transform)
                {
                    if (child.name == "Back Camera")
                    {
                        PlayerCamera = child;
                    }
                }
                MainCamera.enabled = false;
                PlayerCamera.enabled = true;

                foreach (Camera child in PassMembers[3].transform)
                {
                    if (child.name == "Front Camera")
                    {
                        PlayerAtkCamera = child;
                    }
                }
                PlayerAtkCamera.enabled = false;
                */


                ActionPanel.SetActive(true); //Activates the player's command window
                break;
        }


        
        //ActionPanel.SetActive(true); //Activates the player's command window
    }

    IEnumerator PlayerAttack(int damage)
    {
        Debug.Log(turnState);
        PartyTurn++;
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
            if (PartyTurn < PassMembers.Count)
            {
                PlayerTurn();
            }
            else
            {
                turnState = TurnState.EnemyTurn;
                StartCoroutine(EnemyTurn());
            }
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
        PartyTurn = 0;
        Debug.Log(turnState);


        int Attacked = UnityEngine.Random.Range(0, PassMembers.Count); // randomly selects from the members availble to hit
        playerInfo = PassMembers[Attacked]; // sets who gets hit
        Debug.Log("Member: " + Attacked + " getting attacked!");

        yield return new WaitForSeconds(1.0f);
        enemyInfo.damage = (int)Mathf.Sqrt(playerInfo.strength);
        Debug.Log("Enemy's Damage: " + enemyInfo.damage);
        bool isDead = playerInfo.TakeDamage(enemyInfo.damage);

        battleHUD[Attacked].SetHUD(playerInfo);

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

    //Party Creation happens here
    UnitInfo CreatePartyMember(int i)
    {
        var SpawnIn = Instantiate(PartyMembers[i], playerBattleStation); ;
        var Member = SpawnIn.GetComponent<UnitInfo>();
        Member.name = "Member: " + i.ToString();
        return Member;
    }

    BattleHUD CreatePartyHUDs(UnitInfo member)
    {
        var HUD = Instantiate(PlayerHUDprefab, PlayerHUDPanel);
        var PlayerHUD = HUD.GetComponent<BattleHUD>();
        PlayerHUD.name = member.name + "'s HUD";
        PlayerHUD.SetHUD(member);
        return PlayerHUD;
    }


    //Basic Attack Button function
    public void OnAttackButton()
    {
        Debug.Log(turnState);
        // to artifically change turn
        int damage = (int)Mathf.Sqrt(playerInfo.weaponPower / 2) * (int)Mathf.Sqrt(playerInfo.strength);
        if (turnState != TurnState.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerAttack(damage));
    }

    public void OnSkillButton(ActionSkills Skill)
    {
        int damage = 0;
        Debug.Log(turnState);

        //here will be skill damage version
        
        switch(Skill.costType)
        {
            case ActionSkills.CostType.MP:

                playerInfo.currMP = playerInfo.currMP - Skill.cost;
                battleHUD[PartyTurn].SetHUD(playerInfo);
                damage = (int)Mathf.Sqrt(Skill.damageValue) * (int)Mathf.Sqrt(playerInfo.magic);
                Debug.Log("Skill damage: " + damage);
                break;

            case ActionSkills.CostType.HP:

                playerInfo.currHP = playerInfo.currHP - Skill.cost;
                battleHUD[PartyTurn].SetHUD(playerInfo);
                damage = (int)Mathf.Sqrt(Skill.damageValue) * (int)Mathf.Sqrt(playerInfo.strength);
                Debug.Log("Skill damage: " + damage);
                break;
        }

        if (turnState != TurnState.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerAttack(damage));
    }
}
