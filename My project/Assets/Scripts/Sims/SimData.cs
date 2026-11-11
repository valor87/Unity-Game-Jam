using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Data class used for propagating the details of a Sim when creating them in the simulation.
[Serializable]
public class SimData
{
    public string simName => _simName;

    [SerializeField]
    private string _simName;

    //How old the Sim is.
    public int age => _age;

    [SerializeField]
    private int _age;

    //Data used to identify specific types of Traits that Sim entities can have.
    public enum Trait
    {
        ambitious, cheerful, childish, clumsy, creative, erratic, genius
    }
    //The specific list of Traits on this Sim that are used to dictate how they will hypothetically behave. 
    public List<Trait> traits => _traits;

    [SerializeField]
    private List<Trait> _traits = new List<Trait>();

    [SerializeField]
    private List<NeedData> needs = new List<NeedData>();


    public Dictionary<NeedData.NeedType, NeedData> needsMap
    {
        get
        {
            if (_needsMap == null || _needsMap.Count == 0)
            {
                InitializeNeeds();
            }
            return _needsMap;
        }
    }
    //The specific map of Needs on this Sim, along with their initial values.
    private Dictionary<NeedData.NeedType, NeedData> _needsMap;

    //The colour associated with this Sim.
    public Color simColour => _simColour;

    [SerializeField]
    private Color _simColour;

    public NeedData GetNeed(NeedData.NeedType type)
    {
        if(!needsMap.ContainsKey(type))
        {
            return null;
        }
        return needsMap[type];
    }

    private void InitializeNeeds()
    {
        _needsMap = new Dictionary<NeedData.NeedType, NeedData>();
        foreach(NeedData need in needs)
        {
            if(_needsMap.ContainsKey(need.needType))
            {
                continue;
            }
            _needsMap.Add(need.needType, need);
        }
    }


}
