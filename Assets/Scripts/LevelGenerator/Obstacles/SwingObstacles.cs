using UnityEngine;

public class SwingObstacles : MonoBehaviour
{
    [SerializeField] float _rotationTime;
    [SerializeField] Transform _rightApex;
    [SerializeField] Transform _leftApex;
    [SerializeField] AnimationCurve _curve;
    private float resetValue = 1;  
    Quaternion _rotationR;
    Quaternion _rotationL;
    float _baseRotationTime;
    float t;

    bool changeRotation = false;
    float _doubleRotationTimeAmount;
    void Start()
    {
        EventCore.instance.UE_EnterEndElevator.AddListener(ResetRotationTime);
        EventCore.instance.UE_DoubleObsticalSpeed.AddListener(DoubleMovement);
        EventCore.instance.UE_HalfObsticalSpeed.AddListener(HalfMovementSpeed);
        _doubleRotationTimeAmount = _rotationTime * 2;
        _baseRotationTime = _rotationTime;
        _rotationR = _rightApex.rotation;
        _rotationL = _leftApex.rotation;
    }

    void Update()
    {
        RoateAxe();
    }

    private void HalfMovementSpeed()
    {
        _rotationTime = _doubleRotationTimeAmount;
    }

    private void DoubleMovement()
    {
        _rotationTime = _doubleRotationTimeAmount / 2;
    }
    private void ResetRotationTime(Vector3 arg)
    {
        _rotationTime = _baseRotationTime;
    }
    private void RoateAxe()
    {
        t += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(_rotationR, _rotationL, _curve.Evaluate(t / _rotationTime));
        

        if (t / _rotationTime >= resetValue)
        {
            t = 0;
        }

    }

}
