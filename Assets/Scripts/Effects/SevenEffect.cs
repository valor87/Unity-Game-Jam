using UnityEngine;

public class SevenEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_SpawnTwoCoins.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_SpawnNoCoins.Invoke();
    }
}
