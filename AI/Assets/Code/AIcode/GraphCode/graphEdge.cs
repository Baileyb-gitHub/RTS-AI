using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graphEdge : MonoBehaviour
{
    public int from;
    public int to;
    public float GCost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getCost()
    {
        return GCost;
    }
}
