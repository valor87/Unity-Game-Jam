using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class NeedData
{
    public bool IsInitialized => isInitialized;
    private bool isInitialized = false;

    //Data used to identify the specific types of Needs that Sim entities can have.
    public enum NeedType
    {
        hunger, social, fun, comfort, hygiene, room, bladder, energy
    }

    public NeedType needType;

    public float initialValue;

    public float decrementValue;

    public float maxValue;

    public float currentValue;

    public bool hasWarning;

    public void Initialize()
    {
        currentValue = initialValue;
        isInitialized = true;
    }

    public void IncreaseValue(float value)
    {
        currentValue = Mathf.Min(currentValue + value, maxValue);
    }

}
