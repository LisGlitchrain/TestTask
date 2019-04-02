using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Vertex : MonoBehaviour, IEquatable<Vertex>
{
    [SerializeField] List<Vertex> linkedVetrices = new List<Vertex>();
    public List<Vertex> LinkedVertices { get { return linkedVetrices; } set { linkedVetrices = value; } }
    [SerializeField] VertexType type;
    public VertexType Type { get { return type; } set { type = value; } }
    [SerializeField] bool pointOfInterest = false;
    public bool PointOfInterest { get { return pointOfInterest; } set { pointOfInterest = value; } }
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.FindObjectOfType<Graph>().Vertices.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Equals(Vertex other)
    {
        return gameObject.transform.position.Equals(other.transform.position);

    }
}
