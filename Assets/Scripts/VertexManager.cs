using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class VertexManager : MonoBehaviour
{
    VertexManager instance;
    public VertexManager Instance { get { return instance; } }
    public List<Vertex> Vertices = new List<Vertex>();
    public Graph graph;
    [SerializeField] float selectionRadius = 0.5f;
    // Start is called before the first frame update
    void Start()
    {

        instance = this;
        graph = FindObjectOfType<Graph>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vertices.Count == 2 && !CheckDoubleClick())
        {
            if (!Vertices.ToArray()[0].LinkedVertices.Contains(Vertices.ToArray()[1]))
            {
                Vertices.ToArray()[0].LinkedVertices.Add(Vertices.ToArray()[1]);
                Vertices.ToArray()[1].LinkedVertices.Add(Vertices.ToArray()[0]);
                Vertices.Clear();
                print("Linked Deselected");
                graph.Initialize();
                graph.Update();
                OnDrawGizmos();
            }
            else
            {
                Vertices.ToArray()[0].LinkedVertices.Remove(Vertices.ToArray()[1]);
                Vertices.ToArray()[1].LinkedVertices.Remove(Vertices.ToArray()[0]);
                Vertices.Clear();
                print("Unlinked soft Deselected");
                graph.Initialize();
                graph.Update();
                OnDrawGizmos();
            }

        }
        else if (Vertices.Count == 2 && CheckDoubleClick())
        {
            ClearLinks(Vertices.ToArray()[0]);
            Vertices.Clear();
            print("Unlinked hard Deselected");
            graph.Initialize();
            graph.Update();
            OnDrawGizmos();

        }
        else if (Vertices.Count>2)
        {
            Vertices.Clear();
        } 
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        foreach (var vertex in Vertices)
        {
            Handles.DrawWireDisc(vertex.transform.position, Vector3.up, selectionRadius);
        }
    }

    bool CheckDoubleClick()
    {
        return Vertices.ToArray()[0].Equals( Vertices.ToArray()[1]);
    }

    void ClearLinks(Vertex vertex)
    {
        foreach(var vert in vertex.LinkedVertices)
        {
            vert.LinkedVertices.Remove(vertex);
        }
        vertex.LinkedVertices.Clear();
    }

}
