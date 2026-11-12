using UnityEngine;

public class SwingObstacles : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _rightApex = 80;
    [SerializeField] float _leftApex = 260;


    float _rotationAmount = 25;

    bool _sR=false;
    void Start()
    {
        EventCore.instance.UE_EnterEndElevator.AddListener(ReturnMovement);
        EventCore.instance.UE_DoubleObsticalSpeed.AddListener(DoubleMovementSpeed);
    }
    private void DoubleMovementSpeed()
    {
        _rotationAmount = _rotationAmount * 2;
    }

    private void ReturnMovement(Vector3 arg)
    {
        _rotationAmount = _rotationAmount / 2;
    }
    void Update()
    {
        if (_sR == false)
        {
            transform.eulerAngles += new Vector3(0, 0, _rotationAmount) * _rotationSpeed * Time.deltaTime;
            if (Mathf.Round(transform.eulerAngles.z) >= _rightApex && Mathf.Round(transform.eulerAngles.z) <= _rightApex + 50)
            {
                _sR = true;
            }
        }

        if (_sR == true)
        {
            transform.eulerAngles -= new Vector3(0, 0, _rotationAmount) * _rotationSpeed * Time.deltaTime;
            if (Mathf.Round(transform.eulerAngles.z) <= _leftApex && Mathf.Round(transform.eulerAngles.z) >= _leftApex - 50)
            {
                _sR = false;
            }
        }
    }

  
}
