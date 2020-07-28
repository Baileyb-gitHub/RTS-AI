using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perceptionCode : MonoBehaviour
{
    public GameObject owner;
    public string ownerType;
    public string alliedColour;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != owner)
        {
            if (owner.GetComponent<Miner>() != null && owner.GetComponent<Miner>().alliedUnitList.Contains(other.gameObject) || owner.GetComponent<Soldier>() != null && owner.GetComponent<Soldier>().alliedUnitList.Contains(other.gameObject)  && owner.GetComponent<Miner>() != null && owner.GetComponent<Miner>().enemyUnitList.Contains(other.gameObject) || owner.GetComponent<Soldier>() != null && owner.GetComponent<Soldier>().enemyUnitList.Contains(other.gameObject))  // prevents duplicates issue
            {

            }
            else
            {
                if (other.gameObject.tag == "redSoldier" || other.gameObject.tag == "redMiner")
                {

                    if (alliedColour == "red")
                    {
                        if (ownerType == "miner")
                        {
                            owner.GetComponent<Miner>().alliedUnitList.Add(other.gameObject);
                        }
                        else
                        {
                            owner.GetComponent<Soldier>().alliedUnitList.Add(other.gameObject);
                        }
                    }


                    else
                    {
                        if (ownerType == "miner")
                        {
                            owner.GetComponent<Miner>().enemyUnitList.Add(other.gameObject);
                        }
                        else
                        {
                            owner.GetComponent<Soldier>().enemyUnitList.Add(other.gameObject);
                        }
                    }

                }

                else if (other.gameObject.tag == "blueSoldier" || other.gameObject.tag == "blueMiner")
                {

                    if (alliedColour == "red")
                    {
                        if (ownerType == "miner")
                        {
                            owner.GetComponent<Miner>().enemyUnitList.Add(other.gameObject);
                        }
                        else
                        {
                            owner.GetComponent<Soldier>().enemyUnitList.Add(other.gameObject);
                        }
                    }

                    else
                    {
                        if (ownerType == "miner")
                        {
                            owner.GetComponent<Miner>().alliedUnitList.Add(other.gameObject);
                        }
                        else
                        {
                            owner.GetComponent<Soldier>().alliedUnitList.Add(other.gameObject);
                        }
                    }
                }
            }





        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != owner)
        {
            if (other.gameObject.tag == "redSoldier" || other.gameObject.tag == "redMiner")
            {
              

                if (alliedColour == "red")
                {
                    if (ownerType == "miner")
                    {
                       
                        owner.GetComponent<Miner>().alliedUnitList.Remove(other.gameObject);
                    }
                    else
                    {
                        owner.GetComponent<Soldier>().alliedUnitList.Remove(other.gameObject);
                    }
                }


                else
                {
                    if (ownerType == "miner")
                    {
                        owner.GetComponent<Miner>().enemyUnitList.Remove(other.gameObject);
                    }
                    else
                    {
                        owner.GetComponent<Soldier>().enemyUnitList.Remove(other.gameObject);
                    }
                }

            }

            else if (other.gameObject.tag == "blueSoldier")
            {

                if (alliedColour == "red")
                {
                    if (ownerType == "miner")
                    {
                        owner.GetComponent<Miner>().enemyUnitList.Remove(other.gameObject);
                    }
                    else
                    {
                        owner.GetComponent<Soldier>().enemyUnitList.Remove(other.gameObject);
                    }
                }

                else
                {
                    if (ownerType == "miner")
                    {
                        owner.GetComponent<Miner>().alliedUnitList.Remove(other.gameObject);
                    }
                    else
                    {
                        owner.GetComponent<Soldier>().alliedUnitList.Remove(other.gameObject);
                    }
                }
            }
        }    

    }









}
