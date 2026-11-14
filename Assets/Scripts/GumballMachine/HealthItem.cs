using UnityEngine;

public class HealthItem : ShopItem
{
    public override void purchase()
    {
        EventCore.instance.UE_RefillPlayerHealth.Invoke();
        base.purchase();
    }
}
