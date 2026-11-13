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
    }
    public void RunEffect(int number)
    {

    }
}
