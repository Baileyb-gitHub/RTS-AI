using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Miner : MonoBehaviour
{

    public Graph graphReference;   // reference to object holding graph data and path calculations
    public List<int> TraverseList; // list of nodes to target destination


   
    public GameObject textDisplay; // reference to text to show current state
    public baseAI baseAI;

    public string stateManager;  // bank, mine, flee
    public string type;
    public float maxHealth;
    public float health; // health incase attacked, de spawn on 0
    public float speed; // speed of movement 


    public float gatherRate;
    public float maxCarry;  // total recources miner can carry before having to dump
    public float currentCarry; // current recources carried 

    public float ironCarry; // how much iron carried
    public float FoodCarry; // how much food carried
    

    public bool targetSet;  // does new traverse list need calculating
    public string TargetNodeType;  // current desired node type by main ai
    public List<OreNode> ironNodesList;  // nodes to consider for harvest
    public List<OreNode> foodNodesList;  // nodes to consider for harvest
    public OreNode targetNode;  // target node to harvest
    public bool targetInRange;  // is target destination close enough for interacy


    public GameObject triggerObj; // reference to target obj, used to cofirm tag
    public List<GameObject> alliedUnitList;
    public List<GameObject> enemyUnitList;

    private float healCooldown = 15.0f;
    private float currentHealCooldown = 15.0f;


    // Start is called before the first frame update
    void Start()
    {
        stateManager = "mine";
        
      
    }

    // Update is called once per frame
    void Update()
    {
        currentHealCooldown -= 1.0f * Time.deltaTime;

        if (health < 1)
        {
            GameObject.Destroy(gameObject);
        }


        if (stateManager == "bank")
        {
            stateBank();
        }

        else if (stateManager == "mine")
        {
            stateMine();
        }

        else if (stateManager == "flee")
        {
            stateFlee();
        }


        textDisplay.GetComponent<TextMesh>().text = stateManager;  // display current state as text 


    }

   
    

    public void stateBank()
    {

        if (targetSet == false)   // if first loop of state then assign target
        {
            getPathToObject(gameObject, baseAI.gameObject);
            triggerObj = baseAI.gameObject;
            targetSet = true;
            
        }



        if (targetInRange == true)   // if at base store resources and return to mining
        {

            baseAI.foodStored += FoodCarry;
            baseAI.ironStored += ironCarry;
            baseAI.netStored += FoodCarry;
            baseAI.netStored += ironCarry;


            FoodCarry = 0;
            ironCarry = 0;
            currentCarry = 0;
            health = maxHealth;  // heal  in case banking while fleeing

            TraverseList.Clear();
            targetSet = false;
            targetInRange = false;
            stateManager = "mine";
   
            return;
        }
        else   // contine moving towards target destination
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
                transform.position = Vector3.MoveTowards(transform.position, baseAI.gameObject.transform.position, (speed * Time.deltaTime));
            }
        }


    }



    public void stateMine()
    {
        if (currentCarry >= maxCarry)
        {
            TraverseList.Clear();
            targetSet = false;
            targetInRange = false;
            stateManager = "bank";
            return;
        }

        if (health < maxHealth * 0.25 ||  enemyUnitList.Count > alliedUnitList.Count)
        {
            TraverseList.Clear();
            targetSet = false;
            targetInRange = false;
            stateManager = "flee";
            return;
        }

        else
        {


            if (targetSet == false)
            {

                if (TargetNodeType == "iron")
                {
                    targetNode = ironNodesList[Random.Range(0, ironNodesList.Count)];
                }

                if (TargetNodeType == "food")
                {
                    targetNode = foodNodesList[Random.Range(0, foodNodesList.Count )];
                }

                triggerObj = targetNode.gameObject;
                getPathToObject(gameObject, targetNode.gameObject);
                targetSet = true;
            }



            if (targetInRange == true)
            {

                targetNode.storedValue -= gatherRate * Time.deltaTime;

                if (TargetNodeType == "food")
                {
                    FoodCarry += gatherRate * Time.deltaTime;
                    currentCarry += gatherRate * Time.deltaTime;
                }

                if (TargetNodeType == "iron")
                {
                    ironCarry += gatherRate * Time.deltaTime;
                    currentCarry += gatherRate * Time.deltaTime;
                }

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
                    transform.position = Vector3.MoveTowards(transform.position, targetNode.gameObject.transform.position, (speed * Time.deltaTime));
                }
            }





        }

       

    }


    public void stateFlee()
    {
        if (targetSet == false)
        {
            getPathToObject(gameObject, baseAI.gameObject);
            triggerObj = baseAI.gameObject;
            targetSet = true;

        }



        if (targetInRange == true)
        {
            if(currentHealCooldown < 0.1f)
            {
                health = maxHealth;
            }
           

            TraverseList.Clear();
            targetSet = false;
            targetInRange = false;
            stateManager = "mine";      // go to mine, if bank required it will automatically switch
            return;
        }

        if(enemyUnitList.Count < alliedUnitList.Count)
        {
            TraverseList.Clear();
            targetSet = false;
            targetInRange = false;
            stateManager = "mine";
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
                transform.position = Vector3.MoveTowards(transform.position, baseAI.gameObject.transform.position, (speed * Time.deltaTime));
            }
        }

    }




    

    public void getPathToObject(GameObject pathStart,  GameObject pathTarget)
    {
        int closestId = 1;
        float currentCloser = 999;


        int targetId = 1;

        

        foreach (graphNode indic in graphReference.nodeList)
        {
           if( Vector3.Distance(indic.transform.position, pathStart.transform.position)  < currentCloser)
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

        TraverseList.AddRange( graphReference.getPath(graphReference.nodeList[closestId], graphReference.nodeList[targetId]));
        //TraverseList = graphReference.getPath(graphReference.nodeList[closestId], graphReference.nodeList[targetId]);
        // TraverseList.Add(targetId);



    }

      

    void  OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == triggerObj.tag)
        {
            targetInRange = true;
        }

       
    }
    void  OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == triggerObj.tag)
        {
            targetInRange = false; 
        }   
    }

    
}
