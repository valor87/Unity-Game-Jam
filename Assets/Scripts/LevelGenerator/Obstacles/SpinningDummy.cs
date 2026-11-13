using UnityEngine;

public class SpinningDummy : MonoBehaviour
{
    [SerializeField] private float _bufferTime;
    [SerializeField] private int _amountOfSpins;
    [SerializeField] private float _rotationSpeed;
    private float _rotatinoSpeedDoubled;
    private float _rotationSpeedHalfed;
    private bool _windUp = true;
    private float _rotationTarget = 0;
    private float _currentIntervelTime;
    void Start()
    {
        _rotatinoSpeedDoubled = _rotationSpeed * 2;
        _rotationSpeedHalfed = _rotationSpeed / 2;
        EventCore.instance.UE_EnterEndElevator.AddListener(ReturnMovement);
        EventCore.instance.UE_DoubleObsticalSpeed.AddListener(DoubleMovementSpeed);
        EventCore.instance.UE_HalfObsticalSpeed.AddListener(HalfMovementSpeed);
    }
    private void DoubleMovementSpeed()
    {
        _rotationSpeed = _rotatinoSpeedDoubled;
    }
    private void HalfMovementSpeed()
    {
        _rotationSpeed = _rotationSpeedHalfed;
    }
    private void ReturnMovement(Vector3 arg)
    {
        _rotationSpeed = _rotatinoSpeedDoubled / 2;
    }
    void Update()
    {
        // spins the player
        Spin();
        // when the timers complete restart the rotation
        if (_currentIntervelTime >= _bufferTime)
        {
            StartSpin();
        }
    }

    [ContextMenu("spinDummy")]
    // reset values
    void StartSpin()
    {
        _windUp = true;
        _currentIntervelTime = 0;
        _rotationTarget = 0;
    }
    //spin the dummy
    void Spin()
    {
        // start the windup
        if (_windUp)
        {
            
            if (_rotationTarget <= 30 * _amountOfSpins)
            {
                // rotate the dummy
                transform.eulerAngles = new Vector3(0, _rotationTarget, 0);
                _rotationTarget += 2 * _rotationSpeed * Time.deltaTime;
            }
            else 
            {
                // set the new rotation for the dummy
                _rotationTarget = 390 * _amountOfSpins;
                _windUp = false;
            }
        }
        // preform the new rotation
        if (!_windUp)
        {
            if (_rotationTarget > 0)
            {
                //rotate the dummy
                transform.eulerAngles = new Vector3(0, _rotationTarget, 0);
                _rotationTarget -= 5 * _rotationSpeed * Time.deltaTime;
                return;
            }
            _currentIntervelTime += 1 * Time.deltaTime;
        }
    }
}
