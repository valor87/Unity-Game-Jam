using System;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Rigidbody rb;
    public float gravityScale = 9.81f, windScale = 0f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        rb.linearVelocity += Vector3.down * gravityScale + Vector3.right * windScale;
    }
}
