using UnityEngine;

public class FourEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_HalfDamage.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_DoubleDamage.Invoke();
    }
}
