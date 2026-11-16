using UnityEngine;

public class PhobiaItem : ShopItem
{
    public int number;
    protected override void TryPurchase()
    {
        if (CoinCollect.instance.currentCoins >= cost)
        {
            Purchase();
        }
    }
    protected override void Purchase()
    {
        EventCore.instance.UE_RemovePhobia.Invoke(number);
        PhobiaEvents.RaisePhobiaRemoved(number);
        base.Purchase();
    }
}
