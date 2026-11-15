using UnityEngine;

public class GumballMachineInteract : MonoBehaviour, IInteractable
{
    [SerializeField] ShopManager shopManager;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shopManager.CloseShop();
        }
        
    }
    public void Exacute()
    {
        if (!shopManager.isShopOpen)
        {
            shopManager.OpenShop();
        }
    }

    public bool isHoldToInteract()
    {
        return false;
    }
    public bool shouldShowInteractSign()
    {
        return true;
    }
}
