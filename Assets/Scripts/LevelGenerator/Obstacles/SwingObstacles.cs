using UnityEngine;

public class SwingObstacles : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;
    [SerializeField] Transform _rightApex;
    [SerializeField] Transform _leftApex;
    [SerializeField] AnimationCurve _curve;
    private float resetValue = 1;  
    Quaternion _rotationR;
    Quaternion _rotationL;
    float t;

    bool changeRotation = false;
    float _doubleRotationAmount;
    void Start()
    {
        EventCore.instance.UE_EnterEndElevator.AddListener(ReturnMovement);
        EventCore.instance.UE_DoubleObsticalSpeed.AddListener(DoubleMovementSpeed);
        _doubleRotationAmount = _rotationSpeed * 2;

        _rotationR = _rightApex.rotation;
        _rotationL = _leftApex.rotation;
    }

    void Update()
    {
        RoateAxe();
    }

    private void DoubleMovementSpeed()
    {
        _rotationSpeed = _doubleRotationAmount;
    }

    private void ReturnMovement(Vector3 arg)
    {
        _rotationSpeed = _doubleRotationAmount / 2;
    }
    private void RoateAxe()
    {
        t += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(_rotationR, _rotationL, _curve.Evaluate(t / _rotationSpeed));
        

        if (t / _rotationSpeed >= resetValue)
        {
            t = 0;
        }

    }

}
