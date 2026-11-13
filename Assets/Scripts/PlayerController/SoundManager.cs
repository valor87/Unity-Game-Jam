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

    private EventInstance slotInstance;
    private bool slotActive;
    private Coroutine pendingElevatorStart;

    private EventInstance elevatorInstance;
    private bool elevatorPlaying; // true from loop start until the doors are opened

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

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;

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

    // ---------------- Helpers ----------------

    // If no source is given, play at the camera (fine for 2D events or sounds that should feel "in the player's head")
    private Vector3 GetPosition(Transform source)
    {
        if (source != null)
            return source.position;

        return Camera.main != null ? Camera.main.transform.position : Vector3.zero;
    }
}