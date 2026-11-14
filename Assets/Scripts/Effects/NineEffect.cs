using UnityEngine;

public class NineEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_Cloud9.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_EnableFog.Invoke();
    }
}