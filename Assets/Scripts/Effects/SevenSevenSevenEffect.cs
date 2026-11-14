using UnityEngine;

public class SevenSevenSevenEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_JackpotEffect.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_JackpotEffect.Invoke();
    }
}
