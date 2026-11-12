using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private float _triggerDistence;
    private Transform _pT;
    private void Start()
    {
        _pT = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, _pT.position) < _triggerDistence)
            EventCore.instance.UE_EnterEndElevator.Invoke();
    }
}
