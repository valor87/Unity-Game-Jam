using UnityEngine;

public class FiveEffect : EffectNumber
{
    public override void GoodEffect()
    {
        
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_SetSlipperyFloors.Invoke();
    }
}
