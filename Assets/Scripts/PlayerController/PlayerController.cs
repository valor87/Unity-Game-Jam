using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera _camera; //for movement zoom out effects
    [SerializeField] private float moveSpeed, maxMoveSpeed, jumpStrength;
    private bool isGrounded = true;
    private void Start()
    {
        _camera = Camera.main.GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxMoveSpeed;
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (isGrounded)
        {
            MoveInDirection(transform.TransformDirection(moveDirection));
        }
        
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpStrength);
    }

    void MoveInDirection(Vector3 _direction)
    {
        rb.linearVelocity += _direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Something entered me trigger!");
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Something left me trigger!");
    }
}
