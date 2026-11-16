using UnityEngine;

public class HealthItem : ShopItem
{
    protected override void TryPurchase()
    {
        if (CoinCollect.instance.currentCoins >= cost)
        {
            Purchase();
        }
    }
    protected override void Purchase()
    {
        EventCore.instance.UE_RefillPlayerHealth.Invoke();
        base.Purchase();
    }
}
