
using UnityEngine;

[System.Serializable]
public class EffectNumber : MonoBehaviour
{
    [SerializeField] int number;
    public virtual void DoEffect()
    {
        if (PhobiaManager.instance.Phobias[number])
        {
            BadEffect();
        } else
        {
            GoodEffect();
        }
    }
    public virtual void GoodEffect() { }
    public virtual void BadEffect() { }
}
