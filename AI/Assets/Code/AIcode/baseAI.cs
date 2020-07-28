using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseAI : MonoBehaviour
{
    [Header("Base Values")]

    public string team;
    public float baseHealth;
    public float ironStored;
    public float foodStored;
    public float netStored;
    public float recourceRegen;
    public GameObject spawnPoint;
    public List<Miner> minerList;
    public List<Soldier> soldierList;

    [Space(2)]
    [Header("Prefab References")]
    public GameObject miner;
    public GameObject soldier;



    public List<OreNode> ironNodesList;
    public List<OreNode> foodNodesList;
    public List<GameObject> huntList;
    public List<GameObject> guardList;
    public GameObject enemyBase;
    public Graph graphReference;


    [Space(2)]
    [Header("Utility Values")]

    public bool defaultBiases;
    public bool agressiveBiases;
    public bool defensiveBiases;
    public bool randomizedBiases;

    [Space(4)]

    public float milDif; // 1- 100
    public float econDif; //1-100
    public float defensiveBias; // 1-100
    public float agressiveBias; // 1- 100
    public float economyBias; // 1- 100
    public float militaryBias; // 1- 100


    public string currentSpendChoice;
    public string currentOreChoice;
    public string currentSoldierStratagyChoice;
    private float foodNeeded;   // recources not yet attained for next purchase
    private float ironNeeded;   // recources not yet attained for next purchase

    private float soldierIronCost = 30;
    private float soldierFoodCost = 25;
    private float minerFoodCost = 20;
    private float minerIronCost = 15;

    private float soldierUpgradeCost = 60;
    private float minerUpgradeCost =  40;

    private float minerCap = 4;
    private float SoldierCap = 5;


    private float spawnSoldierDesireability;
    private float spawnMinerDesireability;
    private float upgradeMinerDesireability;
    private float upgradeSoldierDesdireability;

    private float ironDesirability;
    private float foodDesirability;

    private float huntDesireability;
    private float guardDesireabiity;
    private float attackBaseDesireability;

    [Space(2)]
    [Header("unit stats")] // units stats to spawn new units with 

    public float soldierAttackDamage;   // add 5
    public float soldierSpeed;    // add 1
    public float soldierMaxHealth; // add 10

    public float minerGatherRate; // add 1 
    public float minerSpeed; // add 1
    public float minerMaxHealth; // add 10

 


    // Start is called before the first frame update
    void Start()
    {
        netStored = foodStored + ironStored;


        soldierAttackDamage = soldier.GetComponent<Soldier>().attackDamage;   // add 5
        soldierSpeed = soldier.GetComponent<Soldier>().speed;
        soldierMaxHealth = soldier.GetComponent<Soldier>().maxHealth;

        minerGatherRate = miner.GetComponent<Miner>().gatherRate;
        minerSpeed = miner.GetComponent<Miner>().speed;
        minerMaxHealth = miner.GetComponent<Miner>().maxHealth;




        if(defaultBiases == true)
        {
            defensiveBias = 50;
            agressiveBias = 80;
            economyBias = 80;
            militaryBias = 60;
        }
        if (agressiveBiases == true)
        {
            defensiveBias = 30;
            agressiveBias = 100;
            economyBias = 40;
            militaryBias = 80;
        }
        if (defensiveBiases == true)
        {
            defensiveBias = 80;
            agressiveBias = 40;
            economyBias = 80;
            militaryBias = 55;
        }
        if (randomizedBiases == true)
        {
            defensiveBias = Random.Range(1,100);
            agressiveBias = Random.Range(1, 100);
            economyBias = Random.Range(1, 100);
            militaryBias = Random.Range(1, 100);
        }

    }

    // Update is called once per frame
    void Update()
    {
        ironStored += (recourceRegen * Time.deltaTime);
        foodStored += (recourceRegen * Time.deltaTime);
        netStored += ((recourceRegen * Time.deltaTime) * 2);

        if (Input.GetKeyDown(KeyCode.H))
        {
            getMinerDesireability();
            getSoldierDesireability();
            getMinerUpgradeDesireability();
            getSoldierUpgradeDesireability();
        }

        getSoldierStratagyDecision();

        getPurchaseDecision();

        if(currentSpendChoice == "miner")
        {
            if(minerIronCost < ironStored && minerFoodCost < foodStored)
            {
                ironStored -= minerIronCost;
                foodStored -= minerFoodCost;


                spawnMiner();
                foodNeeded = 0;
                ironNeeded = 0;
            }
            else
            {
                foodNeeded = minerFoodCost - foodStored;
                ironNeeded = minerIronCost - ironStored;
            }

        }
        else if (currentSpendChoice == "soldier")
        {
            if (soldierIronCost < ironStored && soldierFoodCost < foodStored)
            {
                ironStored -= soldierIronCost;
                foodStored -= soldierFoodCost;


                spawnSoldier();
                foodNeeded = 0;
                ironNeeded = 0;
            }
            else
            {
                foodNeeded = soldierFoodCost - foodStored;
                ironNeeded = soldierIronCost - ironStored;
            }
        }
        else if (currentSpendChoice == "upgradeMiner")
        {
            if (minerUpgradeCost < ironStored )
            {
                ironStored -= minerUpgradeCost;

                minerGatherRate += 0.5f;
                minerSpeed += 0.2f;
                minerMaxHealth += 10;
                pushUpgrades();
            }
            else
            {               
                ironNeeded = minerUpgradeCost - ironStored;
            }
        }
        else if (currentSpendChoice == "upgradeSoldier")
        {
            if (soldierUpgradeCost < ironStored)
            {
                ironStored -= soldierUpgradeCost;

                soldierAttackDamage += 8;
                soldierSpeed += 0.2f;
                soldierMaxHealth += 15;
                pushUpgrades();
            }
            else
            {
                ironNeeded = minerUpgradeCost - ironStored;
            }
        }


        getOreDecision();

        if(currentOreChoice == "food")
        {
            foreach (Miner miner in minerList)
            {               
                miner.TargetNodeType = "food";
            }
        }
        else
        {
            foreach (Miner miner in minerList)
            {
                miner.TargetNodeType = "iron";
            }
        }


        




    }

        




    public void spawnMiner()
    {
        GameObject newMiner = Instantiate(miner, spawnPoint.transform.position, spawnPoint.transform.rotation);

        newMiner.GetComponent<Miner>().baseAI = gameObject.AddComponent<baseAI>();
        newMiner.GetComponent<Miner>().graphReference = graphReference;
        newMiner.GetComponent<Miner>().ironNodesList = ironNodesList;
        newMiner.GetComponent<Miner>().foodNodesList = foodNodesList;
        minerList.Add(newMiner.GetComponent<Miner>());

    }

    public void spawnSoldier()
    {
        GameObject newSoldier = Instantiate(soldier, spawnPoint.transform.position, spawnPoint.transform.rotation);

        newSoldier.GetComponent<Soldier>().baseAI = gameObject.AddComponent<baseAI>();
        newSoldier.GetComponent<Soldier>().graphReference = graphReference;
        newSoldier.GetComponent<Soldier>().enemyBaseAI = enemyBase.GetComponent<baseAI>();

        newSoldier.GetComponent<Soldier>().attackDamage = soldierAttackDamage;    // up[date relevent stats to latest upgraded values
        newSoldier.GetComponent<Soldier>().speed = soldierSpeed;
        newSoldier.GetComponent<Soldier>().maxHealth = soldierMaxHealth;
        newSoldier.GetComponent<Soldier>().health = soldierMaxHealth;

        newSoldier.GetComponent<Soldier>().huntAreas = huntList;
        newSoldier.GetComponent<Soldier>().guardAreas = guardList;

        newSoldier.GetComponent<Soldier>().stateManager = currentSoldierStratagyChoice;

        soldierList.Add(newSoldier.GetComponent<Soldier>());
    }

    public void pushUpgrades()
    {
        foreach(Miner miner in minerList)
        {
            miner.maxHealth = minerMaxHealth;
            miner.speed = minerSpeed;
            miner.gatherRate = minerGatherRate;
        }

        foreach (Soldier soldier in soldierList)
        {
            soldier.maxHealth = soldierMaxHealth;
            soldier.speed = soldierSpeed;
            soldier.attackDamage = soldierAttackDamage;
        }

    }



    public void getPurchaseDecision()
    {
        getMinerDesireability();
        getSoldierDesireability();
        getMinerUpgradeDesireability();
        getSoldierUpgradeDesireability();

        currentSpendChoice = "miner";
        if(spawnSoldierDesireability > spawnMinerDesireability)
        {
            currentSpendChoice = "soldier";

            if (upgradeMinerDesireability > spawnSoldierDesireability)
            {
                currentSpendChoice = "upgradeMiner";


                if (upgradeSoldierDesdireability > upgradeMinerDesireability)
                {
                    currentSpendChoice = "upgradeSoldier";
                }
            }
        }
      

    }
    public void getOreDecision()
    {
        getIronDesirability();
        getFoodDesirability();
       

        currentOreChoice = "iron";
        if (foodDesirability > ironDesirability)
        {
            currentOreChoice = "food";
        }
       

    }


    public void getSoldierStratagyDecision()
    {
        getHuntDesirability();
        getGuardDesirability();
        getAttackBaseDesirability();

        currentSoldierStratagyChoice = "hunt";
        if(guardDesireabiity > huntDesireability)
        {
            currentSoldierStratagyChoice = "guard";

            if (attackBaseDesireability > guardDesireabiity)
            {
                currentSoldierStratagyChoice = "attackBase";

                foreach(Soldier soldier in soldierList)
                {
                    soldier.stateManager = "attackBase";
                }
            }


        }
        

    }


    public void getHuntDesirability()
    {
        int existingHunters = 0;

        foreach (Soldier soldier in soldierList)
        {
            if(soldier.stateManager == "hunt")
            {
                existingHunters += 1;
            }
        }

        if(existingHunters >= 1)
        {
            huntDesireability = (agressiveBias / existingHunters);
        }
        else
        {
            huntDesireability = (agressiveBias);
        }
      
    }

    public void getGuardDesirability()
    {
        int existingGuarders = 0;

        foreach (Soldier soldier in soldierList)
        {
            if (soldier.stateManager == "guard")
            {
                existingGuarders += 1;
            }
        }

        if (existingGuarders >= 1)
        {
            huntDesireability = (guardDesireabiity / existingGuarders);
        }
        else
        {
            huntDesireability = (guardDesireabiity);
        }

    }



    public void getAttackBaseDesirability()
    {
        attackBaseDesireability = (agressiveBias / enemyBase.GetComponent<baseAI>().soldierList.Count);
    }


    public void getMinerDesireability()
    {

        float totalUnitCost = minerFoodCost + minerIronCost;

        float existingUnitsOfType = 1;
        if (minerList.Count >= 1)
        {
            existingUnitsOfType = minerList.Count;
        }
  


        spawnMinerDesireability = ((agressiveBias + economyBias - totalUnitCost) / existingUnitsOfType);

      

        //desireability = Mathf.Clamp(desireability, 0.0f, 10.0f);
     


    }

    public void getSoldierDesireability()
    {
        float totalUnitCost = soldierFoodCost + soldierIronCost;
        float existingUnitsOfType = 1;
        if (soldierList.Count >= 1)
        {
            existingUnitsOfType = soldierList.Count;
        }


        spawnSoldierDesireability = (  (agressiveBias + militaryBias - totalUnitCost) / existingUnitsOfType);

     //   spawnSoldierDesireability  = Mathf.Clamp(spawnSoldierDesireability, 0.0f, 100.0f);


    }

    public void getMinerUpgradeDesireability()
    {

        float totalUpgradeCost = minerUpgradeCost;


        upgradeMinerDesireability = ( (defensiveBias + economyBias - minerUpgradeCost) * minerList.Count );
 
    


    }


    public void getSoldierUpgradeDesireability()
    {
        float totalUpgradeCost = soldierUpgradeCost;


        upgradeMinerDesireability = ((defensiveBias + militaryBias - minerUpgradeCost) * soldierList.Count);

    }



    public void getIronDesirability()
    {

        ironDesirability = (ironNeeded);
    }


    public void getFoodDesirability()
    {
        foodDesirability = (foodNeeded);
    }


}
