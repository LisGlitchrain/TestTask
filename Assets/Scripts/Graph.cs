using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Graph : MonoBehaviour
{

    public Vertex[] Vertices { get; set; }
    List<Vertex> pOI = new List<Vertex>();
    [SerializeField] int maxPathDepth = 3;
    public bool IsInitialized { get; set; }
    [SerializeField] bool drawGraph;
    [SerializeField] int maxPathToAnalyzeCount = 5;
    [SerializeField] int failsCount = 1;
    int childrenCount = 0;
    [SerializeField] float pOIRadius = 1;
    // Start is called before the first frame update
    void Start()
    {
        IsInitialized = false;
        IsInitialized = Initialize();
    }

    // Update is called once per frame
    public void Update()
    {
        if (transform.childCount != childrenCount)
        {
            Initialize();
            childrenCount = transform.childCount;
        }
        if (drawGraph)
        {
            //Debug.Log("Graph is updated");
            DrawGraph();
        }

    }


    public bool Initialize()
    {
        Vertices = GetComponentsInChildren<Vertex>();
        pOI.Clear();
        foreach (var vertex in Vertices)
        {
            if (vertex.PointOfInterest)
            {
                pOI.Add(vertex);
            }
        }
        return true;
    }

    void DrawGraph()
    {
        //print("DROWN");
        foreach (var vertex in Vertices)
        {

            foreach(var inVertex in vertex.LinkedVertices)
            {
                DrawEdge(vertex, inVertex);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (IsInitialized)
        {
            Handles.color = Color.black;
            foreach (var vertex in Vertices)
            {
                if (vertex.PointOfInterest)
                {
                    UnityEditor.Handles.DrawWireDisc(vertex.transform.position, Vector3.up, pOIRadius);
                }
            }
        }
    }



    void DrawEdge(Vertex a, Vertex b)
    {
        Color color = Color.white;
        color = SetColor(GetPointType(a, b));
        Debug.DrawLine(a.transform.position, b.transform.position, color);
    }

    Color SetColor(VertexType a)
    {
        Color color;
        switch (a)
        {
            case VertexType.Path:
                color = Color.yellow;
                break;
            case VertexType.Direction:
                color = Color.blue;
                break;
            case VertexType.Road:
                color = Color.green;
                break;
            default:
                color = Color.white;
                break;
        }
        return color;
    }

    public List<Vertex> GetPathToNextPointOfInterest(Vertex currentPOI, Being being)
    {

        print("+++++++++++++");
        print("+++++++++++++");
        print("+++++++++++++");
        Vertex nextPOI = GetNextPOI(currentPOI, being.Type);
        print($" Next POI {nextPOI.gameObject.name }");
        List<Vertex> path = new List<Vertex>();
        List<Path> pathes = new List<Path>();
        for(var i=0; i< pathes.Count;i++)
        {
            pathes[i] = new Path();
        }
        for (int currentMaxdepth = 1; currentMaxdepth < maxPathDepth; currentMaxdepth++)
        {
            path.Clear();
            path.Add(currentPOI);
            print($"Depth {currentMaxdepth}");
            if (!WeightedDeepSearch(currentPOI, nextPOI, being, 1, currentMaxdepth, path, pathes, maxPathToAnalyzeCount))
            {
                print("Failed to find Route.");
                path.Remove(currentPOI);
            }
            if (pathes.Count == maxPathToAnalyzeCount)
            {
                break;
            }
            //if (DeepSearch(currentPOI, nextPOI, being.Type, 1, currentMaxdepth, path))
            //{
            //    break;
            //}
        }
        foreach (var vertex in path)
        {
            print($"Vertex {vertex.name}");
        }
        print($"Length {path.Count}");
        return path;
    }

    public Vertex GetFirstPOI(BeingType beingType)
    {
        return GetNextPOI();
    }

    bool WeightedDeepSearch(Vertex currentVertex, Vertex targetVertex, Being being, int currentDepth, int maxDepth, List<Vertex> path, List<Path> pathes, int maxPathCount)
    {
        Path tempPath = new Path();

        for (var i=pathes.Count;i< maxPathCount;i++)
        {
            if (WeightedDeepSearchInnerCycle(currentVertex, targetVertex, being, 1, maxDepth, pathes, tempPath))
            {
                pathes.Add(tempPath.Clone() as Path);
                tempPath = new Path();
            }
        }

        float cost = 0f;
        Path goodPath = new Path();
        foreach(var inPath in pathes)
        {
            if (inPath.PathCost < cost || cost ==0f)
            {
                goodPath = inPath.Clone() as Path;
                cost = goodPath.PathCost;
            }
        }
        foreach (var vertex in goodPath.Vertices)
        {
            path.Add(vertex);
        }
        Vertex tempVertex;
        for(var i=0; i< path.Count/2;i++)
        {
            tempVertex = path[i];
            path[i] = path[path.Count - i - 1];
            path[path.Count - i - 1] = tempVertex;
        }

        if (pathes.Count == maxPathCount)
        {
            foreach (var inPath in pathes)
            {
                print($"PATH ANALYZE {pathes.IndexOf(inPath)}");
                print($"Path cost {pathes[pathes.IndexOf(inPath)].PathCost}");
            }
            return true;
        }
        else return false;
    }

    bool WeightedDeepSearchInnerCycle(Vertex currentVertex, Vertex targetVertex, Being being, int currentDepth, int maxDepth, List<Path> pathes, Path tempPath)
    {
        var failCount = 0;
        while (failCount < failsCount)
        {

            foreach (var vertex in currentVertex.LinkedVertices)
            {
                    //print($"Vertex {vertex.gameObject.name}");
                    bool isNotPrevious = true;
                    foreach (var vert in tempPath.Vertices)
                    {
                        if (vert.Equals(vertex))
                        {
                            isNotPrevious = false;
                            break;
                        }
                    }
                    if (vertex.Equals(targetVertex) && isNotPrevious)
                    {
                        tempPath.Vertices.Add(vertex);
                        //tempPath.PathCost++;
                        tempPath.PathCost = being.GetPointCost(
                                                                GetPointType(vertex,
                                                                            tempPath.Vertices.ToArray()[tempPath.Vertices.IndexOf(vertex)]));

                        foreach (var path in pathes)
                        {
                            if (tempPath.Equals(path))
                            {
                                tempPath.Vertices.Remove(vertex);
                                return false;
                            }
                        }
                        //print($"Vertex {vertex.gameObject.name} is added");
                        //foreach (var vert in tempPath.Vertices)
                        //{
                        //    print(vert.name);
                        //}
                        return true;
                    }
                    else if (currentDepth < maxDepth && IsVertexTypeGood(vertex, being.Type) && isNotPrevious)
                    {
                        tempPath.Vertices.Add(vertex);
                        //print($"Vertex {vertex.gameObject.name} is added");

                        if (WeightedDeepSearchInnerCycle(vertex, targetVertex, being, currentDepth + 1, maxDepth, pathes, tempPath))
                        {
                            if (tempPath.Vertices.IndexOf(vertex) > 0)
                            {
                                tempPath.PathCost += being.GetPointCost(
                                                                GetPointType(vertex,
                                                                            tempPath.Vertices.ToArray()[tempPath.Vertices.IndexOf(vertex) - 1]));
                            }

                            //foreach(var vert in tempPath.Vertices)
                            //{
                            //    print(vert.name);
                            //}
                            return true;
                        }
                        else
                        {
                            //if (tempPath.Vertices.Count>1)
                            //{
                                tempPath.Vertices.Remove(vertex);
                            //}

                        }
                    }              
               
            }  
            failCount++;
        }
        return false;      
    }

    public VertexType GetPointType(Vertex currentVertex,Vertex targetVertex)
    {

        if (currentVertex.Type == targetVertex.Type && currentVertex.Type != VertexType.PathDirection && currentVertex.Type != VertexType.PathDirectionRoad)
        {
            return currentVertex.Type;
        }
        else if (currentVertex.Type == VertexType.PathDirection || currentVertex.Type == VertexType.PathDirectionRoad)
        {
            return targetVertex.Type;
        }
        else if (targetVertex.Type == VertexType.PathDirection || targetVertex.Type == VertexType.PathDirectionRoad)
        {
            return currentVertex.Type;
        }
        else if (currentVertex.Type == targetVertex.Type && currentVertex.Type == VertexType.PathDirection)
        {
            return VertexType.Path;
        }
        else 
            return VertexType.ErrorType;
    }

    bool DeepSearch(Vertex currentVertex, Vertex targetVertex, BeingType beingType, int currentDepth, int maxDepth, List<Vertex> path)
    {
        foreach (var vertex in currentVertex.LinkedVertices)
        {

            if (vertex == targetVertex)
            {
                path.Add(vertex);
                return true;
            }
            else if (currentDepth < maxDepth && IsVertexTypeGood(vertex, beingType))
            {
                
                if (DeepSearch(vertex, targetVertex, beingType, currentDepth + 1, maxDepth, path))
                {
                    path.Add(vertex);
                    return true;
                }
            }
        }
        return false;
    }

    bool IsVertexTypeGood(Vertex vertex, BeingType beingType)
    {
        bool isGood = false;
        switch(beingType)
        {
            case BeingType.Car:
                if (vertex.Type == VertexType.Road)
                    isGood = true;
                else if (vertex.Type == VertexType.PathDirectionRoad)
                    isGood = true;
                break;
            case BeingType.Human:
                if (vertex.Type == VertexType.Path)
                    isGood = true;
                else if (vertex.Type == VertexType.Direction)
                    isGood = true;
                else if (vertex.Type == VertexType.PathDirection)
                    isGood = true;
                else if (vertex.Type == VertexType.PathDirectionRoad)
                    isGood = true;
                break;
        }
        return isGood;
    }

    public Vertex GetNextPOI(Vertex currentPOI, BeingType beingType)
    {
        System.Random r = new System.Random();
        var index = r.Next(0,pOI.Count);
        while (currentPOI.transform.Equals(pOI.ToArray()[index].transform) || !IsVertexTypeGood(pOI.ToArray()[index], beingType))
        {
            index = r.Next(0, pOI.Count);
        }
        print($"!!!");
        print($"!!!");
        print($"!!!");
        print($"!!!");
        print($"!!!");
        print($"!!!");
        print($"POI {pOI.ToArray()[index].gameObject.name}");
        return pOI.ToArray()[index];
    }

    Vertex GetNextPOI()
    {
        System.Random r = new System.Random();
        var index = r.Next(0, pOI.Count);
        return pOI.ToArray()[index];
    }

}
