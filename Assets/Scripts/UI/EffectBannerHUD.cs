using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the three top-left effect banners and the larger text banner that drops down
/// behind them while the game is paused.
/// Put this on the HUD object that contains the banners. It must stay active so it can listen for effects.
/// </summary>
public class EffectBannerHUD : MonoBehaviour
{
    [System.Serializable]
    private class BannerSlot
    {
        [Tooltip("The small banner. Hidden until an effect fills it.")]
        public GameObject root;
        [Tooltip("The Image inside the banner that shows the effect icon.")]
        public Image icon;
        [Tooltip("The text inside the big pause-menu banner that describes this effect.")]
        public TMP_Text description;
    }

    [Header("Banners (fill in order: 1st, 2nd, 3rd number)")]
    [SerializeField] private BannerSlot[] slots = new BannerSlot[3];

    [Header("Pause menu details banner")]
    [Tooltip("The big banner that drops down behind the three small ones. Place it BEFORE (behind) the small banners in the hierarchy.")]
    [SerializeField] private RectTransform detailsPanel;
    [SerializeField] private float expandDuration = 0.3f;
    [SerializeField] private AnimationCurve expandCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private int filled;
    private Coroutine expandRoutine;
    private CanvasGroup detailsCanvasGroup;
    private Vector2 shownPosition;
    private float progress; // 0 = tucked away behind the banners, 1 = fully dropped down

    private void Awake()
    {
        if (detailsPanel == null)
            return;

        // Whatever position you set up in the editor is the "dropped down" position
        shownPosition = detailsPanel.anchoredPosition;

        detailsCanvasGroup = detailsPanel.GetComponent<CanvasGroup>();
        if (detailsCanvasGroup == null)
            detailsCanvasGroup = detailsPanel.gameObject.AddComponent<CanvasGroup>();

        ApplyDetailsProgress(0f);
        detailsPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        ClearBanners();
    }

    private void OnEnable()
    {
        EffectNumber.EffectTriggered += OnEffectTriggered;
    }

    private void OnDisable()
    {
        EffectNumber.EffectTriggered -= OnEffectTriggered;
    }

    // ---------------- Banners ----------------

    private void OnEffectTriggered(EffectNumber effect, bool isBad)
    {
        // A new set of three effects is starting: wipe the previous level's banners
        if (filled >= slots.Length)
            ClearBanners();

        BannerSlot slot = slots[filled];
        filled++;

        Sprite sprite = effect.GetIcon(isBad);
        if (slot.icon != null)
        {
            slot.icon.sprite = sprite;
            slot.icon.enabled = sprite != null;
        }

        if (slot.description != null)
            slot.description.text = effect.GetDescription(isBad);

        if (slot.root != null)
            slot.root.SetActive(true);
    }

    /// <summary>Hides all three banners and clears their text. Call this if you ever need to reset manually.</summary>
    public void ClearBanners()
    {
        filled = 0;

        foreach (BannerSlot slot in slots)
        {
            if (slot.root != null)
                slot.root.SetActive(false);
            if (slot.description != null)
                slot.description.text = string.Empty;
        }
    }

    // ---------------- Pause menu expansion ----------------

    /// <summary>True drops the details banner down behind the three banners, false tucks it away.</summary>
    public void SetExpanded(bool expanded)
    {
        if (detailsPanel == null)
            return;

        if (expandRoutine != null)
            StopCoroutine(expandRoutine);

        expandRoutine = StartCoroutine(AnimateDetails(expanded ? 1f : 0f));
    }

    private IEnumerator AnimateDetails(float target)
    {
        detailsPanel.gameObject.SetActive(true);

        float start = progress;
        float elapsed = 0f;

        // unscaledDeltaTime: the game is paused (timeScale 0) while this plays
        while (elapsed < expandDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float k = expandCurve.Evaluate(Mathf.Clamp01(elapsed / expandDuration));
            ApplyDetailsProgress(Mathf.Lerp(start, target, k));
            yield return null;
        }

        ApplyDetailsProgress(target);

        if (target <= 0f)
            detailsPanel.gameObject.SetActive(false);

        expandRoutine = null;
    }

    private void ApplyDetailsProgress(float value)
    {
        progress = value;

        // Slides up behind the small banners when hidden, drops down when shown
        Vector2 hiddenPosition = shownPosition + Vector2.up * detailsPanel.rect.height;
        detailsPanel.anchoredPosition = Vector2.LerpUnclamped(hiddenPosition, shownPosition, value);
        detailsCanvasGroup.alpha = value;
    }
}