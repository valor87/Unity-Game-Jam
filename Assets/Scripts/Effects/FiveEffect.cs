using UnityEngine;

public class FiveEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_FreezeObstacles.Invoke();
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_SetSlipperyFloors.Invoke();
    }
}
