using UnityEngine;


[System.Serializable]
public struct slotOutcomes
{
    public int numebr;
    public EffectNumber effect;
}

public class SlotMachine : MonoBehaviour, IInteractable
{
    private Transform _pT;

    [SerializeField] public slotOutcomes[] numberOutcomes = new slotOutcomes[9];
    public void Exacute()
    {
        Vector3 playerOffset = transform.position - _pT.position;
        EventCore.instance.UE_EnterEndElevator.Invoke(playerOffset);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pT = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
