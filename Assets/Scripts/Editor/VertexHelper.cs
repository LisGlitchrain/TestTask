using System.Collections;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vertex))]
[ExecuteInEditMode]
public class VertexhHelper : Editor
{
    Vertex script;
    EditorWindow sceneWindow;
    int selectorCount = 0;
    //GameObject scriptObject;

    void OnEnable()
    {
        script = (Vertex) target;
        //scriptObject = script.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(Input.mousePosition, Vector3.zero, Color.red);
    }

    void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            selectorCount++;
            if (selectorCount>1)
            {
                Debug.Log("Selected");
                FindObjectOfType<VertexManager>().Instance.Vertices.Add(script);
                EditorUtility.SetDirty(FindObjectOfType<VertexManager>());
                selectorCount = 0;
            }
            //SceneView.RepaintAll();
        }
        //if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        //{
        //    Debug.Log("Left-Mouse Up");
        //}
        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            Debug.Log("Unselected");
            FindObjectOfType<VertexManager>().Instance.Vertices.Clear();
            EditorUtility.SetDirty(FindObjectOfType<VertexManager>());
        }
    }
}
