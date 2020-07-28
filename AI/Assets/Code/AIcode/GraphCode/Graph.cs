using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Graph : MonoBehaviour
{

    public int test1;
    public int test2;

    public bool debugMode;

    public GameObject nodePrefab;
    public GameObject adjacencyPrefab;
    public GameObject areaPrefab;


    public GameObject currentNode;
    public GameObject currentAdjacency;
    public GameObject currentArea;


    public NavMeshTriangulation meshData;
    public List<int> indicies;
    public List<Vector3> verticies;


    public List<graphNode> nodeList;  // list of nodes in scene 
    public List<int> traversedList;   // used for path to object
    public List<int> dontReturnList;   // used for path to object
    public List<graphEdge> minPriQue;  // list of nodes in scene 


    public graphNode currentPathNode;
    List<graphArea> targetAreas;  // list of areas with direct acess to target  node 


    // Start is called before the first frame update
    void Start()
    {

        meshData = NavMesh.CalculateTriangulation();

        int placement = 0;
        foreach (Vector3 vert in meshData.vertices)
        {
            verticies.Add(meshData.vertices[placement]);


            currentNode = Instantiate(nodePrefab, new Vector3(vert.x, vert.y, vert.z), Quaternion.identity);
            currentNode.GetComponent<graphNode>().id = placement;
            nodeList.Add(currentNode.GetComponent<graphNode>());


            placement += 1;
        }

        // Vector3 test = nodeList[3].transform.position;

        placement = 0;

        foreach (int indic in meshData.indices)
        {
            indicies.Add(meshData.indices[placement]);
            placement += 1;
        }







        int counter = 0;

        int triPart1 = 0;
        int triPart2 = 1;
        int triPart3 = 2;

       


        while (counter < (indicies.Count / 3)) //(indicies.Count / 3)
        {

            currentArea = Instantiate(areaPrefab);
            currentArea.GetComponent<graphArea>().area = counter;

            nodeList[indicies[triPart1]].GetComponent<graphNode>().areaList.Add(currentArea.GetComponent<graphArea>());    // assigns current area to each node 
            nodeList[indicies[triPart2]].GetComponent<graphNode>().areaList.Add(currentArea.GetComponent<graphArea>());
            nodeList[indicies[triPart3]].GetComponent<graphNode>().areaList.Add(currentArea.GetComponent<graphArea>());


            //indicies[triPart2]

            //
            //nodeList[indicies[triPart1]].GetComponent<graphNode>().adjacencyList.Add(currentAdjacency.GetComponent<graphEdge>());   // ads first to-from relation for tri part 1 

            currentAdjacency = Instantiate(adjacencyPrefab);
            currentAdjacency.GetComponent<graphEdge>().from = indicies[triPart1];
            currentAdjacency.GetComponent<graphEdge>().to = indicies[triPart2];
            currentAdjacency.GetComponent<graphEdge>().GCost = Vector3.Distance(nodeList[indicies[triPart1]].GetComponent<graphNode>().transform.position, nodeList[indicies[triPart2]].GetComponent<graphNode>().transform.position);
            nodeList[indicies[triPart1]].GetComponent<graphNode>().adjacencyList.Add(currentAdjacency.GetComponent<graphEdge>());   // ads first to-from relation for tri part 1 

            currentAdjacency = Instantiate(adjacencyPrefab);
            currentAdjacency.GetComponent<graphEdge>().from = indicies[triPart1];
            currentAdjacency.GetComponent<graphEdge>().to = indicies[triPart3];
            currentAdjacency.GetComponent<graphEdge>().GCost = Vector3.Distance(nodeList[indicies[triPart1]].GetComponent<graphNode>().transform.position, nodeList[indicies[triPart3]].GetComponent<graphNode>().transform.position);
            nodeList[indicies[triPart1]].GetComponent<graphNode>().adjacencyList.Add(currentAdjacency.GetComponent<graphEdge>());   // ads second to-from relation for tri part 1 



            currentAdjacency = Instantiate(adjacencyPrefab);
            currentAdjacency.GetComponent<graphEdge>().from = indicies[triPart2];
            currentAdjacency.GetComponent<graphEdge>().to = indicies[triPart1];
            currentAdjacency.GetComponent<graphEdge>().GCost = Vector3.Distance(nodeList[indicies[triPart2]].GetComponent<graphNode>().transform.position, nodeList[indicies[triPart1]].GetComponent<graphNode>().transform.position);
            nodeList[indicies[triPart2]].GetComponent<graphNode>().adjacencyList.Add(currentAdjacency.GetComponent<graphEdge>());   // ads first to-from relation for tri part 2 

            currentAdjacency = Instantiate(adjacencyPrefab);
            currentAdjacency.GetComponent<graphEdge>().from = indicies[triPart2];
            currentAdjacency.GetComponent<graphEdge>().to = indicies[triPart3];
            currentAdjacency.GetComponent<graphEdge>().GCost = Vector3.Distance(nodeList[indicies[triPart2]].GetComponent<graphNode>().transform.position, nodeList[indicies[triPart3]].GetComponent<graphNode>().transform.position);
            nodeList[indicies[triPart2]].GetComponent<graphNode>().adjacencyList.Add(currentAdjacency.GetComponent<graphEdge>());   // ads second to-from relation for tri part 2 


            currentAdjacency = Instantiate(adjacencyPrefab);
            currentAdjacency.GetComponent<graphEdge>().from = indicies[triPart3];
            currentAdjacency.GetComponent<graphEdge>().to = indicies[triPart1];
            currentAdjacency.GetComponent<graphEdge>().GCost = Vector3.Distance(nodeList[indicies[triPart3]].GetComponent<graphNode>().transform.position, nodeList[indicies[triPart1]].GetComponent<graphNode>().transform.position);
            nodeList[indicies[triPart3]].GetComponent<graphNode>().adjacencyList.Add(currentAdjacency.GetComponent<graphEdge>());   // ads first to-from relation for tri part 3 

            currentAdjacency = Instantiate(adjacencyPrefab);
            currentAdjacency.GetComponent<graphEdge>().from = indicies[triPart3];
            currentAdjacency.GetComponent<graphEdge>().to = indicies[triPart2];
            currentAdjacency.GetComponent<graphEdge>().GCost = Vector3.Distance(nodeList[indicies[triPart3]].GetComponent<graphNode>().transform.position, nodeList[indicies[triPart2]].GetComponent<graphNode>().transform.position);
            nodeList[indicies[triPart3]].GetComponent<graphNode>().adjacencyList.Add(currentAdjacency.GetComponent<graphEdge>());   // ads second to-from relation for tri part 3 







            // currentAdjacency = Instantiate(adjacencyPrefab);

            //nodeList[indicies[triPart1]].GetComponent<graphNode>().adjacencyList.Add(currentArea.GetComponent<graphArea>());



            triPart1 += 3;
            triPart2 += 3;
            triPart3 += 3;

            // verticies[indicies[triPart1]], verticies[indicies[triPart2]]

            counter += 1;
        }

        placement = 0;
        int placement2 = 0;
        foreach (Vector3 vert in verticies)
        {



            foreach (Vector3 vert2 in verticies)
            {
                if (vert == vert2)
                {

                    int edgeCount = nodeList[placement2].GetComponent<graphNode>().adjacencyList.Count;


                    nodeList[placement].GetComponent<graphNode>().adjacencyList.Add(nodeList[placement2].GetComponent<graphNode>().adjacencyList[0]);
                    nodeList[placement].GetComponent<graphNode>().adjacencyList.Add(nodeList[placement2].GetComponent<graphNode>().adjacencyList[1]);

                    nodeList[placement].areaList.Add(nodeList[placement2].areaList[0]);
                    


                        /*
                        for(int i = nodeList[placement2].GetComponent<graphNode>().adjacencyList.Count; i > 0; i--)
                        {
                            nodeList[placement].GetComponent<graphNode>().adjacencyList.Add(nodeList[placement2].GetComponent<graphNode>().adjacencyList[i - 1]);

                        }
                        */

                        /*

                        currentAdjacency = Instantiate(adjacencyPrefab);
                        currentAdjacency.GetComponent<graphEdge>().from = placement;
                        currentAdjacency.GetComponent<graphEdge>().to = placement2;
                        nodeList[placement].adjacencyList.Add(currentAdjacency.GetComponent<graphEdge>());

                        */
                        edgeCount = 0;
                }

                placement2 += 1;
            }

            placement2 = 0;
            placement += 1;
        }


    }

    public List<int> getPath(graphNode sourceNode, graphNode targetNode)
    {
        minPriQue.Clear();
        traversedList.Clear();
        dontReturnList.Clear();

        bool found = false;
        int exitCount = 1;





        int placement = 0;
        foreach (graphNode indic in nodeList)
        {
            nodeList[placement].GetComponent<graphNode>().HCost = Vector3.Distance(nodeList[placement].GetComponent<graphNode>().transform.position, targetNode.GetComponent<graphNode>().transform.position); // assings h cost
            placement += 1;
        }





        placement = 0;
        foreach (graphEdge edge in sourceNode.adjacencyList)
        {
            minPriQue.Add(sourceNode.adjacencyList[placement]);
            placement += 1;
        }

        

        while (found == false)
        {
            if (exitCount > 500)
            {
                Debug.Log("exit error, was in loop");



              


                if(debugMode == true)   //  visualising path, only used for testing / prrof of work
                {
                    Debug.DrawLine(sourceNode.transform.position, nodeList[traversedList[0]].transform.position, Color.green, 5.5f);  // 
                    for (int i = traversedList.Count; i > 1; i--)
                    {

                        Debug.DrawLine(nodeList[traversedList[i - 1]].transform.position, nodeList[traversedList[i - 2]].transform.position, Color.magenta, 5.5f);

                    }
                }

                


                dontReturnList.Clear();

                found = true;
                return(traversedList);
            }


            float bestFCOST = 999;

            placement = 0;
            foreach (graphEdge edge in minPriQue)
            {
                //if ((minPriQue[placement].GCost) + (   nodeList[minPriQue[placement].to].HCost * 1.5f ) < bestFCOST)
                if (nodeList[minPriQue[placement].to].HCost < bestFCOST)
                {
                    if (dontReturnList.Contains(edge.to))
                    {
                        // dont add as target node if already visited. to avoid backtrack
                    }
                    else
                    {
                        bestFCOST = nodeList[minPriQue[placement].to].HCost;
                        currentPathNode = nodeList[minPriQue[placement].to];
                    }

                  

                }

                   

                placement += 1;
            }


            traversedList.Add(currentPathNode.id);
            dontReturnList.Add(currentPathNode.id);

            placement = 0;
            foreach (Vector3 vert in verticies)
            {
                    if (vert ==  currentPathNode.transform.position)
                    {
                    dontReturnList.Add(nodeList[placement].id);
                    }
                placement += 1;
            }



            
            foreach (graphEdge edge in currentPathNode.adjacencyList)
            {
                if(nodeList[edge.to].transform.position == currentPathNode.transform.position)
                {
                    dontReturnList.Add(nodeList[edge.to].id);
                }
            }
            




            placement = 0;
            foreach (graphArea area in currentPathNode.areaList)
            {
               

                foreach (graphArea targAreas in targetNode.areaList)
                {
                    if (area.area == targAreas.area)
                    {
                        found = true;
                        Debug.Log("FOUND ! Area");





                        //Debug.DrawLine(sourceNode.transform.position, nodeList[traversedList[0]].transform.position, Color.green, 5.5f);  // 

                        if (debugMode == true)   //  visualising path, only used for testing / prrof of work 
                        {
                            for (int i = traversedList.Count; i > 1; i--)
                            {

                                Debug.DrawLine(nodeList[traversedList[i - 1]].transform.position, nodeList[traversedList[i - 2]].transform.position, Color.magenta, 50.5f);

                            }
                        }

                         


                        return (traversedList);
                       
                    }

                }

                if (currentPathNode.id == targetNode.id)
                {
                   
                    found = true;
                
                    return (traversedList);
                    
                }







                placement = 0;


                if(found == false)
                {
                    foreach (graphEdge edge in currentPathNode.adjacencyList)
                    {
                        minPriQue.Add(currentPathNode.adjacencyList[placement]);
                        placement += 1;
                    }
                }

               





                exitCount += 1;
            }


            
        }

        return (traversedList);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            getPath(nodeList[1], nodeList[2]);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            getPath(nodeList[271], nodeList[2056]);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            getPath(nodeList[5], nodeList[223]);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            getPath(nodeList[test1], nodeList[test2]);
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            traversedList.Clear();

        }



    }
}