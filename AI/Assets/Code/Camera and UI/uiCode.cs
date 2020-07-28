using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiCode : MonoBehaviour
{

    public baseAI teamBase;

    public Text baseHealthText;
    public Text baseIronCount;
    public Text baseFoodCount;

    public Text baseMinerCount;
    public Text baseSoldierCount;

    public Text basePurchaseChoice;
    public Text baseOreChoice;
    public Text baseSoldierChoice;


   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        baseHealthText.text = "Base Health - " + teamBase.baseHealth;
        baseIronCount.text = "Iron - " + teamBase.ironStored;
        baseFoodCount.text = "Food - " + teamBase.foodStored;

        baseMinerCount.text = "Miner Num - " + teamBase.minerList.Count;
        baseSoldierCount.text = "Soldier Num - " + teamBase.soldierList.Count;

        basePurchaseChoice.text = "Desired Purchase - " + teamBase.currentSpendChoice;
        baseOreChoice.text = "Desired Ore - " + teamBase.currentOreChoice;
        baseSoldierChoice.text = "Desired Stratagy - " + teamBase.currentSoldierStratagyChoice;
    }









}
