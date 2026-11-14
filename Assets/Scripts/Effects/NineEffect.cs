using UnityEngine;

public class NineEffect : EffectNumber
{
    public override void GoodEffect()
    {

    }

    public override void BadEffect()
    {
        EventCore.instance.UE_FoggyLevel.Invoke();
    }
}