using System;
using System.Xml.Serialization;
using UnityEngine;

public class DamageSurface : MonoBehaviour
{
    [SerializeField] private float _damageNumber;
    [SerializeField] private float _damgeIntervelsSec = .25f;
    [SerializeField] private bool _respawnFloor = false;
    private bool _trippleDamage;
    private float _doubleDamage;
    private float t;
   
    void Start()
    {
        _doubleDamage = _damageNumber * 2;
        EventCore.instance.UE_DoubleDamage.AddListener(DoubleDamage);
        EventCore.instance.UE_EnterEndElevator.AddListener(ResetDamage);
        EventCore.instance.UE_TrippleDamge.AddListener(TrippleDamage);
    }

    private void OnCollisionStay(Collision collision)
    {
        t -= Time.deltaTime;
        if (collision.gameObject.GetComponent<IDamageable>() != null && t <= 0)
        {
            t = _damgeIntervelsSec;
            float _tempDamge = _damageNumber;
            if (_trippleDamage)
            {
                _tempDamge = collision.gameObject.GetComponent<IDamageable>().maxHealth/3;
            } 

            collision.gameObject.GetComponent<IDamageable>().TakeDamage(_tempDamge);
            if (_respawnFloor)
                EventCore.instance.UE_RespawnPlayer.Invoke(collision.transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        t = 0;
    }

    private void ResetDamage(Vector3 arg)
    {
        _damageNumber = _doubleDamage / 2;
        _trippleDamage = false;
    }

    private void DoubleDamage()
    {
        _damageNumber = _doubleDamage;
    }

    private void TrippleDamage()
    {
        _trippleDamage = true;
    }
}
