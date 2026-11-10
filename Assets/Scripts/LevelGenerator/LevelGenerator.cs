using UnityEngine;
public enum propType
{
    None,
    Roof,
    Ground,
    Floating
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

    [Header("Prop infomation")]
    [SerializeField] private int _amountOfRoofProps;
    [SerializeField] private propStruct[] _allSpawnAbleProps = new propStruct[10];

    void Start()
    {

    }

    private void Update()
    {
        debugDrawBoxBounds(Color.green);
    }

    [ContextMenu("Spawn Objects")]
    private void spawnProps()
    {
        for (int i = 0; i < _amountOfRoofProps; i++)
        {
            int _r = Random.Range(0, _allSpawnAbleProps.Length);
            Vector3 _rV = new Vector3(Random.Range(_trSpot1.position.x, _trSpot2.position.x),
                                      Random.Range(_trSpot1.position.y, _trSpot2.position.y),
                                      Random.Range(_trSpot1.position.z, _trSpot2.position.z));

            GameObject _gO = Instantiate(_allSpawnAbleProps[_r].propObject);
            _gO.transform.position = _rV;
        }
    }


    // for drawing a box of the spawnable objects
    private void debugDrawBoxBounds(Color c)
    {
        Vector3 _topLocation = _trSpot1.position;
        Vector3 _bottomLocation = _trSpot2.position;

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
