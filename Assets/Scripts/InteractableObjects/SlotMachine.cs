using UnityEngine;

public class SlotMachine : MonoBehaviour, IInteractable
{
    public void Exacute()
    {
        EventCore.instance.UE_EnterEndElevator.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
