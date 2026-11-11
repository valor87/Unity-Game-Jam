using Unity.VisualScripting;
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
    [SerializeField] private int _amountOfRoofProps;
    [SerializeField] private int _amountOfFloatingProps;
    [SerializeField] private int _amountOfGroundProps;
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
        for (int i = 0; i < _amountOfRoofProps; i++)
        {
            Vector3 _rV = new Vector3(Random.Range(_trSpot1.position.x, _trSpot2.position.x),
                                      Random.Range(_trSpot1.position.y - (_roofOffset * 1.5f), _trSpot1.position.y - _roofOffset),
                                      Random.Range(_trSpot1.position.z, _trSpot2.position.z));
            GameObject _gO = null;
            while(true)
            {
                int _r = Random.Range(0, _allSpawnAbleProps.Length);
                if (_allSpawnAbleProps[_r].thisPropType == propType.Roof)
                {
                    _gO = Instantiate(_allSpawnAbleProps[_r].propObject);
                    break;
                }
            }

            _gO.transform.position = _rV;
        }

    }

    private void spawnPropsLoop()
    {

    }
    // for drawing a box of the spawnable objects
    private void debugDrawBoxBounds(Transform t1, Transform t2, Color c)
    {
        Vector3 _topLocation = t1.position;
        Vector3 _bottomLocation = t2.position;

        Vector3 _v1 = _topLocation;
        Vector3 _v2 = new Vector3(_topLocation.x, _topLocation.y, _bottomLocation.z);
        Vector3 _v3 = new Vector3(_bottomLocation.x, _topLocation.y, _bottomLocation.z);
        Vector3 _v4 = new Vector3(_bottomLocation.x, _topLocation.y, _topLocation.z);

        Vector3 _v5 = new (_topLocation.x, _bottomLocation.y, _topLocation.z);
        Vector3 _v6 = new Vector3(_topLocation.x, _bottomLocation.y, _bottomLocation.z);
        Vector3 _v7 = new Vector3(_bottomLocation.x, _bottomLocation.y, _bottomLocation.z);
        Vector3 _v8 = new Vector3(_bottomLocation.x, _bottomLocation.y, _topLocation.z);

        Debug.DrawLine(_v1, _v2, c);
        Debug.DrawLine(_v2, _v3, c);
        Debug.DrawLine(_v3, _v4, c);
        Debug.DrawLine(_v4, _v1, c);
        Debug.DrawLine(_v5, _v6, c);
        Debug.DrawLine(_v6, _v7, c);
        Debug.DrawLine(_v7, _v8, c);
        Debug.DrawLine(_v8, _v5, c);

        Debug.DrawLine(_v1, _v5, c);
        Debug.DrawLine(_v2, _v6, c);
        Debug.DrawLine(_v3, _v7, c);
        Debug.DrawLine(_v4, _v8, c);
    }

}
