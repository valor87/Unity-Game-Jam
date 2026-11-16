using UnityEngine;

public class ThreeEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_GivePlayerSheild.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_TrippleDamge.Invoke();
    }
}
