using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public List<GameObject> PartyMembers = new List<GameObject>(); //to hold the game objects
    public List<UnitInfo> PassMembers = new List<UnitInfo>(); // to get their info easier
    int PartyTurn;
    int EnemyTurnCount;
    public List<UnitInfo>EnemyMembers = new List<UnitInfo>();
    public int enemyCount;
    public int deadCounter = 0;


    public GameObject enemyPrefab;
    [SerializeField] GameObject ActionPanel;
    [SerializeField] GameObject EnemySelectionPanel;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public TurnState turnState;

    public UnitInfo playerInfo;
    public UnitInfo enemyInfo;

    public BattleHUD playerHUD;
    public BattleHUD[] battleHUD;
    public EnemyHPSlider enemyHUD;

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
        EnemyTurnCount = 0;
    }

    void Start()
    {
        MainCamera.enabled = true;
        turnState = TurnState.Start;
        Debug.Log(turnState);
        enemyCount = UnityEngine.Random.Range(1, 5);
        StartCoroutine(SetupBattle());
        
    }

    IEnumerator SetupBattle()
    {
        for (int index = 0; index < PartyMembers.Count; index++)
        {
            PassMembers[index] = CreatePartyMember(index);
            battleHUD[index] = CreatePartyHUDs(PassMembers[index]);
        }

        
        for (int i = 0; i < enemyCount; i++)
        {
            EnemyMembers.Add(CreateEnemies(i));
        }

        //GameObject enemyGo = Instantiate(enemyPrefab, enemyBattleStation);
        //enemyInfo = enemyGo.GetComponent<UnitInfo>();


        yield return new WaitForEndOfFrame();

        turnState = TurnState.PlayerTurn;
        PlayerTurn(); //Starts the player's turn
    }

    void PlayerTurn()
    {
        Debug.Log(turnState);
        EnemyTurnCount = 0;
        //some kind of switch case for party memembers here maybe
        if (PartyTurn < PassMembers.Count)
        {
            playerInfo = PassMembers[PartyTurn];
            if (PassMembers[0].currHP == 0)
            {
                turnState = TurnState.Lost;
                EndBattle();
            }
            else if (PassMembers[PartyTurn].currHP == 0 && PassMembers[0].currHP != 0)
            {
                PartyTurn++;
                PlayerTurn();
            }
            else
            {
                Debug.Log("Party Member " + PartyTurn);
                ActionPanel.SetActive(true); //Activates the player's command window
            }
        }
        else
        {
            turnState = TurnState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerAttack(int damage)
    {
        Debug.Log(turnState);
        PartyTurn++;
        //Damage with the base attack to enemy
        playerInfo.damage = damage;

        enemyHUD = enemyInfo.transform.GetComponent<EnemyHPSlider>();
        enemyHUD.SetSlider(enemyInfo);

        bool isDead = enemyInfo.TakeDamage(playerInfo.damage);

        //They updated enemy Hud here
        enemyHUD.SetSlider(enemyInfo);

        yield return new WaitForSeconds(1.0f);

        enemyHUD.DisableBar(); //to disable the bar

        //Check if the enemy is dead
        if (isDead)
        {
            deadCounter++;
            //End the battle if all is dead
            if (deadCounter == EnemyMembers.Count)
            {
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

    void EndBattle() //Not finished yet
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


    public int RandomPartyMember()
    {
        int random;
        random = UnityEngine.Random.Range(0, PassMembers.Count);
        return random;
    }

    public IEnumerator EnemyTurn()
    {
        //Attack name here
        PartyTurn = 0;
        Debug.Log(turnState);

        if (EnemyTurnCount < EnemyMembers.Count)
        {
            enemyInfo = EnemyMembers[EnemyTurnCount];
            if (EnemyMembers[EnemyTurnCount].currHP == 0)
            {
                EnemyTurnCount++;
                StartCoroutine(EnemyTurn());
            }
            else
            {
                Debug.Log("Enemy " + EnemyTurnCount + "'s Turn");
            }
        }
        else
        {
            turnState = TurnState.PlayerTurn;
            PlayerTurn();
        }


        //determining who to hit
        int Attacked = RandomPartyMember(); // randomly selects from the members availble to hit
        playerInfo = PassMembers[Attacked]; // sets who gets hit

        while (playerInfo.currHP == 0)
        {
            Attacked = RandomPartyMember();
            playerInfo = PassMembers[Attacked];
        }


        Debug.Log(playerInfo.name + " is Attacked!");

        yield return new WaitForSeconds(1.0f);


        int randomAction = UnityEngine.Random.Range(0, 100);
        if (randomAction >= 50)
        {
            //Normal Attack
            Debug.Log("Normal Attack");
            enemyInfo.damage = (int)Mathf.Sqrt(enemyInfo.strength);
            Debug.Log("Enemy's Damage: " + enemyInfo.damage);
        }
        else
        {
            //Skill Attack
            Debug.Log("Skill Attack");
            ActionSkills selectedSkill;
            if (enemyInfo.SkillList != null)//checks to see if enemy even has skills
            {
                int randomSkill = UnityEngine.Random.Range(0, enemyInfo.SkillList.Count); //selects random skill
                selectedSkill = enemyInfo.SkillList[randomSkill];
                if (SkillCostCheck(enemyInfo, selectedSkill) == true)
                {
                    if (selectedSkill.costType == ActionSkills.CostType.MP)
                    {
                        Debug.Log(selectedSkill.name + " Selected");
                        enemyInfo.currMP = enemyInfo.currMP - selectedSkill.cost;
                        enemyInfo.damage = (int)Mathf.Sqrt(selectedSkill.damageValue) * (int)Mathf.Sqrt(enemyInfo.magic);
                    }
                    else
                    {
                        Debug.Log(selectedSkill.name + " Selected");
                        enemyInfo.currHP = enemyInfo.currHP - Mathf.RoundToInt((enemyInfo.baseHP * selectedSkill.cost) / 100);
                        enemyInfo.damage = (int)Mathf.Sqrt(selectedSkill.damageValue) * (int)Mathf.Sqrt(enemyInfo.strength);
                    }
                }
                else
                {
                    enemyInfo.damage = (int)Mathf.Sqrt(enemyInfo.strength);
                    Debug.Log("Enemy's Damage: " + enemyInfo.damage);
                }
            }
            else
            {
                enemyInfo.damage = (int)Mathf.Sqrt(enemyInfo.strength);
                Debug.Log("Enemy's Damage: " + enemyInfo.damage);
            }

        }    
        bool isDead = playerInfo.TakeDamage(enemyInfo.damage);

        battleHUD[Attacked].SetHUD(playerInfo);
        

        yield return new WaitForSeconds(1.0f);

        if (isDead)
        {
            EnemyTurnCount++;
            if (PassMembers[0].currHP == 0)
            {
                turnState = TurnState.Lost;
                EndBattle();
            }
            else
            {
                if (EnemyTurnCount == enemyCount)
                {
                    turnState = TurnState.PlayerTurn;
                    PlayerTurn();
                }
                else
                {
                    StartCoroutine(EnemyTurn());
                }
            }
        }
        else
        {
            EnemyTurnCount++;
            if (EnemyTurnCount == enemyCount)
            {
                turnState = TurnState.PlayerTurn;
                PlayerTurn();
            }
            else
            {
                StartCoroutine(EnemyTurn());
            }
        }
    }


    //Function to pass turn to next party memeber
    public void TurnPass()
    {
        PartyTurn++;
        if (PartyTurn < PassMembers.Count)
        {
            ActionPanel.SetActive(false);
            PlayerTurn();
        }
        else
        {
            ActionPanel.SetActive(false);
            turnState = TurnState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    //Party Creation happens here
    UnitInfo CreatePartyMember(int i)
    {
        var SpawnIn = Instantiate(PartyMembers[i], playerBattleStation); ;
        var Member = SpawnIn.GetComponent<UnitInfo>();
        Member.name = "Member " + i.ToString() + ": " + SpawnIn.name;
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

    //Enemy party creation
    UnitInfo CreateEnemies(int i)
    {
        var SpawnIn = Instantiate(enemyPrefab, enemyBattleStation); ;
        var Member = SpawnIn.GetComponent<UnitInfo>();
        Member.name = "Enemy " + i.ToString();
        return Member;
    }


    /// <summary>
    /// The UI button functions section
    /// </summary>


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

    //Basic Skill Attack Button funciton
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

                playerInfo.currHP = playerInfo.currHP - Mathf.RoundToInt((playerInfo.baseHP * Skill.cost)/100);//subtract perentage from hp
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
    
    //checking is user can even use the skill
    public bool SkillCostCheck(UnitInfo user, ActionSkills Skill)
    {
        if (Skill.costType == ActionSkills.CostType.HP)
        {
            if (user.currHP - Mathf.RoundToInt((user.baseHP * Skill.cost) / 100) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (Skill.costType == ActionSkills.CostType.MP)
        {
            if (user.currMP - Skill.cost >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    
}