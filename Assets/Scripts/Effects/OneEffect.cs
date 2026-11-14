using UnityEngine;
using System.Diagnostics;

[System.Serializable]
public class OneEffect : EffectNumber
{
    public override void GoodEffect()
    {
        EventCore.instance.UE_HealWhenMoving.Invoke();
        UnityEngine.Debug.Log("Heal effect in play");
    }

    public override void BadEffect()
    {
        EventCore.instance.UE_TakeDamageWhenStill.Invoke();
    }
}
