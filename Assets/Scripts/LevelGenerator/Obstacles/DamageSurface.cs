using System;
using UnityEngine;

public class DamageSurface : MonoBehaviour
{
    [SerializeField] private float _damageNumber;
    [SerializeField] private float _damgeIntervelsSec = .25f;
    private float t;
   
    private void OnCollisionStay(Collision collision)
    {
        t -= Time.deltaTime;
        if (collision.gameObject.GetComponent<IDamageable>() != null && t <= 0)
        {
            t = _damgeIntervelsSec;
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(_damageNumber);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        t = 0;
    }
}
