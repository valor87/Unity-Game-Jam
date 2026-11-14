using System.Collections.Generic;
using UnityEngine;

public class PhobiaManager : MonoBehaviour
{
    public static PhobiaManager instance;
    public Dictionary<int, bool>Phobias = new Dictionary<int, bool>();
    public List<EffectNumber> Effects = new List<EffectNumber>();

    private void Start()
    {
        instance = this;
        for(int i=1; i< 10; i++)
        {
            Phobias.Add(i, false);
            Debug.Log("Phobia added: " + i);
        }
        EventCore.instance.UE_RemovePhobia.AddListener(RemovePhobia);
    }
    public void StartAllActiveEffects()
    {
        RunEffect(SlotMachine.Instance.firstNumber);
        RunEffect(SlotMachine.Instance.secondNumber);
        RunEffect(SlotMachine.Instance.thirdNumber);
    }
    private void RunEffect(int number)
    {
        Effects[number - 1].DoEffect();
    }
    public void AddPhobia(int number)
    {
        Phobias[number] = true;
        PhobiaEvents.RaisePhobiaAdded(number);
    }
    public void RemovePhobia(int number)
    {
        Phobias[number] = false;
    }
}
