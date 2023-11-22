using AutoLayout3D;
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

public enum TypeReaction
{
    Normal,
    Weak,
    Resist,
    Null,
    Drain
}

public class BattleStateMachine : MonoBehaviour
{

    public Camera MainCamera;
    public Camera BackCamera;
    public Camera FrontCamera;

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

    DamageDisplay DamageDisplay;


    public EventDisplay eventDisplay;
    public GameObject BattleResultsScreen;
    public ActionSkills BasicAttack;

    public enum MenuOptions //for the first menu selection
    {
        Attack,
        Skills,
        Items,
        Pass
    }

    public TypeReaction Reaction;


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
        enemyCount = Random.Range(1, 5);
        StartCoroutine(SetupBattle());
        
    }

    IEnumerator SetupBattle()
    {
        for (int index = 0; index < PartyMembers.Count; index++)
        {
            PassMembers[index] = CreatePartyMember(index);
            battleHUD[index] = CreatePartyHUDs(PassMembers[index]);
            playerBattleStation.GetComponent<XAxisLayoutGroup3D>().UpdateLayout();
        }

        
        for (int i = 0; i < enemyCount; i++)
        {
            EnemyMembers.Add(CreateEnemies(i));
            enemyBattleStation.GetComponent<XAxisLayoutGroup3D>().UpdateLayout();
        }

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
            EnemyTurn();
        }
    }

    IEnumerator PlayerAttack(int damage)
    {
        Debug.Log("Player Attack");
        //Damage with the base attack to enemy
        playerInfo.damage = damage;

        eventDisplay.Enable(playerInfo.UnitName + " Attacked " + enemyInfo.UnitName);


        //Get the Enemy HP Bar and then Enable it
        enemyHUD = enemyInfo.transform.GetComponent<EnemyHPSlider>();
        DamageDisplay = enemyInfo.transform.GetComponent<DamageDisplay>();
        yield return new WaitForSeconds(1.0f);
        enemyHUD.EnableBar();

        bool isDead = enemyInfo.TakeDamage(playerInfo.damage, ReturnReaction(BasicAttack, enemyInfo));
        yield return new WaitForSeconds(1.0f);

        //update enemy Hud here
        DamageDisplay.EnableText();
        DamageDisplay.DamageText(damage, ReturnReaction(BasicAttack, enemyInfo));
        if (ReturnReaction(BasicAttack, enemyInfo) != TypeReaction.Null)
        {
            enemyHUD.ChangeHealth(damage / (int)Mathf.Sqrt(enemyInfo.endurance * 8 + enemyInfo.armorDefense), ReturnReaction(BasicAttack, enemyInfo));
            yield return new WaitWhile(() => enemyHUD.isDone == false);
        }
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
                NextTurn();
            }
            
        }
        else
        {
            //End Turn
            NextTurn();
        }
        // Change the state based on what happened
    }

    IEnumerator SkillAttack(ActionSkills skill, int damage)
    {
        Debug.Log("Skill Attack");
        //Damage with the base attack to enemy
        playerInfo.damage = DamageOnReaction(ReturnReaction(skill, enemyInfo), damage);

        eventDisplay.Enable(skill.skillName);


        //Get the Enemy HP Bar and then Enable it
        enemyHUD = enemyInfo.transform.GetComponent<EnemyHPSlider>();
        DamageDisplay = enemyInfo.transform.GetComponent<DamageDisplay>();
        yield return new WaitForSeconds(1.0f);
        enemyHUD.EnableBar();

        bool isDead = enemyInfo.TakeDamage(playerInfo.damage, ReturnReaction(skill, enemyInfo));
        yield return new WaitForSeconds(1.0f);

        //update enemy Hud here
        DamageDisplay.EnableText();
        DamageDisplay.DamageText(damage, ReturnReaction(skill, enemyInfo));
        if (ReturnReaction(skill, enemyInfo) != TypeReaction.Null)
        {
            enemyHUD.ChangeHealth(damage / (int)Mathf.Sqrt(enemyInfo.endurance * 8 + enemyInfo.armorDefense), ReturnReaction(skill, enemyInfo));
            yield return new WaitWhile(() => enemyHUD.isDone == false);
        }
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
                NextTurn();
            }

        }
        else
        {
            //End Turn
            NextTurn();
        }
        // Change the state based on what happened
    }


    void EndBattle() //Not finished yet
    {
        if(turnState == TurnState.Won) 
        {
            //Temp
            Debug.Log("Fight Over: Win");
            BattleResultsScreen.SetActive(true);
        }
        else if (turnState == TurnState.Lost)
        {
            //Temp
            Debug.Log("You lost");
            BattleResultsScreen.SetActive(true);
        }
    }


    public int RandomPartyMember()//calls for random party memeber for the enemy to attack
    {
        int random;
        random = Random.Range(0, PassMembers.Count - 1);
        return random;
    }

    public void EnemyTurn()
    {
        PartyTurn = 0;
        Debug.Log(turnState);

        if (EnemyTurnCount < EnemyMembers.Count)
        {
            enemyInfo = EnemyMembers[EnemyTurnCount];
            if (EnemyMembers[EnemyTurnCount].currHP == 0)
            {
                Debug.Log("Enemy: " + EnemyTurnCount + " Can't Attack!");
                EnemyTurnCount++;
                EnemyTurn();
            }
            else
            {
                Debug.Log("Enemy " + EnemyTurnCount + "'s Turn");
                StartCoroutine(EnemyActionSelection());
            }
        }
        else
        {
            turnState = TurnState.PlayerTurn;
            PlayerTurn();
        }
    }

    IEnumerator EnemyActionSelection()
    {

        //determining who to hit
        int Attacked = RandomPartyMember(); // randomly selects from the members availble to hit
        playerInfo = PassMembers[Attacked]; // sets who gets hit

        if (playerInfo.currHP == 0)
        {
            Debug.Log("Selected Dead Memember");
            while (playerInfo.currHP == 0)// finds a player who's not dead already
            {
                Attacked = RandomPartyMember();
                playerInfo = PassMembers[Attacked];
            }
        }


        Debug.Log(playerInfo.name + " is Attacked!");

        int randomAction = Random.Range(0, 100);
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
                int randomSkill = Random.Range(0, enemyInfo.SkillList.Count); //selects random skill
                selectedSkill = enemyInfo.SkillList[randomSkill];
                if (SkillCostCheck(enemyInfo, selectedSkill) == true)
                {
                    switch(selectedSkill.costType)
                    {
                        case ActionSkills.CostType.MP:
                            Debug.Log(selectedSkill.name + " Selected");
                            enemyInfo.currMP = enemyInfo.currMP - selectedSkill.cost;
                            enemyInfo.damage = (int)(Mathf.Sqrt(selectedSkill.damageValue) * Mathf.Sqrt(enemyInfo.magic));

                            break;
                        case ActionSkills.CostType.HP:
                            Debug.Log(selectedSkill.name + " Selected");
                            enemyInfo.currHP = enemyInfo.currHP - Mathf.RoundToInt((enemyInfo.baseHP * selectedSkill.cost) / 100);
                            enemyInfo.damage = (int)(Mathf.Sqrt(selectedSkill.damageValue) * Mathf.Sqrt(enemyInfo.strength));

                            break;
                    }
                }
                else
                {
                    enemyInfo.damage = (int)Mathf.Sqrt(enemyInfo.strength);
                    Debug.Log("Override to Normal Attack");
                    Debug.Log("Enemy's Damage: " + enemyInfo.damage);
                }
            }
            else
            {
                enemyInfo.damage = (int)Mathf.Sqrt(enemyInfo.strength);
                Debug.Log("Override to Normal Attack");
                Debug.Log("Enemy's Damage: " + enemyInfo.damage);
            }

        }
        eventDisplay.Enable(enemyInfo.UnitName + " Attacks " + playerInfo.UnitName);
        bool isDead = playerInfo.TakeDamage(enemyInfo.damage, ReturnReaction(BasicAttack, playerInfo));

        battleHUD[Attacked].SetHUD(playerInfo);


        yield return new WaitForSeconds(1.0f);

        if (isDead)
        {
            if (PassMembers[0].currHP == 0)
            {
                turnState = TurnState.Lost;
                EndBattle();
            }
            else
            {
                NextTurn();
            }
        }
        else
        {

            NextTurn();
        }
    }


    //Function to easily determine next turn
    public void NextTurn()
    {
        if (turnState == TurnState.PlayerTurn)
        {
            PartyTurn++;
            if (PartyTurn < PassMembers.Count)
            {
                PlayerTurn();
            }
            else
            {
                turnState = TurnState.EnemyTurn;
                EnemyTurn();
            }
        }
        else if (turnState == TurnState.EnemyTurn)
        {
            EnemyTurnCount++;
            if (EnemyTurnCount < EnemyMembers.Count)
            {
                EnemyTurn();
            }
            else
            {
                turnState = TurnState.PlayerTurn;
                PlayerTurn();
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
            EnemyTurn();
        }
    }

    //Party Creation happens here
    UnitInfo CreatePartyMember(int i)
    {
        var SpawnIn = Instantiate(PartyMembers[i], playerBattleStation); ;
        var Member = SpawnIn.GetComponent<UnitInfo>();
        Member.name = "Member " + (i+1).ToString() + ": " + Member.UnitName;
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
        
        int damage = (int)Mathf.Sqrt(playerInfo.weaponPower / 2) * (int)Mathf.Sqrt(playerInfo.strength);
        // to artifically change turn
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

        StartCoroutine(SkillAttack(Skill,damage));
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


    //making for later
    public bool AccuracyCheck(UnitInfo user, UnitInfo target, ActionSkills skill)
    {
        int Accuracy = Random.Range(0, 100);
        if (Accuracy >= target.agility)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    //This is how Elemental Type Reactions work
    public TypeReaction ReturnReaction(ActionSkills skill, UnitInfo target)
    {
        if (target.typeWeak.HasFlag(skill.damageType))
        {
            Debug.Log("Weak");
            return TypeReaction.Weak;
        }
        else if (target.typeResist.HasFlag(skill.damageType))
        {
            Debug.Log("Resist");
            return TypeReaction.Resist;
        }
        else if (target.typeNull.HasFlag(skill.damageType))
        {
            Debug.Log("Null");
            return TypeReaction.Null;
        }
        else if (target.typeDrain.HasFlag(skill.damageType))
        {
            Debug.Log("Drain");
            return TypeReaction.Drain;
        }
        else
        {
            return TypeReaction.Normal;
        }
    }

    public int DamageOnReaction(TypeReaction reaction, int damage)
    {
        switch (reaction)
        {
            default: //Normal I think
                return damage;
            case TypeReaction.Weak:
                damage = damage * 2;
                return damage;
            case TypeReaction.Resist:
                damage = damage / 2;
                return damage;
            case TypeReaction.Null:
                damage = 0;
                return damage;
            case TypeReaction.Drain:
                return damage;
        }
    }
}