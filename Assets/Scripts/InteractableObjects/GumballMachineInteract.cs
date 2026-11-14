using UnityEngine;

public class GumballMachineInteract : MonoBehaviour, IInteractable
{
    [SerializeField] ShopManager shopManager;
    public void Exacute()
    {
        shopManager.OpenShop();
    }
}
