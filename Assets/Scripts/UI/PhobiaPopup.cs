using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows a HUD popup near the center of the screen when the player contracts a phobia.
/// Listens to PhobiaEvents.PhobiaAdded, which AddPhobia raises with the phobia that was added.
/// Can fade away on its own, wait for a key press, or either.
/// Put this on an always-active object (NOT on the popup itself, which gets hidden).
/// </summary>
public class PhobiaPopup : MonoBehaviour
{
    public enum DismissMode
    {
        AutoFade,            // fades out after Display Duration
        PressKey,            // stays until the player presses the dismiss key
        AutoFadeOrPressKey   // whichever happens first
    }

    [System.Serializable]
    private class PhobiaEntry
    {
        [Tooltip("1-9 for the normal numbers. Use 777 and 666 for the two special rolls, which are their own separate phobias.")]
        public int number;
        [Tooltip("Number icon shown in the popup.")]
        public Sprite icon;
        [Tooltip("Title of the phobia, shown in the larger text above the description.")]
        public string title;
        [TextArea(2, 5)]
        [Tooltip("Description of what the phobia does, shown in smaller text below the title.")]
        public string description;
    }

    [Header("Phobia data (one entry per phobia: 1-9, plus 777 and 666)")]
    [SerializeField] private List<PhobiaEntry> entries = new List<PhobiaEntry>();

    [Header("UI references")]
    [Tooltip("The popup panel. Needs a CanvasGroup (used for fading). Place and size it however you like.")]
    [SerializeField] private CanvasGroup popup;
    [SerializeField] private Image icon;
    [Tooltip("Text for the phobia's title. Give this the larger font.")]
    [SerializeField] private TMP_Text titleText;
    [Tooltip("Text for the phobia's description. Give this the smaller font, below the title.")]
    [SerializeField] private TMP_Text description;
    [Tooltip("Optional object such as 'Press ENTER to continue'. Only shown when pressing a key can dismiss the popup.")]
    [SerializeField] private GameObject dismissPrompt;

    [Header("When to show")]
    [Tooltip("Seconds to wait after the phobia is added before the popup appears. Use this to sync with the slot machine reveal.")]
    [SerializeField] private float delayBeforeShow = 0f;

    [Header("How it goes away")]
    [SerializeField] private DismissMode dismissMode = DismissMode.AutoFade;
    [Tooltip("Seconds the popup stays up before fading (AutoFade modes).")]
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private KeyCode dismissKey = KeyCode.Return;
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private Coroutine showRoutine;

    private void Awake()
    {
        if (popup == null)
        {
            Debug.LogWarning("PhobiaPopup: no popup CanvasGroup assigned.");
            return;
        }

        if (popup.gameObject == gameObject)
            Debug.LogWarning("PhobiaPopup: put this script on a parent object, not on the popup itself, or it will be disabled with it.");

        popup.alpha = 0f;
        popup.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PhobiaEvents.PhobiaAdded += Show;
    }

    private void OnDisable()
    {
        PhobiaEvents.PhobiaAdded -= Show;
    }

    /// <summary>
    /// Shows the popup for a phobia (1-9, or 777 / 666). Normally triggered by PhobiaEvents.PhobiaAdded,
    /// but you can also call it yourself to test the layout.
    /// </summary>
    public void Show(int phobia)
    {
        if (popup == null)
            return;

        PhobiaEntry entry = entries.Find(e => e.number == phobia);
        if (entry == null)
        {
            Debug.LogWarning($"PhobiaPopup: no entry set up for phobia {phobia}.");
            return;
        }

        if (showRoutine != null)
            StopCoroutine(showRoutine);

        showRoutine = StartCoroutine(ShowRoutine(entry));
    }

    /// <summary>Hides the popup immediately.</summary>
    public void Hide()
    {
        if (showRoutine != null)
        {
            StopCoroutine(showRoutine);
            showRoutine = null;
        }

        if (popup != null)
        {
            popup.alpha = 0f;
            popup.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowRoutine(PhobiaEntry entry)
    {
        if (delayBeforeShow > 0f)
            yield return new WaitForSecondsRealtime(delayBeforeShow);

        if (icon != null)
        {
            icon.sprite = entry.icon;
            icon.enabled = entry.icon != null;
        }

        if (titleText != null)
            titleText.text = entry.title;

        if (description != null)
            description.text = entry.description;

        bool autoDismiss = dismissMode != DismissMode.PressKey;
        bool keyDismiss = dismissMode != DismissMode.AutoFade;

        if (dismissPrompt != null)
            dismissPrompt.SetActive(keyDismiss);

        popup.gameObject.SetActive(true);
        yield return Fade(0f, 1f, fadeInDuration);

        float timer = 0f;
        while (true)
        {
            if (keyDismiss && Input.GetKeyDown(dismissKey))
                break;

            if (autoDismiss)
            {
                timer += Time.unscaledDeltaTime; // keeps counting even if the game is paused
                if (timer >= displayDuration)
                    break;
            }

            yield return null;
        }

        yield return Fade(1f, 0f, fadeOutDuration);

        popup.gameObject.SetActive(false);
        showRoutine = null;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            popup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        popup.alpha = to;
    }
}