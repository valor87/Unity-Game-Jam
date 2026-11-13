using UnityEngine;

public class OneEffect : EffectNumber
{
    public override void GoodEffect()
    {
        
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_TakeDamageWhenStill.Invoke();
    }
}
