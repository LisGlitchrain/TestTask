using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    Vertex[] vertices;
    public Vertex[] Vertices { get { return vertices; } set { vertices = value; } }
    List<Vertex> pOI = new List<Vertex>();
    [SerializeField] int maxPathDepth;
    public bool IsInitialized { get; set; }
    [SerializeField] bool drawGraph;
    // Start is called before the first frame update
    void Start()
    {
        IsInitialized = false;
        vertices = FindObjectsOfType<Vertex>();
        foreach(var vertex in Vertices)
        {
            if (vertex.PointOfInterest)
            {
                pOI.Add(vertex);
            }
        }
        IsInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (drawGraph) DrawGraph();
    }


    void DrawGraph()
    {
        foreach(var vertex in Vertices)
        {
            foreach(var inVertex in vertex.LinkedVertices)
            {
                DrawEdge(vertex, inVertex);
            }
        }
    }

    void DrawEdge(Vertex a, Vertex b)
    {
        Color color = Color.white;
        if (a.Type == b.Type)
        {
            color = SetColor(a);
            Debug.DrawLine(a.transform.position, b.transform.position, color);
        }
        else if (a.Type == VertexType.PathDirection)
        {
            color = SetColor(b);
            Debug.DrawLine(a.transform.position, b.transform.position, color);
        }
        else if (b.Type == VertexType.PathDirection)
        {
            color = SetColor(a);
            Debug.DrawLine(a.transform.position, b.transform.position, color);
        }
        else
        {
            print("ERROR VERTEXTYPE");
        }
    }

    Color SetColor(Vertex a)
    {
        Color color;
        switch (a.Type)
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

    public List<Vertex> GetPathToNextPointOfInterest(Vertex currentPOI, BeingType beingType)
    {
        Vertex nextPOI = GetNextPOI(currentPOI);
        List<Vertex> path = new List<Vertex>();
        for (int currentMaxdepth = 1; currentMaxdepth < maxPathDepth; currentMaxdepth++)
        {
            if (DeepSearch(currentPOI, nextPOI, beingType, 1, currentMaxdepth, ref path))
            {
                path.Add(currentPOI);
                break;
            }
        }
        print("+++++++++++++");
        foreach(var vertex in path)
        {
            print($"Vertex {vertex.name}");
        }
        print($"Length {path.Count}");
        return path;
    }

    public Vertex GetFirstPOI( BeingType beingType)
    {
        return GetNextPOI();
    }

    bool DeepSearch(Vertex currentVertex, Vertex targetVertex, BeingType beingType, int currentDepth, int maxDepth, ref List<Vertex> path)
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
                if (DeepSearch(vertex, targetVertex, beingType, currentDepth + 1, maxDepth, ref path))
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
                break;
            case BeingType.Human:
                if (vertex.Type == VertexType.Path)
                    isGood = true;
                if (vertex.Type == VertexType.Direction)
                    isGood = true;
                if (vertex.Type == VertexType.PathDirection)
                    isGood = true;
                break;
        }
        return isGood;
    }

    Vertex GetNextPOI(Vertex currentPOI)
    {
        System.Random r = new System.Random();
        var index = r.Next(0,pOI.Count);
        while (currentPOI == pOI.ToArray()[index])
        {
            index = r.Next(0, pOI.Count);
        }
        return pOI.ToArray()[index];
    }

    Vertex GetNextPOI()
    {
        System.Random r = new System.Random();
        var index = r.Next(0, pOI.Count);
        return pOI.ToArray()[index];
    }

}
