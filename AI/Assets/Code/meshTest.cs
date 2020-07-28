using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class meshTest : MonoBehaviour
{

    public string teststr;
    public NavMeshTriangulation myData;

    public List<int> indicies;


    public List<Vector3> verticies;
    public List<int> areas;



    // Start is called before the first frame update
    void Start()
    {

       


        myData = NavMesh.CalculateTriangulation();
        

        NavMesh.CalculateTriangulation();

        Debug.Log(myData);
     
        Debug.Log("indicies - " + myData.indices[21]);
        

        Debug.Log("verticies - " +  myData.vertices[2]);

        Debug.Log("areas - " + myData.areas[4]);

        int[] test = new int[5];
        

        

        int placement = 0;
        foreach (int indic in myData.indices)
        {
            indicies.Add(myData.indices[placement]);
            placement += 1;
        }


         placement = 0;
        foreach (Vector3 vert in myData.vertices)
        {
            verticies.Add(myData.vertices[placement]);
            placement += 1;
        }
       

     
        foreach (int area in myData.areas)
        {
           // Debug.Log("areas - " + myData.areas[area]);
            areas.Add(myData.areas[area]);
          
        }

        /*
        int lowCap = 0;
        int highCap = 1;

        while (highCap < 422)
        {
            Debug.DrawLine(verticies[lowCap], verticies[highCap], Color.magenta, 20.5f);
            lowCap += 1;
            highCap += 1;
        }
        */

        int counter = 1;

        int triPart1 = 0;
        int triPart2 = 1;
        int triPart3 = 2;
        int col = 1;



        Debug.Log(indicies.Count);

        while (counter <  indicies.Count / 3)
        {

            
            if (col == 1)
            {
                Debug.DrawLine(verticies[indicies[triPart1]], verticies[indicies[triPart2]], Color.black, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart2]], verticies[indicies[triPart3]], Color.black, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart3]], verticies[indicies[triPart1]], Color.black, 200.5f);
            }
            if (col == 2)
            {
                Debug.DrawLine(verticies[indicies[triPart1]], verticies[indicies[triPart2]], Color.blue, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart2]], verticies[indicies[triPart3]], Color.blue, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart3]], verticies[indicies[triPart1]], Color.blue, 200.5f);
            }
            if (col == 3)
            {
                Debug.DrawLine(verticies[indicies[triPart1]], verticies[indicies[triPart2]], Color.red, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart2]], verticies[indicies[triPart3]], Color.red, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart3]], verticies[indicies[triPart1]], Color.red, 200.5f);
            }
            if (col == 4)
            {
                Debug.DrawLine(verticies[indicies[triPart1]], verticies[indicies[triPart2]], Color.magenta, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart2]], verticies[indicies[triPart3]], Color.magenta, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart3]], verticies[indicies[triPart1]], Color.magenta, 200.5f);
            }
            if (col == 5)
            {
                Debug.DrawLine(verticies[indicies[triPart1]], verticies[indicies[triPart2]], Color.green, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart2]], verticies[indicies[triPart3]], Color.green, 200.5f);
                Debug.DrawLine(verticies[indicies[triPart3]], verticies[indicies[triPart1]], Color.green, 200.5f);
            }

            triPart1 += 3;
             triPart2 += 3;
             triPart3 += 3;

            col += 1;
            if(col > 5)
            {
                col = 1;
            }

            counter += 1;
        }



        Debug.DrawLine(verticies[5], verticies[6], Color.white, 20.5f);
        Debug.DrawLine(verticies[6], verticies[7], Color.blue, 20.5f);
        Debug.DrawLine(verticies[7], verticies[5], Color.red, 20.5f);

            

      //  Debug.DrawLine(verticies[2], verticies[3], Color.yellow, 20.5f);
      //  Debug.DrawLine(verticies[3], verticies[4], Color.blue, 20.5f);
      //  Debug.DrawLine(verticies[4], verticies[5], Color.red, 20.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
