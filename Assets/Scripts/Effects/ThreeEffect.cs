using UnityEngine;

public class ThreeEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_UsedShield.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_TrippleDamge.Invoke();
    }
}
