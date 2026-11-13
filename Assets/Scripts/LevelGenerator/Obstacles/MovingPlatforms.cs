using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private float _platformSpeed;
    [SerializeField] private Transform _leftPosition;
    [SerializeField] private Transform _rightPosition;
    private Transform _plat;
    private float _moveAmount = 2;
    private float _moveAmountDoubled;
    private float _moveAmountHalved;

    void Start()
    {
        _moveAmountDoubled = _moveAmount * 2;
        _moveAmountHalved = _moveAmount / 2;
        _plat = transform.GetChild(0).transform;
        EventCore.instance.UE_EnterEndElevator.AddListener(ReturnMovement);
        EventCore.instance.UE_DoubleObsticalSpeed.AddListener(DoubleMovementSpeed);
        EventCore.instance.UE_HalfObsticalSpeed.AddListener(HalfMovementSpeed);
    }
    private void DoubleMovementSpeed()
    {
        _platformSpeed = _moveAmountDoubled;
    }
    private void HalfMovementSpeed()
    {
        _platformSpeed = _moveAmountHalved;
    }
    private void ReturnMovement(Vector3 arg)
    {
        _platformSpeed = _moveAmountDoubled / 2;
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
