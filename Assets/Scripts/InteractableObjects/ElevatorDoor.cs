using UnityEngine;

public class ElevatorDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private float _openAmount = 5;
    [SerializeField] private float _stayOpenTime = 1;
    private Vector3 _oPos;
    private Vector3 _cPos;

    public void Exacute()
    {
        while (transform.position.y < _oPos.y)
        {
            transform.position = Vector3.Lerp(_cPos, _oPos, Time.deltaTime);
        }
        
        transform.position = _oPos;
        float t = 0;

        while (t < _stayOpenTime)
        {
            t += Time.deltaTime;
        }

        while (transform.position.y > _cPos.y)
        {
            transform.position = Vector3.Lerp(_oPos, _cPos, Time.deltaTime);
        }

        transform.position = _cPos;
    }

    void Start()
    {
        _cPos = transform.position;
        _oPos = new(transform.position.x, transform.position.y * _openAmount,transform.position.z);
    }

}
