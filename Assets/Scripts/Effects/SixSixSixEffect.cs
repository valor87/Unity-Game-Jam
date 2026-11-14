using UnityEngine;

public class SixSixSixEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_666Effect.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_666Effect.Invoke();
    }
}
