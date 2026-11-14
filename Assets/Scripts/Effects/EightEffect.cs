using UnityEngine;

public class EightEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_EnableSuperJumpOffWalls.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_EnableBouncyWalls.Invoke();
    }
}
