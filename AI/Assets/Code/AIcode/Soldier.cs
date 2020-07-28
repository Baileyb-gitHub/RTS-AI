using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Graph graphReference;   // reference to object holding graph data and path calculations
    public List<int> TraverseList; // list of nodes to target destination
    public List<GameObject> guardAreas; 
    public List<GameObject> huntAreas;   


    public GameObject textDisplay; // reference to text to show current state
    public baseAI baseAI;
    public baseAI enemyBaseAI;

    public string stateManager;  // bank, mine, flee
    public string type;
    public float maxHealth;
    public float health; // health incase attacked, de spawn on 0
    public float speed; // speed of movement 

    public float attackDamage;
    public float CurrentAttackCooldown;
    public float maxAttackCooldown;

    public float waitTimer;
    public float waitTimerMax;

    public bool targetSet;  // does new traverse list need calculating
    public GameObject targetArea;  // target place to go
    public GameObject targetCombatent;  // target enemy
    public bool targetInRange;  // is target destination close enough for interacy


    public GameObject triggerObj; // reference to target obj, used to cofirm tag
    public List<GameObject> alliedUnitList;
    public List<GameObject> enemyUnitList;

    public float curentRePath; 

    // Start is called before the first frame update
    void Start()
    {
      


    }

    // Update is called once per frame
    void Update()
    {
        CurrentAttackCooldown -= Random.Range(0.6f, 1.0f) * Time.deltaTime;
        if(health < 1)
        {
            GameObject.Destroy(gameObject);
        }


        if (stateManager == "guard")
        {
            stateGuard();
        }

        else if (stateManager == "hunt")
        {
            stateHunt();
        }

        else if (stateManager == "attackUnit")
        {
            stateAttackUnit();
        }

        else if (stateManager == "attackBase")
        {
            stateAttackBase();
        }


        textDisplay.GetComponent<TextMesh>().text = stateManager;




      



    }


   


    public void stateGuard()
    {

        if (targetSet == false)
        {
            // pick random guard position
            GameObject selectedGuardSpot = guardAreas[Random.Range(0, guardAreas.Count)];

            targetArea = selectedGuardSpot;
            getPathToObject(gameObject, selectedGuardSpot);
            triggerObj = selectedGuardSpot;
            targetSet = true;

        }



        if (targetInRange == true)
        {
            if(waitTimer < 1.0f)
            {
                waitTimer = waitTimerMax;
                targetSet = false;
                targetInRange = false;
                TraverseList.Clear();
            }
            else
            {
                waitTimer -= 1.0f * Time.deltaTime;
            }
            return;
        }
        else
        {
            if (TraverseList.Count > 0)
            {
                if (Vector3.Distance(transform.position, graphReference.nodeList[TraverseList[0]].transform.position) > 1.0f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, graphReference.nodeList[TraverseList[0]].transform.position, (speed * Time.deltaTime));

                }
                else
                {
                    TraverseList.RemoveAt(0);

                }

            }

            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetArea.gameObject.transform.position, (speed * Time.deltaTime));
            }
        }


    }



    public void stateHunt()
    {


        if (enemyUnitList.Count >= 1)
        {
            targetSet = false;
            targetInRange = false;
            TraverseList.Clear();
            stateManager = "attackUnit";
            return;
        }


            if (targetSet == false)
            {
              GameObject selectedHuntSpot = huntAreas[Random.Range(0, huntAreas.Count)];

                targetArea = selectedHuntSpot;
                 getPathToObject(gameObject, selectedHuntSpot);
              triggerObj = selectedHuntSpot.gameObject;
              targetSet = true;
            }



            if (targetInRange == true)
            {

                if (waitTimer < 1.0f)
                {
                waitTimer = waitTimerMax;
                targetSet = false;
                targetInRange = false;
                TraverseList.Clear();
                 }
                 else
                 {
                      waitTimer -= 1.0f * Time.deltaTime;
                 }
              return;

            }



            else
            {

                if (TraverseList.Count > 0)
                {
                    if (Vector3.Distance(transform.position, graphReference.nodeList[TraverseList[0]].transform.position) > 1.0f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, graphReference.nodeList[TraverseList[0]].transform.position, (speed * Time.deltaTime));

                    }
                    else
                    {
                        TraverseList.RemoveAt(0);

                    }

                }

                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetArea.gameObject.transform.position, (speed * Time.deltaTime));
                }
            }





        



    }


    public void stateAttackUnit()
    {
        if (targetSet == false)
        {
            targetCombatent = enemyUnitList[Random.Range(0, enemyUnitList.Count)];
            getPathToObject(gameObject, targetCombatent.gameObject);
            triggerObj = targetCombatent.gameObject;
            targetSet = true;
            curentRePath = Vector3.Distance(gameObject.transform.position, targetCombatent.transform.position);

        }


        
        if(curentRePath < 0.1f)
        {
            TraverseList.Clear();
            getPathToObject(gameObject, targetCombatent.gameObject);
            curentRePath = Vector3.Distance(gameObject.transform.position, targetCombatent.transform.position);
        }
        

        if(targetCombatent == null)
        {
            targetSet = false;
            targetInRange = false;
            TraverseList.Clear();
            stateManager = baseAI.currentSoldierStratagyChoice;

            
            foreach (var enemy in enemyUnitList)   // removes killed enemy from enemy list (doesent exit list from trigger exit)
            {
                if (enemy == null)
                {
                    enemyUnitList.Remove(enemy);              
                }
            }

       
            return;
        }

        if (targetInRange == true)
        {
            // attack unit if not on cooldown

            if (CurrentAttackCooldown < 0.1f)
            {
                if(targetCombatent.tag == "blueSoldier" || targetCombatent.tag == "redSoldier")
                {
                    targetCombatent.GetComponent<Soldier>().health -= attackDamage;
                }
                if (targetCombatent.tag == "blueMiner" || targetCombatent.tag == "redMiner")
                {
                    targetCombatent.GetComponent<Miner>().health -= attackDamage;
                }

                CurrentAttackCooldown = maxAttackCooldown;
            }

        }            
        else
        {
            curentRePath -= 6.5f * Time.deltaTime;



            if (TraverseList.Count > 0)
            {
                if (Vector3.Distance(transform.position, graphReference.nodeList[TraverseList[0]].transform.position) > 0.8f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, graphReference.nodeList[TraverseList[0]].transform.position, (speed * Time.deltaTime));

                }
                else
                {
                    TraverseList.RemoveAt(0);
                   // TraverseList.Clear();
                   // getPathToObject(gameObject, targetCombatent.gameObject);

                }

            }

            else
            {
                transform.position = Vector3.MoveTowards(transform.position, baseAI.gameObject.transform.position, (speed * Time.deltaTime));
            }
        }

    }


    public void stateAttackBase()
    {
        if (targetSet == false)
        {

            targetArea = enemyBaseAI.gameObject;
            getPathToObject(gameObject, enemyBaseAI.gameObject);
            triggerObj = targetArea;
            targetSet = true;

        }



        if (targetInRange == true)
        {
            // attack base if not on cooldown
            if (CurrentAttackCooldown < 0.1f)
            {
                enemyBaseAI.baseHealth -= attackDamage;
                CurrentAttackCooldown = maxAttackCooldown;
            }

        }

        // if target killed or too far away then back to hunt / guard 


        else
        {
            if (TraverseList.Count > 0)
            {
                if (Vector3.Distance(transform.position, graphReference.nodeList[TraverseList[0]].transform.position) > 0.8f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, graphReference.nodeList[TraverseList[0]].transform.position, (speed * Time.deltaTime));

                }
                else
                {
                    TraverseList.RemoveAt(0);

                }

            }

            else
            {
                transform.position = Vector3.MoveTowards(transform.position, enemyBaseAI.gameObject.transform.position, (speed * Time.deltaTime));
            }
        }

    }






    public void getPathToObject(GameObject pathStart, GameObject pathTarget)
    {
        int closestId = 1;
        float currentCloser = 999;


        int targetId = 1;



        foreach (graphNode indic in graphReference.nodeList)
        {
            if (Vector3.Distance(indic.transform.position, pathStart.transform.position) < currentCloser)
            {
                closestId = indic.id;
                currentCloser = Vector3.Distance(indic.transform.position, pathStart.transform.position);
            }


        }

        currentCloser = 999;

        foreach (graphNode indic in graphReference.nodeList)
        {
            if (Vector3.Distance(indic.transform.position, pathTarget.transform.position) < currentCloser)
            {
                targetId = indic.id;
                currentCloser = Vector3.Distance(indic.transform.position, pathTarget.transform.position);
            }


        }

        TraverseList.AddRange(graphReference.getPath(graphReference.nodeList[closestId], graphReference.nodeList[targetId]));
        //TraverseList = graphReference.getPath(graphReference.nodeList[closestId], graphReference.nodeList[targetId]);
        // TraverseList.Add(targetId);



    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == triggerObj)
        {
            targetInRange = true;
        }


    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == triggerObj)
        {
            targetInRange = false;
        }
    }

}
