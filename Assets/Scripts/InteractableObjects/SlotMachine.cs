using UnityEngine;
public class SlotMachine : MonoBehaviour, IInteractable
{
    private Transform playerTransform;

    [SerializeField] public slotOutcomes[] numberOutcomes = new slotOutcomes[9];

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }
    public void Exacute()
    {
        Vector3 playerOffset = transform.position - playerTransform.position;
        EventCore.instance.UE_EnterEndElevator.Invoke(playerOffset);
    }
}

[System.Serializable]
public struct slotOutcomes
{
    public int numebr;
    public EffectNumber effect;
}

