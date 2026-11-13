using System.Collections;
using UnityEngine;
public class SlotMachine : MonoBehaviour
{
    public static SlotMachine Instance;
    [SerializeField] public slotOutcome[] numberOutcomes = new slotOutcome[9];

    private void Start()
    {
        Instance = this;
    }
    
}

[System.Serializable]
public struct slotOutcome
{
    public int number;
    public EffectNumber effect;
}

