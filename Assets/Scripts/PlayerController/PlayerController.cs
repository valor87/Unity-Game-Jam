using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera _camera; //for movement zoom out effects
    [SerializeField] private float moveSpeed, maxMoveSpeed, jumpStrength;
    private Vector3 moveDirection;
    private Transform surfacePlayerIsTouching;


    private bool isGrounded = true;
    private void Start()
    {
        _camera = Camera.main.GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        if (isGrounded)
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            MoveInDirection(direction);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {

    }
    void Jump()
    {
        //temp code not how jump will actually work

        rb.linearVelocity += Vector3.up * jumpStrength;
    }
    void MoveInDirection(Vector3 _direction)
    {
        Vector3 rMove = transform.right;
        Vector3 fMove = transform.forward;

        rb.linearVelocity += (rMove * _direction.x + fMove * _direction.z) * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        surfacePlayerIsTouching = other.transform;
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collider be triggering");
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if ( surfacePlayerIsTouching == other.transform)
        {
            surfacePlayerIsTouching = null;
            isGrounded = false;
        }
    }
}
