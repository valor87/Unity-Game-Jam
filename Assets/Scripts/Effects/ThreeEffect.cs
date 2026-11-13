using UnityEngine;

public class ThreeEffect : EffectNumber
{
    public override void GoodEffect()
    {

    }

    public override void BadEffect()
    {
        EventCore.instance.UE_TrippleDamge.Invoke();
    }
}
