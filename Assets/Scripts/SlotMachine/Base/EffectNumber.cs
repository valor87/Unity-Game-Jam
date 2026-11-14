using System;
using UnityEngine;

[System.Serializable]
public class EffectNumber : MonoBehaviour
{
    [SerializeField] int number;

    [Header("HUD banner icons")]
    [Tooltip("Icon shown in the top-left banner when this number's GOOD effect is active.")]
    [SerializeField] private Sprite goodIcon;
    [Tooltip("Icon shown in the top-left banner when this number's BAD effect is active (player already has this phobia).")]
    [SerializeField] private Sprite badIcon;

    [Header("Pause menu descriptions")]
    [Tooltip("Text shown in the pause menu while the GOOD effect is active.")]
    [SerializeField, TextArea(2, 4)] private string goodDescription;
    [Tooltip("Text shown in the pause menu while the BAD effect is active.")]
    [SerializeField, TextArea(2, 4)] private string badDescription;

    public int Number => number;
    public Sprite GetIcon(bool isBad) => isBad ? badIcon : goodIcon;
    public string GetDescription(bool isBad) => isBad ? badDescription : goodDescription;

    /// <summary>
    /// Fired every time an effect runs. The bool is true if the BAD effect is being applied.
    /// The banner HUD listens to this, so PhobiaManager doesn't need to know about the UI.
    /// </summary>
    public static event Action<EffectNumber, bool> EffectTriggered;

    public virtual void DoEffect()
    {
        bool isBad = PhobiaManager.instance.Phobias[number];

        // Announce the result first, so the HUD shows what was decided at this moment
        EffectTriggered?.Invoke(this, isBad);

        if (isBad)
        {
            BadEffect();
        }
        else
        {
            GoodEffect();
        }
    }

    public virtual void GoodEffect() { }
    public virtual void BadEffect() { }
}