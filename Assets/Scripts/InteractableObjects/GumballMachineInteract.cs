using UnityEngine;

public class GumballMachineInteract : MonoBehaviour, IInteractable
{
    [SerializeField] ShopManager shopManager;
    public void Exacute()
    {
        if (shopManager.isShopOpen)
        {
            shopManager.CloseShop();
        }
        else
        {
            shopManager.OpenShop();
        }
    }
}
