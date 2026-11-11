using UnityEngine;

public class ElevatorDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private float _openAmount = 5;
    [SerializeField] private float _stayOpenTime = 1;

    private Vector3 _oPos;
    private Vector3 _cPos;
    
    private Transform _dTrans;

    private float _oT = 0;
    private float _cT = 0;
    private float _t = 0;

    public void Exacute()
    {
        _dTrans = transform;
    }
    
    void Start()
    {
        _cPos = transform.position;
        _oPos = new(transform.position.x, transform.position.y * _openAmount,transform.position.z);
    }

    void Update()
    {
        if (_dTrans != null)
        {
            if (transform.position.y < _oPos.y && _oT/_stayOpenTime < 0.99f)
            {
                _oT += Time.deltaTime;
                transform.position = Vector3.Lerp(_cPos, _oPos, _oT/_stayOpenTime);
                return;
            }

            if (_t < _stayOpenTime)
            {
                _t += Time.deltaTime;
                return;
            }

            if (transform.position.y > _cPos.y && _cT / _stayOpenTime < 0.99f)
            {
                _cT += Time.deltaTime;
                transform.position = Vector3.Lerp(_oPos, _cPos, _cT/_stayOpenTime);
                return;
            }

            transform.position = _cPos;
            resetDoorValues();
        }
    }
    private void resetDoorValues()
    {
        _dTrans = null;
        _oT = 0;
        _cT = 0;
        _t = 0;
    }
}
