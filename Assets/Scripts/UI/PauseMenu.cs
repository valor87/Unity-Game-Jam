using UnityEngine;
using FMODUnity;

/// <summary>
/// Escape toggles the pause menu. Freezes the game, unlocks the cursor, and drops the
/// effect description banner down behind the three top-left banners.
/// Put this on an object that is ALWAYS active (not on the pause panel itself).
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>Other scripts (mouse look, input, etc.) can check this to ignore input while paused.</summary>
    public static bool IsPaused { get; private set; }

    [Header("References")]
    [Tooltip("The pause panel: dim background, volume slider, resume button. Hidden when not paused.")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private EffectBannerHUD bannerHUD;
    [SerializeField] private VolumeSlider volumeSlider;

    [Header("Settings")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [Tooltip("Optional FMOD bus to pause while the game is paused, for example bus:/SFX. Leave empty to keep audio running.")]
    [SerializeField] private string pausedBusPath = "";

    private CursorLockMode previousLockState;
    private bool previousCursorVisible;

    private void Start()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Applies the saved volume at game start, even though the slider's panel is hidden
        if (volumeSlider != null)
            volumeSlider.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }

    private void OnDestroy()
    {
        // Never leave the game frozen if this object is destroyed or the scene reloads while paused
        if (IsPaused)
            SetBusPaused(false);

        IsPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        if (IsPaused)
            return;

        IsPaused = true;
        Time.timeScale = 0f;

        previousLockState = Cursor.lockState;
        previousCursorVisible = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);
        if (bannerHUD != null)
            bannerHUD.SetExpanded(true);

        SetBusPaused(true);
    }

    /// <summary>Also hook this up to the Resume button's OnClick.</summary>
    public void Resume()
    {
        if (!IsPaused)
            return;

        IsPaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = previousLockState;
        Cursor.visible = previousCursorVisible;

        if (pausePanel != null)
            pausePanel.SetActive(false);
        if (bannerHUD != null)
            bannerHUD.SetExpanded(false);

        SetBusPaused(false);
    }

    private void SetBusPaused(bool paused)
    {
        if (string.IsNullOrEmpty(pausedBusPath))
            return;

        try
        {
            RuntimeManager.GetBus(pausedBusPath).setPaused(paused);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"PauseMenu: couldn't find FMOD bus '{pausedBusPath}'. {e.Message}");
        }
    }
}