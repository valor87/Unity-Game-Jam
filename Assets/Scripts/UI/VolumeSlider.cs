using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

/// <summary>
/// Put this on the volume Slider in the pause menu. Controls an FMOD bus and remembers the setting.
/// PauseMenu calls Initialize() at game start, so the saved volume applies even before the menu is opened.
/// </summary>
[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [Tooltip("FMOD bus to control. bus:/ is the master bus (everything).")]
    [SerializeField] private string busPath = "bus:/";
    [Tooltip("Key used to save the setting between sessions.")]
    [SerializeField] private string playerPrefsKey = "MasterVolume";
    [SerializeField, Range(0f, 1f)] private float defaultVolume = 1f;

    private Slider slider;
    private FMOD.Studio.Bus bus;
    private bool initialized;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (initialized)
            return;

        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;

        try
        {
            bus = RuntimeManager.GetBus(busPath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"VolumeSlider: couldn't find FMOD bus '{busPath}'. {e.Message}");
            return;
        }

        float saved = PlayerPrefs.GetFloat(playerPrefsKey, defaultVolume);
        slider.SetValueWithoutNotify(saved);
        bus.setVolume(saved);

        slider.onValueChanged.AddListener(OnSliderChanged);
        initialized = true;
    }

    private void OnSliderChanged(float value)
    {
        bus.setVolume(value);
        PlayerPrefs.SetFloat(playerPrefsKey, value);
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderChanged);
    }
}