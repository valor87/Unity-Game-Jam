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
    [SerializeField] private float moveSpeed, airStrafeSpeed, maxMoveSpeed, jumpStrength;
    private Vector3 moveDirection;
    private List<Transform> surfacesPlayerIsTouching = new List<Transform>();
    private bool isGrounded => surfacesPlayerIsTouching.Count > 0;

    private void Start()
    {
        _camera = Camera.main.GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxMoveSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(rb.linearVelocity);
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
        Collider[] hits = Physics.OverlapSphere(transform.position - Vector3.up * 0.41f, 0.72f);
        rb.linearVelocity += Vector3.up * jumpStrength;
    }
    void MoveInDirection(Vector3 _direction)
    {
        Vector3 direction = (transform.right * _direction.x + transform.forward * _direction.z);
        rb.linearVelocity = direction.normalized * moveSpeed + transform.up * rb.linearVelocity.y;
    }
    void AirStrafe(Vector3 _direction)
    {
        rb.linearVelocity += (transform.right * _direction.x + transform.forward * _direction.z).normalized * airStrafeSpeed;
    }

    //TRIGGER COLLISIONS CHECKING FOR SURFACES PLAYER IS COLLIDING WITH
    private void OnTriggerEnter(Collider other)
    {
        if (!surfacesPlayerIsTouching.Contains(other.transform))
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

}
