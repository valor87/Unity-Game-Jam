using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private float _platformSpeed;
    [SerializeField] private Transform _leftPosition;
    [SerializeField] private Transform _rightPosition;
    private Transform _plat;
    private float _mA = 2;
    void Start()
    {
        _plat = transform.GetChild(0).transform;
    }

    void Update()
    {
        _plat.position += new Vector3(_mA,0,0) * _platformSpeed * Time.deltaTime;
        
        if (_plat.position.x > _rightPosition.position.x || _plat.position.x < _leftPosition.position.x)
        {
            _mA = _mA * -1;
        }
    }
}
