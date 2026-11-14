using UnityEngine;

public class TwoEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_DoubleObsticalSpeed.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_HalfObsticalSpeed.Invoke();
    }
}
