using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVisualizer : MonoBehaviour
{
    static List<GameObject> lines = new List<GameObject>();
    static List<GameObject> pathLines = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void Redraw(Graph graph)
    {
        DestroyLineRenderers(lines);
        var i = 0;
        foreach (var vert in graph.Vertices)
        {
            foreach (var invert in vert.LinkedVertices)
            {
                GameObject line = new GameObject($"Line {i}");
                line.AddComponent<LineRenderer>();
                LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, vert.gameObject.transform.position);
                lineRenderer.SetPosition(1, invert.gameObject.transform.position);
                lineRenderer.material = SetMat(vert, invert);
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lines.Add(line);
                line.transform.SetParent(graph.transform);
                i++;
            }
        }
    }

    public static void DestroyLineRenderers(List<GameObject> lines)
    {
        while (lines.Count>0)
        {
            DestroyImmediate(lines[0]);
            lines.RemoveAt(0);
        }
    }

    static Material SetMat(Vertex a, Vertex b)
    {
        string path = "Materials/Line";
        switch (Graph.GetPointType(a,b))
        {
            case VertexType.Direction:
                path = path + "Blue";
                break;
            case VertexType.Path:
                path = path + "Yellow";
                break;
            case VertexType.Road:
                path = path + "Green";
                break;
        }
        return Resources.Load(path) as Material;
    }

    public static void DrawPath(List<Vertex> path)
    {
        DestroyLineRenderers(pathLines);
        for (var i = 0; i< path.Count-1;i++)
        {
            GameObject line = new GameObject($"Path {i}");
            line.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, path[i].gameObject.transform.position);
            lineRenderer.SetPosition(1, path[i+1].gameObject.transform.position);
            lineRenderer.material = Resources.Load("Materials/LinePurple") as Material;
            lineRenderer.startWidth = 0.3f;
            lineRenderer.endWidth = 0.3f;
            pathLines.Add(line);      
        }
    }
}
