using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Being : MonoBehaviour
{
    [SerializeField] BeingType type;
    public BeingType Type { get {return type; } set { type = value; } }
    List<Vertex> path = new List<Vertex>();
    Graph graph;
    Vertex pOI;

    [SerializeField] float speed;
    [SerializeField] float accuracy;
    [SerializeField] float pathCost;
    [SerializeField] float roadCost;
    [SerializeField] float pathDirectionCost;
    [SerializeField] float directionCost;
    [SerializeField] float pathDirectionRoadCost;
    [SerializeField] bool canIGo;
    // Start is called before the first frame update
    void Start()
    {
        graph = FindObjectOfType<Graph>();
        pOI = graph.GetFirstPOI(BeingType.Car);
        transform.position = pOI.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        DrawPath();
        if (canIGo)
        RidePath(path,Time.deltaTime);
    }

    void DrawPath()
    {
        Vertex[] pathArray = path.ToArray();
        for(var i = 1; i < pathArray.Length; i++)
        {
            Debug.DrawLine(pathArray[i].transform.position, pathArray[i-1].transform.position, Color.magenta);
        }
    }

    public float GetPointCost(VertexType type)
    {
        switch(type)
        {
            case VertexType.Path:
                return pathCost;
            case VertexType.Direction:
                return directionCost;
            case VertexType.Road:
                return roadCost;
            case VertexType.PathDirection:
                return pathDirectionCost;
            case VertexType.PathDirectionRoad:
                return pathDirectionRoadCost;
            default:
                return -1f;
        }
    }

    public void NextPOI()
    {
        pOI = graph.GetNextPOI(pOI);
        transform.position = pOI.gameObject.transform.position;
    }

    public void PathToPoint()
    {
        path = graph.GetPathToNextPointOfInterest(pOI, this);
    }

    void RidePath(List<Vertex> path, float deltaTime)
    {
        if (path.Count == 1)
        {
            pOI = path.ToArray()[0];
        }
        if (path.Count>0)
        {
            GoToPoint(path.ToArray()[path.Count-1].gameObject.transform.position, speed * deltaTime);
            if ((path.ToArray()[path.Count - 1].gameObject.transform.position - transform.position).magnitude < accuracy)
            {
                path.RemoveAt(path.Count - 1);
            }
        }
        else
        {
            canIGo = false;
        }
    }

    void GoToPoint( Vector3 point, float speedTime)
    {
        transform.position += (point -transform.position).normalized* speedTime;
    }

    public void Move()
    {
        canIGo = true;
    }
}

