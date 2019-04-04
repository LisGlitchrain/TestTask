using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] Dropdown targetDrop;
    [SerializeField] Dropdown currentPOI;
    [SerializeField] Dropdown beingType;
    Graph graph;
    Being being;
    bool initialized;


    void Start()
    {
        graph = FindObjectOfType<Graph>();
        being = FindObjectOfType<Being>();
    }

    private void Update()
    {
        if (graph.IsInitialized && !initialized)
        {

            targetDrop.ClearOptions();
            currentPOI.ClearOptions();
            beingType.ClearOptions();
            List<Dropdown.OptionData> optionsTarget = new List<Dropdown.OptionData>();
            foreach (var poi in graph.POI)
            {
                optionsTarget.Add(new Dropdown.OptionData(poi.gameObject.name));
            }
            targetDrop.AddOptions(optionsTarget);
            List<Dropdown.OptionData> optionsCurrent = new List<Dropdown.OptionData>();
            foreach (var poi in graph.POI)
            {
                optionsCurrent.Add(new Dropdown.OptionData(poi.gameObject.name));
            }
            currentPOI.AddOptions(optionsCurrent);
            currentPOI.value = graph.POI.IndexOf(being.POI);
            List<Dropdown.OptionData> optionsType = new List<Dropdown.OptionData>();
            foreach (var type in Enum.GetNames(typeof(BeingType)))
            {
                optionsType.Add(new Dropdown.OptionData(type));
            }
            beingType.AddOptions(optionsType);
            beingType.value = (int) being.Type;
        }
    }

    public void ChangeCurrentPOI()
    {
        being.ChangeCurrentPOI(graph.POI[currentPOI.value]);
    }

    public void ChangeTargetPOI()
    {
        being.ChangeTargetPOI(graph.POI[targetDrop.value]);
    }

    public void ChangeBeingType()
    {
        being.ChangeType(beingType.options.ToArray()[beingType.value].text);
    }
}
