using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Movement audio for a first-person Rigidbody player: footsteps, jump, and a looping
/// wall-run sound driven by an FMOD parameter. Sits next to PlayerController on the same GameObject.
/// Does not touch or depend on any of PlayerController's members; it only reads the
/// Rigidbody's velocity, the Space key, and nearby surfaces.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("FMOD Events")]
    [SerializeField] private EventReference footstepEvent;
    [Tooltip("Optional. Leave empty for no jump sound.")]
    [SerializeField] private EventReference jumpEvent;
    [Tooltip("Optional. Looping event with a 0-1 parameter (0 = silent, 1 = full volume). Starts at game start and stays running; only the parameter changes.")]
    [SerializeField] private EventReference wallRunEvent;
    [Tooltip("Must match the parameter name in FMOD exactly (case-sensitive).")]
    [SerializeField] private string wallRunParameterName = "WallRunning";

    [Header("Stride")]
    [Tooltip("Meters travelled between footsteps. Your player is fast, so try 2-4 and tune by ear.")]
    [SerializeField] private float strideLength = 2.5f;
    [Tooltip("Speed below this is ignored (no steps when barely moving).")]
    [SerializeField] private float minSpeed = 0.5f;
    [Tooltip("Hard cap on step rate so very high speeds don't turn into a machine gun.")]
    [SerializeField] private float minStepInterval = 0.2f;

    [Header("Surface check")]
    [Tooltip("Radius of the sphere around the player used to detect surfaces. Must reach the floor: about half the pill's height plus a margin.")]
    [SerializeField] private float surfaceCheckRadius = 1.2f;
    [Tooltip("Layers that count as surfaces. Exclude the Player layer if needed.")]
    [SerializeField] private LayerMask surfaceMask = ~0;
    [Tooltip("A surface whose normal points up more than this counts as floor. Anything steeper counts as wall.")]
    [SerializeField, Range(0f, 1f)] private float floorMinNormalY = 0.5f;
    [Tooltip("A wall only counts when it's this close to the player's center. About the pill's radius plus a small margin.")]
    [SerializeField] private float wallContactDistance = 0.7f;

    [Header("Wall run")]
    [Tooltip("Speed needed along the wall for the wall-run sound to turn on.")]
    [SerializeField] private float wallRunMinSpeed = 3f;
    [Tooltip("Grace period (seconds) before the sound turns off, so tiny gaps in contact don't make it flicker.")]
    [SerializeField] private float wallRunStopDelay = 0.1f;
    [Tooltip("If off, no footsteps play while on a wall (the wall-run sound replaces them).")]
    [SerializeField] private bool footstepsOnWalls = false;

    [Header("Landing")]
    [Tooltip("Must be airborne at least this long for a landing footstep to play.")]
    [SerializeField] private float minAirTimeForLanding = 0.25f;

    private Rigidbody rb;
    private readonly Collider[] hitBuffer = new Collider[8];
    private float distanceAccumulated;
    private float lastStepTime;
    private float airTime;
    private bool wasTouching;

    private EventInstance wallRunInstance;
    private bool wallRunInstanceCreated;
    private bool wallRunOn;
    private float lastWallContactTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CheckSurfaces(out bool floor, out bool wall);
        wasTouching = floor || wall;

        // The wall-run event runs continuously from the start; only its parameter changes
        if (!wallRunEvent.IsNull)
        {
            wallRunInstance = RuntimeManager.CreateInstance(wallRunEvent);
            wallRunInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            wallRunInstance.setParameterByName(wallRunParameterName, 0f);
            wallRunInstance.start();
            wallRunInstanceCreated = true;
        }
    }

    private void Update()
    {
        CheckSurfaces(out bool touchingFloor, out bool touchingWall);
        bool touching = touchingFloor || touchingWall;
        float speed = rb.linearVelocity.magnitude;

        // Jump: same input and condition PlayerController uses (Space while touching a surface)
        if (Input.GetKeyDown(KeyCode.Space) && touching)
        {
            if (!jumpEvent.IsNull)
                RuntimeManager.PlayOneShot(jumpEvent, transform.position);
            lastStepTime = Time.time; // keeps a footstep from landing right on top of the jump
            SetWallRun(false);        // jumping off the wall turns the wall-run sound off immediately
        }

        UpdateWallRun(touchingWall, touchingFloor, speed);

        // Airtime / landing
        bool canStep = touchingFloor || (touchingWall && footstepsOnWalls);

        if (!touching)
        {
            airTime += Time.deltaTime;
        }
        else
        {
            // Landing = just play another footstep
            if (!wasTouching && airTime >= minAirTimeForLanding && canStep)
            {
                PlayFootstep();
                distanceAccumulated = 0f;
            }
            airTime = 0f;
        }
        wasTouching = touching;

        // Footsteps
        if (!canStep || speed < minSpeed)
            return;

        distanceAccumulated += speed * Time.deltaTime;

        if (distanceAccumulated >= strideLength && Time.time - lastStepTime >= minStepInterval)
        {
            distanceAccumulated = 0f;
            PlayFootstep();
        }
    }

    private void LateUpdate()
    {
        // Keep the wall-run sound positioned on the player
        if (wallRunInstanceCreated)
            wallRunInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
    }

    private void OnDisable()
    {
        SetWallRun(false);
    }

    private void OnDestroy()
    {
        if (!wallRunInstanceCreated)
            return;

        wallRunInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        wallRunInstance.release();
        wallRunInstanceCreated = false;
    }

    // ---------------- Wall run ----------------

    private void UpdateWallRun(bool touchingWall, bool touchingFloor, float speed)
    {
        // Wall running = on a wall, not on the floor, moving fast enough
        bool wantOn = touchingWall && !touchingFloor && speed >= wallRunMinSpeed;

        if (wantOn)
        {
            lastWallContactTime = Time.time;
            SetWallRun(true);
        }
        else if (wallRunOn && Time.time - lastWallContactTime > wallRunStopDelay)
        {
            SetWallRun(false);
        }
    }

    // Sets the FMOD parameter to 1 (sound on) or 0 (sound off). Only talks to FMOD when the state changes.
    private void SetWallRun(bool on)
    {
        if (!wallRunInstanceCreated || wallRunOn == on)
            return;

        wallRunInstance.setParameterByName(wallRunParameterName, on ? 1f : 0f);
        wallRunOn = on;
    }

    // ---------------- Helpers ----------------

    private void PlayFootstep()
    {
        lastStepTime = Time.time;
        RuntimeManager.PlayOneShot(footstepEvent, transform.position);
    }

    // Select the player in the Scene view to see the check spheres
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, surfaceCheckRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wallContactDistance);
    }

    /// <summary>
    /// Finds nearby surfaces and classifies them as floor or wall by the direction
    /// from the surface's closest point to the player.
    /// </summary>
    private void CheckSurfaces(out bool floor, out bool wall)
    {
        floor = false;
        wall = false;
        Vector3 pos = transform.position;

        int count = Physics.OverlapSphereNonAlloc(pos, surfaceCheckRadius, hitBuffer,
                                                  surfaceMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < count; i++)
        {
            Collider col = hitBuffer[i];

            // Skip the player's own colliders
            if (col.transform.IsChildOf(transform))
                continue;

            Vector3 away = pos - col.ClosestPoint(pos);
            float distance = away.magnitude;

            // Player center is inside the collider; treat as floor so we never go silent
            if (distance < 0.0001f)
            {
                floor = true;
                continue;
            }

            float normalY = away.y / distance;

            if (normalY > floorMinNormalY)
            {
                floor = true;
            }
            else if (normalY > -floorMinNormalY && distance <= wallContactDistance)
            {
                wall = true; // ceilings (normalY very negative) are ignored
            }
        }
    }
}