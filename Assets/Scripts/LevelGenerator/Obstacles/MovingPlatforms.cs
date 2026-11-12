using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private float _platformSpeed;
    [SerializeField] private Transform _leftPosition;
    [SerializeField] private Transform _rightPosition;
    private Transform _plat;
    private float _moveAmount = 2;
    void Start()
    {
        _plat = transform.GetChild(0).transform;
        EventCore.instance.UE_EnterEndElevator.AddListener(ReturnMovement);
        EventCore.instance.UE_DoubleObsticalSpeed.AddListener(DoubleMovementSpeed);
    }
    private void DoubleMovementSpeed()
    {
        _platformSpeed = _platformSpeed * 2;
    }

    private void ReturnMovement(Vector3 arg)
    {
        _platformSpeed = _platformSpeed / 2;
    }
    void Update()
    {
        _plat.localPosition += new Vector3(_moveAmount,0,0) * _platformSpeed * Time.deltaTime;
        
        if (_plat.localPosition.x > _rightPosition.localPosition.x || _plat.localPosition.x < _leftPosition.localPosition.x)
        {
            _moveAmount = _moveAmount * -1;
        }
    }
}
