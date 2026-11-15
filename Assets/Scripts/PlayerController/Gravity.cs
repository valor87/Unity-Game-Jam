using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Rigidbody rb;
    public float gravityScale, lowGravityScale, windScale = 0.4f;
    private float originalGravityScale;
    private Vector3 wind = Vector3.zero;
    [SerializeField] ParticleSystem windParticles;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalGravityScale = gravityScale;
        EventCore.instance.UE_StartWindEffect.AddListener(EnableWindEffect);
        EventCore.instance.UE_EnterEndElevator.AddListener(DisableWindEffect);
        EventCore.instance.UE_Cloud9.AddListener(EnableCloud9);
        EventCore.instance.UE_EnterEndElevator.AddListener(DisableCloud9);
    }
    void FixedUpdate()
    {
        rb.linearVelocity += (Vector3.down * gravityScale) + (wind * windScale);
    }
    private void EnableWindEffect()
    {
        float randomX = Random.Range(0.5f, 1);
        float randomZ = Random.Range(0.5f, 1);
        wind = new Vector3(randomX, 0, randomZ);
        windParticles.transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, wind), Vector3.up);
        windParticles.Play();
    }
    private void DisableWindEffect(Vector3 arg)
    {
        windParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        wind = Vector3.zero;
    }
    private void EnableCloud9()
    {
        gravityScale = lowGravityScale;
    }
    private void DisableCloud9(Vector3 arg)
    {
        gravityScale = originalGravityScale;
    }
}
