using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreNode : MonoBehaviour
{
    public string nodeType; // what type, used by miner ai 
    public bool isEmpty;  // has node recources been exhausted ?
    public float storedValue;  // how much of recource left in node 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void checkStatus()  // called after node mined to check it wasnt the last stored recource 
    {
        if (storedValue < 1)
        {
            isEmpty = true;
        }
    }
}
