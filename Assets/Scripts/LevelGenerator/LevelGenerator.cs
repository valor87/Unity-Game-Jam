using UnityEngine;
public enum propType
{
    None, Roof, Ground, Floating
}

[System.Serializable]
public struct propStruct
{
    public propType thisPropType;
    public GameObject propObject;
}

public class LevelGenerator : MonoBehaviour
{
    [Header("Spawn locations")]
    [Tooltip("Top corner location for the transforms")]
    [SerializeField] private Transform _trSpot1;
    [Tooltip("Bottom corner location oppisite form the one above")]
    [SerializeField] private Transform _trSpot2;
    [SerializeField] private float _roofOffset = 50;
    [Header("Prop infomation")]
    [SerializeField] private propStruct[] _allSpawnAbleProps = new propStruct[10];

    #region amount of props
    [Header("----------------------------------------------------------------------------")]
    public int _amountOfRoofProps;
    public int _amountOfFloatingProps;
    public int _amountOfGroundProps;
    #endregion

    void Start()
    {

    }

    private void Update()
    {
        debugDrawBoxBounds(_trSpot1, _trSpot2, Color.green);
    }

    [ContextMenu("Spawn Objects")]
    private void spawnProps()
    {
        spawnPropsLoop(propType.Roof);
        spawnPropsLoop(propType.Floating);
        spawnPropsLoop(propType.Ground);
    }

    private void spawnPropsLoop(propType _p)
    {
        for (int i = 0; i < _amountOfRoofProps; i++)
        {
            Vector3 _rV = Vector3.zero;
            GameObject _gO = null;

            switch (_p)
            {
                case propType.Roof:
                    _rV = new Vector3(Random.Range(_trSpot1.position.x, _trSpot2.position.x),
                                          Random.Range(_trSpot1.position.y - (_roofOffset * 1.5f), _trSpot1.position.y - _roofOffset),
                                          Random.Range(_trSpot1.position.z, _trSpot2.position.z));
                    break;
                case propType.Ground:
                    _rV = new Vector3(Random.Range(_trSpot1.position.x, _trSpot2.position.x),
                                          _trSpot2.position.y,
                                          Random.Range(_trSpot1.position.z, _trSpot2.position.z));
                    break;
                case propType.Floating:
                    _rV = new Vector3(Random.Range(_trSpot1.position.x, _trSpot2.position.x),
                                         Random.Range(_trSpot1.position.y - (_roofOffset * 1.5f), _trSpot2.position.y + _roofOffset + (_roofOffset * 1.5f)),
                                         Random.Range(_trSpot1.position.z, _trSpot2.position.z));
                    break;
            }
            
            while (true)
            {
                int _r = Random.Range(0, _allSpawnAbleProps.Length);
                if (_allSpawnAbleProps[_r].thisPropType == _p)
                {
                    _gO = Instantiate(_allSpawnAbleProps[_r].propObject);
                    break;
                }
            }
            _gO.transform.position = _rV;
            _gO.GetComponent<Prop>().pT = _p;
            _gO.GetComponent<Prop>().lg = this;

        }
    }
    // for drawing a box of the spawnable objects
    private void debugDrawBoxBounds(Transform _t1, Transform _t2, Color _c)
    {
        Vector3 _tL = _t1.position;
        Vector3 _bL = _t2.position;

        Vector3 _v1 = _tL;
        Vector3 _v2 = new Vector3(_tL.x, _tL.y, _bL.z);
        Vector3 _v3 = new Vector3(_bL.x, _tL.y, _bL.z);
        Vector3 _v4 = new Vector3(_bL.x, _tL.y, _tL.z);

        Vector3 _v7 = _bL;
        Vector3 _v5 = new Vector3(_tL.x, _bL.y, _tL.z);
        Vector3 _v6 = new Vector3(_tL.x, _bL.y, _bL.z);
        Vector3 _v8 = new Vector3(_bL.x, _bL.y, _tL.z);

        Debug.DrawLine(_v1, _v2, _c);
        Debug.DrawLine(_v2, _v3, _c);
        Debug.DrawLine(_v3, _v4, _c);
        Debug.DrawLine(_v4, _v1, _c);
        Debug.DrawLine(_v5, _v6, _c);
        Debug.DrawLine(_v6, _v7, _c);
        Debug.DrawLine(_v7, _v8, _c);
        Debug.DrawLine(_v8, _v5, _c);

        Debug.DrawLine(_v1, _v5, _c);
        Debug.DrawLine(_v2, _v6, _c);
        Debug.DrawLine(_v3, _v7, _c);
        Debug.DrawLine(_v4, _v8, _c);
    }
}
