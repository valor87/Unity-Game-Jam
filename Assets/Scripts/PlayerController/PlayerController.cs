using NUnit.Framework;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera _camera; //for movement zoom out effects
    [Header("Camera units")]
    [SerializeField] private AnimationCurve cameraFOVMultiplier;
    [SerializeField] private float minSpeedForFOVChange, maxSpeedForFOVChange;
    private float currentCamFOV;

    [Header("Movement units")]
    [SerializeField] private float moveSpeed, airStrafeSpeed, maxMoveSpeed, jumpStrength, linearDampingAmount;
    private Vector3 moveDirection;
    private List<Transform> surfacesPlayerIsTouching = new List<Transform>();
    private bool isGrounded => surfacesPlayerIsTouching.Count > 0;

    private bool isFloorSlippery = false;

    private bool alreadyDoubleJumped, canDoubleJump = false;
    private void Start()
    {
        _camera = Camera.main;
        rb = GetComponent<Rigidbody>();

        EventCore.instance.UE_SetSlipperyFloors.AddListener(SetSlipperyFloors);
        EventCore.instance.UE_EnterEndElevator.AddListener(ResetSlipperyFloors);
        EventCore.instance.UE_EnableDoubleJump.AddListener(EnableDoubleJump);
        EventCore.instance.UE_EnterEndElevator.AddListener(DisableDoubleJump);
    }

    private void Update()
    {
        if (!isFloorSlippery)
        {
            rb.linearDamping = linearDampingAmount;
        }
        
        rb.maxLinearVelocity = maxMoveSpeed;
        if (!isGrounded)
        {
            rb.linearDamping = 0;
            rb.maxLinearVelocity = 700;
        }
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            if (isGrounded)
            {
                Jump();
                alreadyDoubleJumped = false;
            }
            else if (canDoubleJump && !alreadyDoubleJumped)
            {
                Jump();
                alreadyDoubleJumped = true;
            }
        }
        ChangeCameraFOVBasedOnSpeed();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (isGrounded)
        {
            MoveInDirection(direction);
        } else
        {
            AirStrafe(direction);
        }
    }
    void Jump()
    {
        //Collider[] colliders = Physics.OverlapSphere(transform.position - Vector3.up * 0.41f, 0.72f);
        List<Collider> colliders = new List<Collider>();
        foreach (Transform t in surfacesPlayerIsTouching)
        {
            colliders.Add(t.GetComponent<Collider>());
        }
        Vector3 meanPosition = Vector3.zero;

        foreach (Collider collider in colliders)
        {
            Vector3 closestPos = collider.ClosestPoint(transform.position - Vector3.up * 0.41f);
            Debug.Log("Closest pos: " + closestPos + " | name of obj: " + collider.name);
            meanPosition += closestPos;
        }
        if(colliders.Count>0) meanPosition /= colliders.Count;

        Debug.Log("mean position: " +  meanPosition);
        Vector3 direction = transform.position - meanPosition;
        Debug.Log("direction: " + direction);
        rb.linearVelocity += direction.normalized * jumpStrength;
    }
    void MoveInDirection(Vector3 _direction)
    {
        Vector3 direction = (transform.right * _direction.x + transform.forward * _direction.z);
        rb.linearVelocity += direction.normalized * moveSpeed * Time.deltaTime  /*+ transform.up * rb.linearVelocity.y*/;
    }
    void AirStrafe(Vector3 _direction)
    {
        rb.linearVelocity += (transform.right * _direction.x + transform.forward * _direction.z/2).normalized * airStrafeSpeed;
    }

    void ChangeCameraFOVBasedOnSpeed()
    {
        float speed = Mathf.InverseLerp(minSpeedForFOVChange, maxSpeedForFOVChange, rb.linearVelocity.magnitude);
        float targCameraFOV = Mathf.Lerp(60, 120, speed);
        currentCamFOV = Mathf.Lerp(currentCamFOV, targCameraFOV, 0.05f);
        _camera.fieldOfView = currentCamFOV;
    }
    //TRIGGER COLLISIONS CHECKING FOR SURFACES PLAYER IS COLLIDING WITH
    private void OnTriggerEnter(Collider other)
    {
        if (!surfacesPlayerIsTouching.Contains(other.transform) && !other.CompareTag("Coin"))
        {
            surfacesPlayerIsTouching.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (surfacesPlayerIsTouching.Contains(other.transform))
        {
            surfacesPlayerIsTouching.Remove(other.transform);
        }
    }
    private void SetSlipperyFloors()
    {
        isFloorSlippery = true;
    }
    private void ResetSlipperyFloors(Vector3 arg)
    {
        isFloorSlippery = false;
    }
    
    private void EnableDoubleJump()
    {
        canDoubleJump = true;
    }
    private void DisableDoubleJump(Vector3 arg)
    {
        canDoubleJump = false;
    }
}


