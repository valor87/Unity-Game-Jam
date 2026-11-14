using UnityEngine;

public class PhobiaItem : ShopItem
{
    public int number;
    public override void purchase()
    {
        EventCore.instance.UE_RemovePhobia.Invoke(number);
        base.purchase();
    }
}
