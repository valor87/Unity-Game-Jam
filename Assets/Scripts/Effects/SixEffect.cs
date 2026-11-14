using UnityEngine;

public class SixEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_EnableDoubleJump.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_StartWindEffect.Invoke();
    }
}
