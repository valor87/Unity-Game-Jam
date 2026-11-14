using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Central place for non-player sound effects (slot machine, elevator, etc.).
/// Player movement audio stays in PlayerFootsteps.
/// Put this on a ROOT GameObject in the first scene; it persists across scene loads.
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    /// <summary>What the slot machine rolled. Decides which slot machine sound plays.</summary>
    public enum SlotResult
    {
        Normal,
        Jackpot777,
        Ominous666
    }

    /// <summary>Which version of the music is playing. The numbers match the Music State parameter in FMOD.</summary>
    public enum MusicState
    {
        Level = 0,    // drums version
        Elevator = 1, // no drums, quieter
        Shop = 2      // 8-bit version
    }

    [Header("Music")]
    [Tooltip("One 2D event with all three music tracks playing in sync, crossfaded by the Music State parameter.")]
    [SerializeField] private EventReference musicEvent;
    [Tooltip("Must match the parameter name in FMOD exactly (case-sensitive).")]
    [SerializeField] private string musicStateParameter = "Music State";
    [Tooltip("Start the music automatically when the game starts.")]
    [SerializeField] private bool autoStartMusic = true;

    [Header("Slot machine")]
    [Tooltip("Default slot machine sound, used for almost every roll. The elevator starts when the sound finishes.")]
    [SerializeField] private EventReference slotMachineEvent;
    [Tooltip("Plays instead of the default when the player rolls 777. Falls back to the default if empty.")]
    [SerializeField] private EventReference slotMachine777Event;
    [Tooltip("Plays instead of the default when the player rolls 666. Falls back to the default if empty.")]
    [SerializeField] private EventReference slotMachine666Event;
    [Tooltip("Seconds after the slot machine sound starts before the elevator starts. The slot machine sound keeps playing underneath. If a slot sound is shorter than this, the elevator starts when it finishes.")]
    [SerializeField] private float elevatorStartDelay = 6f;

    [Header("Elevator")]
    [Tooltip("One event: start up -> loop while 'Closed' -> transitions to the door-open sound when set to 'Open'.")]
    [SerializeField] private EventReference elevatorEvent;
    [Tooltip("Must match the labeled parameter name in FMOD exactly (case-sensitive).")]
    [SerializeField] private string elevatorStateParameter = "Elevator State";
    [Tooltip("Parameter label while the elevator is moving. Must match FMOD exactly.")]
    [SerializeField] private string closedLabel = "Closed";
    [Tooltip("Parameter label that triggers the door-open sound. Must match FMOD exactly.")]
    [SerializeField] private string openLabel = "Open";

    [Header("UI")]
    [Tooltip("One-shot played whenever a pop-up message appears (intro messages and phobia messages). Use a 2D event.")]
    [SerializeField] private EventReference popupEvent;

    [Header("Player")]
    [Tooltip("One-shot played when the player takes damage. Use a 2D event so it always sounds the same.")]
    [SerializeField] private EventReference playerDamageEvent;

    [Header("Obstacles")]
    [Tooltip("One-shot played when a spinning dummy launches its attack. Use a 3D event so it comes from the dummy.")]
    [SerializeField] private EventReference dummySwingEvent;
    [Tooltip("One-shot played each time a swinging axe passes the middle of its swing. Use a 3D event so it comes from the axe.")]
    [SerializeField] private EventReference axeSwingEvent;

    [Header("Lava level")]
    [Tooltip("Looping lava ambience. Plays from when the lava appears until the player enters the end elevator.")]
    [SerializeField] private EventReference lavaAmbienceEvent;
    [Tooltip("One-shot played when the player falls into the lava.")]
    [SerializeField] private EventReference lavaBurnEvent;

    [Header("Coins")]
    [Tooltip("One-shot played when the player collects a coin.")]
    [SerializeField] private EventReference coinCollectEvent;

    private EventInstance slotInstance;
    private bool slotActive;
    private Coroutine pendingElevatorStart;

    private EventInstance elevatorInstance;
    private bool elevatorPlaying; // true from loop start until the doors are opened

    private EventInstance musicInstance;
    private bool musicStarted;
    private MusicState currentMusicState = MusicState.Level;
    private MusicState stateBeforeShop = MusicState.Level;

    private EventInstance lavaInstance;
    private bool lavaInstancePlaying;
    private bool lavaLevelActive;

    /// <summary>True from when the lava appears until the player enters the end elevator.</summary>
    public bool IsLavaActive => lavaLevelActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // A duplicate manager destroys itself in Awake, so only the real one starts the music
        if (Instance == this && autoStartMusic)
            StartMusic();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;

            if (musicStarted)
            {
                musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                musicInstance.release();
                musicStarted = false;
            }

            StopLavaAmbience(immediate: true);
        }

        CancelPendingElevatorStart();

        if (elevatorPlaying)
        {
            elevatorInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            elevatorInstance.release();
            elevatorPlaying = false;
        }
    }

    // ---------------- Slot machine + elevator ----------------

    /// <summary>
    /// Call when the slot machine is pulled, AFTER the result is rolled. Plays the matching slot machine
    /// sound (default, 777 or 666), then starts the elevator the moment that sound finishes.
    /// </summary>
    public void PlaySlotMachineThenElevator(SlotResult result, Transform source = null)
    {
        CancelPendingElevatorStart();
        pendingElevatorStart = StartCoroutine(SlotMachineThenElevatorRoutine(result, source));
    }

    /// <summary>Plays the default slot machine sound. Kept so older calls without a result still work.</summary>
    public void PlaySlotMachineThenElevator(Transform source = null)
    {
        PlaySlotMachineThenElevator(SlotResult.Normal, source);
    }

    private EventReference GetSlotMachineEvent(SlotResult result)
    {
        switch (result)
        {
            case SlotResult.Jackpot777:
                return slotMachine777Event.IsNull ? slotMachineEvent : slotMachine777Event;
            case SlotResult.Ominous666:
                return slotMachine666Event.IsNull ? slotMachineEvent : slotMachine666Event;
            default:
                return slotMachineEvent;
        }
    }

    private IEnumerator SlotMachineThenElevatorRoutine(SlotResult result, Transform source)
    {
        EventReference chosenEvent = GetSlotMachineEvent(result);

        if (!chosenEvent.IsNull)
        {
            slotInstance = RuntimeManager.CreateInstance(chosenEvent);
            slotInstance.set3DAttributes(RuntimeUtils.To3DAttributes(GetPosition(source)));
            slotInstance.start();
            slotActive = true;

            float waited = 0f;
            while (waited < elevatorStartDelay)
            {
                slotInstance.getPlaybackState(out PLAYBACK_STATE state);
                if (state == PLAYBACK_STATE.STOPPED)
                    break;

                waited += Time.deltaTime;
                yield return null;
            }

            slotInstance.release();
            slotActive = false;
        }

        pendingElevatorStart = null;
        StartElevator(source);
    }

    private void CancelPendingElevatorStart()
    {
        if (pendingElevatorStart != null)
        {
            StopCoroutine(pendingElevatorStart);
            pendingElevatorStart = null;
        }

        // Let a still-playing slot machine sound finish on its own; just free it afterwards
        if (slotActive)
        {
            slotInstance.release();
            slotActive = false;
        }
    }

    // ---------------- Elevator ----------------

    /// <summary>
    /// Starts the elevator event with the state set to Closed (start up, then the loop).
    /// Usually called for you by PlaySlotMachineThenElevator.
    /// </summary>
    public void StartElevator(Transform source = null)
    {
        if (elevatorPlaying || elevatorEvent.IsNull)
            return;

        elevatorInstance = RuntimeManager.CreateInstance(elevatorEvent);
        elevatorInstance.set3DAttributes(RuntimeUtils.To3DAttributes(GetPosition(source)));
        elevatorInstance.setParameterByNameWithLabel(elevatorStateParameter, closedLabel);
        elevatorInstance.start();
        elevatorPlaying = true;
    }

    /// <summary>
    /// Call when the player opens the far elevator doors. Sets the state to Open so FMOD transitions
    /// from the loop into the door-open sound, then lets the event finish on its own.
    /// </summary>
    public void OpenElevatorDoors(Transform source = null)
    {
        CancelPendingElevatorStart(); // in case the doors are opened before the elevator ever started

        if (!elevatorPlaying)
            return;

        elevatorInstance.setParameterByNameWithLabel(elevatorStateParameter, openLabel);

        // release() lets the event play out (the door-open sound) and then frees it automatically
        elevatorInstance.release();
        elevatorPlaying = false;
    }

    // ---------------- Music ----------------

    /// <summary>Starts the music in the Level state. Called automatically unless Auto Start Music is off.</summary>
    public void StartMusic()
    {
        if (musicStarted || musicEvent.IsNull)
            return;

        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.setParameterByName(musicStateParameter, (float)MusicState.Level);
        musicInstance.start();
        musicStarted = true;
        currentMusicState = MusicState.Level;
    }

    /// <summary>Crossfades to another version of the music. FMOD's seek speed on the parameter sets how smooth it is.</summary>
    public void SetMusicState(MusicState state)
    {
        currentMusicState = state;

        if (!musicStarted)
            return;

        musicInstance.setParameterByName(musicStateParameter, (float)state);
    }

    /// <summary>Call when the shop menu opens: switches to the 8-bit version and remembers what to go back to.</summary>
    public void EnterShop()
    {
        if (currentMusicState != MusicState.Shop)
            stateBeforeShop = currentMusicState;

        SetMusicState(MusicState.Shop);
    }

    /// <summary>Call when the shop menu closes: goes back to whichever version was playing before.</summary>
    public void ExitShop()
    {
        SetMusicState(stateBeforeShop);
    }

    // ---------------- Coins ----------------

    /// <summary>Call at the moment a coin is collected. Pass the coin's transform so the sound plays from it.</summary>
    public void PlayCoinCollect(Transform source = null)
    {
        if (coinCollectEvent.IsNull)
            return;

        RuntimeManager.PlayOneShot(coinCollectEvent, GetPosition(source));
    }

    // ---------------- Lava level ----------------

    /// <summary>Call when the lava appears. Starts the lava ambience loop and marks the lava as active.</summary>
    public void StartLavaAmbience(Transform source = null)
    {
        lavaLevelActive = true;

        if (lavaInstancePlaying || lavaAmbienceEvent.IsNull)
            return;

        lavaInstance = RuntimeManager.CreateInstance(lavaAmbienceEvent);
        lavaInstance.set3DAttributes(RuntimeUtils.To3DAttributes(GetPosition(source)));
        lavaInstance.start();
        lavaInstancePlaying = true;
    }

    /// <summary>Call when the player enters the end elevator. Fades out the ambience. Safe to call when no lava is active.</summary>
    public void StopLavaAmbience(bool immediate = false)
    {
        lavaLevelActive = false;

        if (!lavaInstancePlaying)
            return;

        lavaInstance.stop(immediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        lavaInstance.release();
        lavaInstancePlaying = false;
    }

    /// <summary>Call when the player falls into the lava.</summary>
    public void PlayLavaBurn(Transform source = null)
    {
        if (lavaBurnEvent.IsNull)
            return;

        RuntimeManager.PlayOneShot(lavaBurnEvent, GetPosition(source));
    }

    // ---------------- UI ----------------

    /// <summary>Call at the moment a pop-up message appears on screen.</summary>
    public void PlayPopup()
    {
        if (popupEvent.IsNull)
            return;

        RuntimeManager.PlayOneShot(popupEvent, GetPosition(null));
    }

    // ---------------- Player ----------------

    /// <summary>Call when the player takes damage.</summary>
    public void PlayPlayerDamage()
    {
        if (playerDamageEvent.IsNull)
            return;

        // No source: plays at the camera, which suits a 2D "it hit me" sound
        RuntimeManager.PlayOneShot(playerDamageEvent, GetPosition(null));
    }

    // ---------------- Obstacles ----------------

    /// <summary>Call at the moment a spinning dummy launches its attack. Pass the dummy's transform so the sound comes from it.</summary>
    public void PlayDummySwing(Transform source)
    {
        if (dummySwingEvent.IsNull)
            return;

        RuntimeManager.PlayOneShot(dummySwingEvent, GetPosition(source));
    }

    /// <summary>Call each time a swinging axe passes the middle of its swing. Pass the axe's transform so the sound comes from it.</summary>
    public void PlayAxeSwing(Transform source)
    {
        if (axeSwingEvent.IsNull)
            return;

        RuntimeManager.PlayOneShot(axeSwingEvent, GetPosition(source));
    }

    // ---------------- Helpers ----------------

    // If no source is given, play at the camera (fine for 2D events or sounds that should feel "in the player's head")
    private Vector3 GetPosition(Transform source)
    {
        if (source != null)
            return source.position;

        return Camera.main != null ? Camera.main.transform.position : Vector3.zero;
    }
}