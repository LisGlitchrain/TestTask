using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVisualizer : MonoBehaviour
{
    Graph graph;
    [SerializeField] LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        graph = FindObjectOfType<Graph>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] positions = new Vector3[graph.Vertices.Length];
        var i = 0;
        foreach (var vert in graph.Vertices)
        {
            positions[i] = vert.gameObject.transform.position;
            i++;
        }
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    void DrawEdge()
    {

    }
}
