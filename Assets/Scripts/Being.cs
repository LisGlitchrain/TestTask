using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Being : MonoBehaviour
{
    [SerializeField] BeingType type = BeingType.Car;
    List<Vertex> path = new List<Vertex>();
    Graph graph;
    // Start is called before the first frame update
    void Start()
    {
        graph = FindObjectOfType<Graph>();

    }

    // Update is called once per frame
    void Update()
    {
        if (path.Count == 0)
        {
            Vertex firstPOI = graph.GetFirstPOI(BeingType.Car);
            transform.position = firstPOI.transform.position;
            path = graph.GetPathToNextPointOfInterest(firstPOI, type);          
        }
        DrawPath();
    }

    void DrawPath()
    {
        Vertex[] pathArray = path.ToArray();
        for(var i = 1; i < pathArray.Length; i++)
        {
            Debug.DrawLine(pathArray[i].transform.position, pathArray[i-1].transform.position, Color.magenta);
        }
    }
}
