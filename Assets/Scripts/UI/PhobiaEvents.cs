using System;

/// <summary>
/// Announces when the player contracts a phobia. AddPhobia calls RaisePhobiaAdded(number) once
/// per phobia actually added, and anything interested (like PhobiaPopup) listens to PhobiaAdded.
/// </summary>
public static class PhobiaEvents
{
    /// <summary>Phobia id for a 777 roll. It is its own phobia, separate from 7.</summary>
    public const int Phobia777 = 777;

    /// <summary>Phobia id for a 666 roll. It is its own phobia, separate from 6.</summary>
    public const int Phobia666 = 666;

    /// <summary>Fired with the phobia that was added: 1-9, or 777 / 666 for the special rolls.</summary>
    public static event Action<int> PhobiaAdded;

    /// <summary>Call this from AddPhobia, right when a phobia is added.</summary>
    public static void RaisePhobiaAdded(int phobia)
    {
        PhobiaAdded?.Invoke(phobia);
    }
}