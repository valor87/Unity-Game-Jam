using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Rigidbody rb;
    public float gravityScale = 9.81f, windScale = 0.4f;
    private Vector3 wind = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        EventCore.instance.UE_StartWindEffect.AddListener(EnableWindEffect);
        EventCore.instance.UE_EnterEndElevator.AddListener(DisableWindEffect);
    }
    void FixedUpdate()
    {
        rb.linearVelocity += Vector3.down * gravityScale + wind * windScale;
    }
    private void EnableWindEffect()
    {
        float randomX = Random.Range(0, 1);
        float randomZ = Random.Range(0, 1);
        wind = new Vector3(randomX, 0, randomZ);
    }
    private void DisableWindEffect(Vector3 arg)
    {
        wind = Vector3.zero;
    }
}
