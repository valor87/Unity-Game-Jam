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
    }

    private void Update()
    {
        if (isGrounded)
        {
            MoveInDirection(moveVector());
            Debug.Log(rb.linearVelocity);
            
        }
        
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
   
    void Jump()
    {
        //temp code not how jump will actually work later
        rb.linearVelocity += Vector3.up * jumpStrength;
    }
    Vector3 moveVector()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }
    void MoveInDirection(Vector3 _direction)
    {
        Vector3 rMove = transform.right;
        Vector3 fMove = transform.forward;

        rb.linearVelocity +=  (rMove * _direction.x + fMove * _direction.z) * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}
